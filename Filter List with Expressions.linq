<Query Kind="Expression" />

var allFiltered = Filter(AllCustomer, "Name", "Moumit");

public static List<T> Filter<T>(this List<T> Filterable, string PropertyName, object ParameterValue)
{
	ConstantExpression c = Expression.Constant(ParameterValue);
	ParameterExpression p = Expression.Parameter(typeof(T), "xx");
	MemberExpression m = Expression.PropertyOrField(p, PropertyName);
	var Lambda = Expression.Lambda<Func<T, Boolean>>(Expression.Equal(c, m), new[] { p });
	Func<T, Boolean> func = Lambda.Compile();
	return Filterable.Where(func).ToList();
}