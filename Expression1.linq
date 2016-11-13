<Query Kind="Program" />

class test
{
	public string personalNummer { get; set; }
	public string firma { get; set; }
}

void Main()
{
	var item = new test();

	item.personalNummer = "100123456";
	item.firma = "10";
	object condition = "personalNummer.StartsWith(firma)";

	// This expression represents a conditional operation. 
	// It evaluates the test (first expression) and
	// executes the iftrue block (second argument) if the test evaluates to true, 
	// or the iffalse block (third argument) if the test evaluates to false.
	Expression conditionExpr = Expression.Condition(
							   Expression.Constant(condition),
							   Expression.Constant("Firma ist enthalten"),
							   Expression.Constant("Firma ist eben nicht enthalten")
							 );

	// Print out the expression.
	Console.WriteLine(conditionExpr.ToString());
	
	// The following statement first creates an expression tree,
	// then compiles it, and then executes it.       
	Console.WriteLine(Expression.Lambda<Func<string>>(conditionExpr).Compile()());
}


