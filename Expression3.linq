<Query Kind="Program" />

public class Story
{
	public string Author { get; set; }

	public IList<string> Contributors { get; set; }
}

public enum FilterOperator
{
	Contains,
	IsEqualTo,
	IsNotEqualTo,
	IsGreaterThan,
	IsGreaterThanOrEqualTo,
	IsLessThan,
	IsLessThanOrEqualTo,
	StartsWith,
	EndsWith
}

public enum LogicalOperator { 
	And,
	Or
}

void Main()
{
	var stories = new List<Story>();
	var filters = new List<FilterCriteria>();
	filter.Add(new FilterCriteria { ValueToCompare = "Test", PropertyToCompare = "Author", FilterOperator = FilterOperator.Contains });

	Expression<Func<string, bool>> func = t => t.Contains("Test");

	filter.Add(new FilterCriteria { ValueToCompare = "Test", PropertyToCompare = "Contributors", FilterOperator = FilterOperator.Contains, Expression = func });

	stories.Filter(filters, LogicalOperator.Or).ToList();
}

public interface IFilterCriteria
{
	string PropertyToCompare { get; set; }
	object ValueToCompare { get; set; }
	FilterOperator FilterOperator { get; set; }
	bool IsList { get; set; }
	Expression Expression { get; set; }
}

public class FilterCriteria
{
	public string PropertyToCompare { get; set; }
	public object ValueToCompare { get; set; }
	public FilterOperator FilterOperator { get; set; }
	public bool IsList { get; set; }
	public Expression Expression { get; set; }
}

public static IQueryable<T> Filter<T>(this IQueryable<T> query, IList<IFilterCriteria> filterCriterias, LogicalOperator logicalOperator = LogicalOperator.And)
{
	if (filterCriterias != null && filterCriterias.Any())
	{
		var resultCondition = filterCriterias.ToExpression(query, logicalOperator);

		var parameter = Expression.Parameter(query.ElementType, "p");

		if (resultCondition != null)
		{
			var lambda = Expression.Lambda(resultCondition, parameter);

			var mce = Expression.Call(
				typeof(Queryable), "Where",
				new[] { query.ElementType },
				query.Expression,
				lambda);

			return query.Provider.CreateQuery<T>(mce);
		}
	}
	return query;
}

public static Expression ToExpression<T>(this IList<IFilterCriteria> filterCriterias, IQueryable<T> query, LogicalOperator logicalOperator = LogicalOperator.And)
{
	Expression resultCondition = null;
	if (filterCriterias.Any())
	{
		var parameter = Expression.Parameter(query.ElementType, "p");

		foreach (var filterCriteria in filterCriterias)
		{
			var propertyExpression = filterCriteria.PropertyToCompare.Split('.').Aggregate<string, MemberExpression>(null, (current, property) => Expression.Property(current ?? (parameter as Expression), property));

			Expression valueExpression;
			var constantExpression = Expression.Constant(filterCriteria.ValueToCompare);

			if (!filterCriteria.IsList)
			{
				valueExpression = Expression.Convert(constantExpression, propertyExpression.Type);
			}
			else
			{
				valueExpression = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) },
												  propertyExpression, filterCriteria.Expression,
												  Expression.Constant(filterCriteria.ValueToCompare,
																	  typeof(string)));
			}

			Expression condition;
			switch (filterCriteria.FilterOperator)
			{
				case FilterOperator.IsEqualTo:
					condition = Expression.Equal(propertyExpression, valueExpression);
					break;
				case FilterOperator.IsNotEqualTo:
					condition = Expression.NotEqual(propertyExpression, valueExpression);
					break;
				case FilterOperator.IsGreaterThan:
					condition = Expression.GreaterThan(propertyExpression, valueExpression);
					break;
				case FilterOperator.IsGreaterThanOrEqualTo:
					condition = Expression.GreaterThanOrEqual(propertyExpression, valueExpression);
					break;
				case FilterOperator.IsLessThan:
					condition = Expression.LessThan(propertyExpression, valueExpression);
					break;
				case FilterOperator.IsLessThanOrEqualTo:
					condition = Expression.LessThanOrEqual(propertyExpression, valueExpression);
					break;
				case FilterOperator.Contains:
					// if collection
					if (propertyExpression.Type.IsGenericType &&
						typeof(IEnumerable<>)
							.MakeGenericType(propertyExpression.Type.GetGenericArguments())
							.IsAssignableFrom(propertyExpression.Type))
					{
						// find AsQueryable method
						var toQueryable = typeof(Queryable).GetMethods()
							.Where(m => m.Name == "AsQueryable")
							.Single(m => m.IsGenericMethod)
							.MakeGenericMethod(typeof(string));

						// find Any method
						var method = typeof(Queryable).GetMethods()
							.Where(m => m.Name == "Any")
							.Single(m => m.GetParameters().Length == 2)
							.MakeGenericMethod(typeof(string));

						// make expression
						condition = Expression.Call(
							null,
							method,
							Expression.Call(null, toQueryable, propertyExpression),
							filterCriteria.Expression
						);
					}
					else
					{
						condition = Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), valueExpression);
					}
					break;
				case FilterOperator.StartsWith:
					condition = Expression.Call(propertyExpression, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), valueExpression);
					break;
				case FilterOperator.EndsWith:
					condition = Expression.Call(propertyExpression, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), valueExpression);
					break;
				default:
					condition = valueExpression;
					break;
			}

			if (resultCondition != null)
			{
				switch (logicalOperator)
				{
					case LogicalOperator.And:
						resultCondition = Expression.AndAlso(resultCondition, condition);
						break;
					case LogicalOperator.Or:
						resultCondition = Expression.OrElse(resultCondition, condition);
						break;
				}
			}
			else
			{
				resultCondition = condition;
			}
		}
	}
	return resultCondition;
}