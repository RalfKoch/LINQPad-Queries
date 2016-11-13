<Query Kind="Program" />

class test
{
	public string personalNummer { get; set; }
	public string firma { get; set; }
	public string kennziffer { get; set; }
	public int von { get; set; }
	public int bis { get; set; }
	public int sollstunden { get; set; }
	public int iststunden { get {return bis - von;} }
	public IList<string> kennziffernVertraeglichkeit { get; set; }
	public string kennziffernVertraeglichkeitString { get {return string.Join(",", kennziffernVertraeglichkeit.ToArray());}}
}

void Main()
{
	// Diese Methoden werden ausgeführt:
	ExpressionTest01();
	ExpressionTest02();
	ExpressionTest03();
}

void ExpressionTest01()
{
	var item = new test();

	item.personalNummer = "100123456";
	item.firma = "10";
	//object condition = "personalNummer.StartsWith(firma)";
	object condition = item.personalNummer == string.Empty;

	// This expression represents a conditional operation. 
	// It evaluates the test (first expression) and
	// executes the iftrue block (second argument) if the test evaluates to true, 
	// or the iffalse block (third argument) if the test evaluates to false.
	Expression conditionExpr = Expression.Condition(
							   Expression.Constant(condition),
							   Expression.Constant("Firma ist enthalten"),
							   Expression.Constant("Firma ist eben nicht enthalten")
							 );

	Console.WriteLine("Methode 'ExpressionTest01':");

	// Print out the expression.
	Console.WriteLine(conditionExpr.ToString());

	// The following statement first creates an expression tree,
	// then compiles it, and then executes it.       
	Console.WriteLine(Expression.Lambda<Func<string>>(conditionExpr).Compile()());
	Console.WriteLine("---");
}

void ExpressionTest02()
{
	var item = new test();

	item.personalNummer = "100123456";
	item.firma = "10";

	Console.WriteLine("Methode 'ExpressionTest02':");
	var lambda = GetExpression<test>("Personalnummer", "10", FilterType.StartsWith);
	Console.WriteLine(string.Format("StartsWith: {0}", lambda.Compile()(item)));

	lambda = GetExpression<test>("Personalnummer", "56", FilterType.EndsWith);
	Console.WriteLine(string.Format("EndsWith: {0}", lambda.Compile()(item)));

	lambda = GetExpression<test>("Personalnummer", "123", FilterType.Contains);
	Console.WriteLine(string.Format("Contains: {0}", lambda.Compile()(item)));

	lambda = GetExpression<test>("Personalnummer", "15", FilterType.StartsWith);
	Console.WriteLine(string.Format("StartsWith: {0}", lambda.Compile()(item)));

	lambda = GetExpression<test>("Personalnummer", "10", FilterType.EndsWith);
	Console.WriteLine(string.Format("EndsWith: {0}", lambda.Compile()(item)));

	lambda = GetExpression<test>("Personalnummer", "1235", FilterType.Contains);
	Console.WriteLine(string.Format("Contains: {0}", lambda.Compile()(item)));
	Console.WriteLine("---");
}

void ExpressionTest03()
{
	var item = new test();

	item.personalNummer = "100123456";
	item.firma = "10";
	item.kennziffernVertraeglichkeit = new[] { "00", "01", "02", "03", "04", "05", };

	Console.WriteLine("Methode 'ExpressionTest03':");
	var lambda = GetExpression<test>("KennziffernString", "06", FilterType.Contains);
	Console.WriteLine(string.Format("Contains 06: {0}", lambda.Compile()(item)));
	lambda = GetExpression<test>("KennziffernString", "02", FilterType.Contains);
	Console.WriteLine(string.Format("Contains 02: {0}", lambda.Compile()(item)));
	Console.WriteLine("---");
}

void ExpressionTest04()
{
	var item = new test();

	item.personalNummer = "100123456";
	item.firma = "10";
	item.kennziffernVertraeglichkeit = new[] { "00", "01", "02", "03", "04", "05", };

	Console.WriteLine("Methode 'ExpressionTest04':");
	var lambda = GetExpression<test>("kennziffernVertraeglichkeitString", "06", FilterType.Contains);
	Console.WriteLine(string.Format("Contains 06: {0}", lambda.Compile()(item)));
	lambda = GetExpression<test>("kennziffernVertraeglichkeitString", "02", FilterType.Contains);
	Console.WriteLine(string.Format("Contains 02: {0}", lambda.Compile()(item)));
	Console.WriteLine("---");
}

static Expression<Func<T, bool>> GetExpression<T>(string propertyName, string propertyValue)
{
	var parameterExp = Expression.Parameter(typeof(T), "type");
	var propertyExp = Expression.Property(parameterExp, propertyName);
	MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
	var someValue = Expression.Constant(propertyValue, typeof(string));
	var containsMethodExp = Expression.Call(propertyExp, method, someValue);

	return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
}

static Expression<Func<T, bool>> GetExpression<T>(string propertyName, string propertyValue, FilterType filterType)
{
	var parameterExp = Expression.Parameter(typeof(T), "type");
	var propertyExp = Expression.Property(parameterExp, propertyName);
	MethodInfo method;

	switch (filterType)
	{
		case FilterType.StartsWith:
			method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
			break;

		case FilterType.EndsWith:
			method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
			break;

		default:
			method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
			break;
	}
	var someValue = Expression.Constant(propertyValue, typeof(string));
	var containsMethodExp = Expression.Call(propertyExp, method, someValue);

	return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
}


private enum FilterType
{
	StartsWith,
	EndsWith,
	Contains
}