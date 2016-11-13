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
		new Rule ( "Administrator", "Global", "IsAdmin", ExpressionType.Equal, "true"),
		new Rule ( "Betriebsbüro", "Global", "IsBB", ExpressionType.Equal, "true"),
		new Rule ( "Fachbereich", "Global", "Kennziffer", ExpressionType.Equal, "00"),
        new Rule ( "Fachbereich", "Global", "Kennziffer", ExpressionType.Equal, "01")
	};

	var compiledMakeModelYearRules = PrecompiledRules.CompileRule(new List<Car>(), rules);

	// Create a list to house your test cars
	List<Car> cars = new List<Car>();

	// Create a car that's year and model fail the rules validations      
	Car car1_Bad = new Car
	{
		Year = 2011,
		Make = "El Nino",
		Model = "Torche"
	};

	// Create a car that meets all the conditions of the rules validations
	Car car2_Good = new Car
	{
		Year = 2015,
		Make = "El Nino",
		Model = "Torch"
	};

	// Add your cars to the list
	cars.Add(car1_Bad);
	cars.Add(car2_Good);

	// Iterate through your list of cars to see which ones meet the rules vs. the ones that don't
	cars.ForEach(car =>
	{
		if (compiledMakeModelYearRules.TakeWhile(rule => rule(car)).Count() > 0)
		{
			Console.WriteLine(string.Concat("Car model: ", car.Model, " Passed the compiled rules engine check!"));
		}
		else
		{
			Console.WriteLine(string.Concat("Car model: ", car.Model, " Failed the compiled rules engine check!"));
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

public class Fachbereich : IEquatable<Fachbereich>, IValidatableObject
{
	#region Fields
	/// <summary>
	/// Die aktulle Kennziffernbezeichnung des Fachbereiches
	/// </summary>
	private string mKennziffer;

	private string mBezeichnung;

	#endregion

	/// <summary>
	/// Kennziffer aka Code des Fachbereiches
	/// </summary>
	/// <value>
	/// The kennziffer.
	/// </value>
	[Required]
	[StringLength(2, ErrorMessage = "Der Fachbereichscode kann nur 2 Zeichen enthalten!")]
	public string Kennziffer
	{
		get { return mKennziffer; }
		set
		{
			Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Kennziffer" });
			mKennziffer = value;
		}
	}

	/// <summary>
	/// Lesbare Bezeichnung des Fachbereiches.
	/// </summary>
	/// <value>
	/// The bezeichnung.
	/// </value>
	[Required]
	[StringLength(50, ErrorMessage = "Die Fachbereichsbezeichnung darf nur 50 Zeichen enthalten!")]
	public string Bezeichnung
	{
		get { return mBezeichnung; }
		set
		{
			Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Bezeichnung" });
			mBezeichnung = value;
		}
	}

	/// <summary>
	/// Liste der Bundeslaender mit denen dieser Fachbereich assoziiert werden kann.
	/// </summary>
	/// <value>
	/// The bundeslaender.
	/// </value>

	[Required]
	public string Bundesland { get; set; }

	public string KennzifferBestaetigungSn { get; set; }

	public string KennzifferBestaetigungIlv { get; set; }

	public string KennzifferVerrechnungSn { get; set; }

	public string KennzifferVerrechnungIlv { get; set; }

	public string KennzifferSystem { get; set; }

	public string KennzifferEbSn { get; set; }

	public string KennzifferEbIlv { get; set; }

	public string KennzifferIlv { get; set; }

	public string VerrechnungIlvBetriebsBuero { get; set; }

	public object Firma { get; set; }

	public DateTime StartDatumSn { get; set; }

	public DateTime StartDatumIlv { get; set; }

	/// <summary>
	/// Liefert oder Setzt das Flag, welches den Fachbereich in die ProduktionsDirektion legt oder nicht.
	/// </summary>
	/// <value>
	/// <c>true</c> wenn diese Instanz zur Produktions Direktion gehört; sonst, <c>false</c>.
	/// </value>
	public bool IsProduktionsDirektion { get; set; }

	/// <summary>
	/// Flag, das ILV sichtbar sein soll oder nicht.
	/// Wenn der Wert der Datenbank 'J' ist, dann hat der Fachbereich ILV Daten und diese sollen auch angezeigt werden.
	/// </summary>
	/// <value>
	///   <c>true</c> if [ilv is visible]; otherwise, <c>false</c>.
	/// </value>
	public bool IlvIsVisible
	{
		get
		{
			if (string.IsNullOrWhiteSpace(KennzifferIlv)) return false;
			return KennzifferIlv == "J";
		}
	}

	public Fachbereich()
	{
	}

	public Fachbereich(string _FBKz, string _FBBez, string _FBKzBestSN, string _FBKzBestILV, string _FBKzVerrSN, string _FBKzVerrILV, string _FBKzSystem, string _FBKzEBSN, string _FBKzEBILV, string _FBKzILV,
	string _FBKzVerrILV_BB_EB)
	{
		Kennziffer = _FBKz;
		Bezeichnung = _FBBez;
		KennzifferBestaetigungSn = _FBKzBestSN;
		KennzifferBestaetigungIlv = _FBKzBestILV;
		KennzifferVerrechnungSn = _FBKzVerrSN;
		KennzifferVerrechnungIlv = _FBKzVerrILV;
		KennzifferSystem = string.IsNullOrEmpty(_FBKzSystem) ? "G" : _FBKzSystem;
		KennzifferEbSn = _FBKzEBSN;
		KennzifferEbIlv = _FBKzEBILV;
		KennzifferIlv = _FBKzILV;
		VerrechnungIlvBetriebsBuero = _FBKzVerrILV_BB_EB;
	}

	public bool Equals(Fachbereich other)
	{
		if (other == null) return false;
		return !String.IsNullOrWhiteSpace(Kennziffer) && Kennziffer.Equals(other.Kennziffer);
	}

	public override int GetHashCode()
	{
		var hashKennziffer = Kennziffer.GetHashCode();

		return hashKennziffer;
	}

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		var results = new List<ValidationResult>();
		foreach (var property in GetType().GetProperties())
		{
			Validator.TryValidateProperty(property.GetValue(this, null), new ValidationContext(this, null, null) { MemberName = property.Name }, results);
			if (results.Count <= 0) continue;
			foreach (var err in results)
			{
				yield return new ValidationResult(err.ErrorMessage);
			}

			results.Clear();
		}
	}

	public override string ToString()
	{
		return string.Format("{0}: {1}", Kennziffer, Bezeichnung);
	}
}