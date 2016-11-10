<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
</Query>

void Main()
{
	// Alle Regeln in einer Liste anlegen.
	// Dann kann man bestimmte Regeln nach Namen oder nach Groupen extrahieren und anwenden.
	List<Rule> rules = new List<Rule>
	{
		// Create some rules using LINQ.ExpressionTypes for the comparison operators
		// IMPORTANT
		// Can we connect more than one rule with and Operator AND / OR ???
		// We need to do some research on that!!!
		// We also need to check if the rules can use the AND / OR operator, that would help a lot.
		new Rule ( "Administrator", "Global", "IsAdmin", ExpressionType.Equal, "true"),
		new Rule ( "Betriebsbüro", "Global", "IsBB", ExpressionType.Equal, "true"),
	};

	// Nachteil hier:
	// Diese definierten Regeln sind im Code vorhanden und müssen daher bei Modifikation neu ausgespielt werden
	// Diese Regeln sollten auch via einer Datenbank erstellt werden können und dann dynamisch eingelesen werden.
	// Dann werde diese alle durchlaufen und entsprechend reagiert.
	var compiledEnableButtonRules = PrecompiledRules.CompileRule(new List<Button>(), rules);

	// Create a list to house your test buttons
	List<Button> buttons = new List<Button>();

	var btnSave = new Button
	{
		Name = "btnSave",
		Enabled = false,
		Visible = true
	}

	var btnNew = new Button
	{
		Name = "btnNew",
		Enabled = false,
		Visible = true
	}
	
	// Add your buttons to the list
	buttons.Add(btnSave);
	buttons.Add(btnNew);

	// Iterate through your list of cars to see which ones meet the rules vs. the ones that don't
	buttons.ForEach(button =>
	{
		if (compiledEnableButtonRules.TakeWhile(rule => rule(button)).Count() > 0)
		{
			Console.WriteLine(string.Concat("Button Name: ", button.Name, " Passed the compiled rules enabled check!"));
		}
		else
		{
			Console.WriteLine(string.Concat("Button Name: ", button.Name, " Failed the compiled rules enabled check!"));
		}
	});
}

// Define other methods and classes here

/// Author: Cole Francis, Architect
/// The pre-compiled rules type
/// 
public static class PrecompiledRules
{
	///
	/// A method used to precompile rules for a provided type
	/// 
	public static List<Func<T, bool>> CompileRule<T>(List<T> targetEntity, List<Rule> rules)
	{
		var compiledRules = new List<Func<T, bool>>();

		// Loop through the rules and compile them against the properties of the supplied shallow object 
		rules.ForEach(rule =>
		{
			var genericType = Expression.Parameter(typeof(T));
			var key = MemberExpression.Property(genericType, rule.ComparisonPredicate);
			var propertyType = typeof(T).GetProperty(rule.ComparisonPredicate).PropertyType;
			var value = Expression.Constant(Convert.ChangeType(rule.ComparisonValue, propertyType));
			var binaryExpression = Expression.MakeBinary(rule.ComparisonOperator, key, value);

			//compiledRules.Add(Expression.Lambda<Func>(binaryExpression, genericType).Compile());
			compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
		});

		// Return the compiled rules to the caller
		return compiledRules;
	}

	///
	/// A method used to precompile rules for a provided type
	/// 
	public static List<Func<T, bool>> CompileRule<T>(List<Rule> rules)
	{
		var compiledRules = new List<Func<T, bool>>();

		// Loop through the rules and compile them against the properties of the supplied shallow object 
		rules.ForEach(rule =>
		{
			var genericType = Expression.Parameter(typeof(T));
			var key = MemberExpression.Property(genericType, rule.ComparisonPredicate);
			var test = typeof(T);
			var test1 = test.GetProperty("Year");
			var propertyType = typeof(T).GetProperty(rule.ComparisonPredicate).PropertyType;
			var value = Expression.Constant(Convert.ChangeType(rule.ComparisonValue, propertyType));
			var binaryExpression = Expression.MakeBinary(rule.ComparisonOperator, key, value);

			//compiledRules.Add(Expression.Lambda<Func>(binaryExpression, genericType).Compile());
			compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
		});

		// Return the compiled rules to the caller
		return compiledRules;
	}
}

///
/// The Rule type
/// 
public class Rule
{
	///
	/// Denotes the rules predictate (e.g. Name); comparison operator(e.g. ExpressionType.GreaterThan); value (e.g. "Cole")
	/// 

	// Name der Regel, dieser sollte die Regel beschreiben.
	public string Name { get; set; }

	// Name der Gruppe, zu der diese Regel gehört.
	public string Group { get; set; }

	// Beschreibung der Regel, damit sollte es klarer werden für den Anwender.
	public string Description { get; set; }

	// Name der Eigenschaft (Prädikat), die durch die Regel geprüft werden soll.
	public string ComparisonPredicate { get; set; }

	// Der Vergleichsoperand der Regel, der die Eigenschaft (Prädikat) mit dem Wert vergleicht.
	public ExpressionType ComparisonOperator { get; set; }

	// Der Wert, mit der die Eigenschaft der Regel verglichen werden soll.
	public string ComparisonValue { get; set; }

	/// 
	/// The rule method that 
	/// 
	public Rule(string name, string comparisonPredicate, ExpressionType comparisonOperator, string comparisonValue)
	{
		Name = name;
		ComparisonPredicate = comparisonPredicate;
		ComparisonOperator = comparisonOperator;
		ComparisonValue = comparisonValue;
	}

	public Rule(string name, string group, string comparisonPredicate, ExpressionType comparisonOperator, string comparisonValue)
	{
		Name = name;
		Group = group;
		ComparisonPredicate = comparisonPredicate;
		ComparisonOperator = comparisonOperator;
		ComparisonValue = comparisonValue;
	}

	public Rule(string name, string group, string description, string comparisonPredicate, ExpressionType comparisonOperator, string comparisonValue)
	{
		Name = name;
		Group = group;
		Description = description;
		ComparisonPredicate = comparisonPredicate;
		ComparisonOperator = comparisonOperator;
		ComparisonValue = comparisonValue;
	}
}

public class Button : IButton
{
	public string Name { get; set; }
	public bool Enabled { get; set; }
	public bool Visible { get; set; }
}

public class IButton
{
	string Name { get; set; }
	bool Enabled { get; set; }
	bool Visible { get; set; }
}

public class Car : ICar
{
	public int Year { get; set; }
	public string Make { get; set; }
	public string Model { get; set; }
}

public class ICar
{
	int Year { get; set; }
	string Make { get; set; }
	string Model { get; set; }
}