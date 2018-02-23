<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\SMDiagnostics.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.Internals.dll</Reference>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Linq.Expressions</Namespace>
  <Namespace>System.Reflection</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
  <Namespace>System.Runtime.Serialization.Json</Namespace>
  <Namespace>System.Text.RegularExpressions</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	// Alle Regeln in einer Liste anlegen.
	// Dann kann man bestimmte Regeln nach Namen oder nach Groupen extrahieren und anwenden.
	Rule rule = new Rule()
	{
		Operation = ExpressionType.AndAlso.ToString("g"),
		Rules = new List<Rule>()
				{
					new Rule(){ Property = "mitarbeiter.Kennziffer", Value = "00", Operation = ExpressionType.Equal.ToString("g")},
					new Rule(){
						Operation = ExpressionType.Or.ToString("g"),
						Rules = new List<Rule>(){
							new Rule(){ Property = "mitarbeiter.IsAdmin", Value = "true", Operation = ExpressionType.Equal.ToString("g")},
							new Rule(){ Property = "mitarbeiter.IsBB", Value = "true", Operation = ExpressionType.Equal.ToString("g")}
						}
					}
				}
	};

	var controller = new RuleEngineController();
	Console.WriteLine("As XML Format:");
	controller.SaveRuleAsXml(rule);
	Console.WriteLine();
	Console.WriteLine("As JSON Format:");
	controller.SaveRulesAsJson(rule);

	return;
}

//// Define other methods and classes here
//
///// Author: Cole Francis, Architect
///// The pre-compiled rules type
///// 
//public static class PrecompiledRules
//{
//	///
//	/// A method used to precompile rules for a provided type
//	/// 
//	public static List<Func<T, bool>> CompileRule<T>(List<T> targetEntity, List<Rule> rules)
//	{
//		var compiledRules = new List<Func<T, bool>>();
//
//		// Loop through the rules and compile them against the properties of the supplied shallow object 
//		rules.ForEach(rule =>
//		{
//			var genericType = Expression.Parameter(typeof(T));
//			var key = MemberExpression.Property(genericType, rule.Property);
//			var propertyType = typeof(T).GetProperty(rule.Property).PropertyType;
//			var value = Expression.Constant(Convert.ChangeType(rule.Value, propertyType));
//			var binaryExpression = Expression.MakeBinary(rule.Operation, key, value);
//
//			//compiledRules.Add(Expression.Lambda<Func>(binaryExpression, genericType).Compile());
//			compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
//		});
//
//		// Return the compiled rules to the caller
//		return compiledRules;
//	}
//
//	///
//	/// A method used to precompile rules for a provided type
//	/// 
//	public static List<Func<T, bool>> CompileRule<T>(List<Rule> rules)
//	{
//		var compiledRules = new List<Func<T, bool>>();
//
//		// Loop through the rules and compile them against the properties of the supplied shallow object 
//		rules.ForEach(rule =>
//		{
//			var genericType = Expression.Parameter(typeof(T));
//			var key = MemberExpression.Property(genericType, rule.Property);
//			var test = typeof(T);
//			var test1 = test.GetProperty("Year");
//			var propertyType = typeof(T).GetProperty(rule.Property).PropertyType;
//			var value = Expression.Constant(Convert.ChangeType(rule.Value, propertyType));
//			var binaryExpression = Expression.MakeBinary(rule.Operation, key, value);
//
//			//compiledRules.Add(Expression.Lambda<Func>(binaryExpression, genericType).Compile());
//			compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
//		});
//
//		// Return the compiled rules to the caller
//		return compiledRules;
//	}
//}

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
	public string Property { get; set; }

	// Der Vergleichsoperand der Regel, der die Eigenschaft (Prädikat) mit dem Wert vergleicht.
	public string Operation { get; set; }

	// Der Wert, mit der die Eigenschaft der Regel verglichen werden soll.
	public string Value { get; set; }

	public List<Rule> Rules { get; set; }

	public List<object> Inputs { get; set; }

	/// 
	/// The rule method that 
	/// 
	public Rule()
	{
	}

	public Rule(string name, string property, string operation, string value)
	{
		Name = name;
		Property = property;
		Operation = operation;
		Value = value;
	}

	public Rule(string name, string group, string property, string operation, string value)
	{
		Name = name;
		Group = group;
		Property = property;
		Operation = operation;
		Value = value;
	}

	public Rule(string name, string group, string description, string property, string operation, string value)
	{
		Name = name;
		Group = group;
		Description = description;
		Property = property;
		Operation = operation;
		Value = value;
	}
}

public class Mitarbeiter
{
	public string Kennziffer { get; set; }

	public bool IsAdmin { get; set; }

	public bool IsBB { get; set; }
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

public interface IRuleEngineController
{
	void SaveRuleAsXml(Rule rule);

	void SaveRuleAsXml(List<Rule> rules);

	void SaveRulesAsJson(Rule rule);

	void SaveRulesAsJson(List<Rule> rules);

	void DeleteRule(Rule rule);

	void GetRuleByName(string name);
	
	bool ExecuteRuleByName<T>(string name, T value);
}

/// <summary>
/// Hier werden die Regeln ausgelesen und ausgeführt, dass Ergebnis wird an den Aufruf zurückgegeben.
/// </summary>
/// <seealso cref="Flink.RuleEngine.IRuleEngineController" />
public class RuleEngineController
{
	private RuleEngine RuleEngine { get; set; }

	private List<Rule> Rules { get; set; }

	public RuleEngineController()
	{
		RuleEngine = new RuleEngine();
	}

	public void SaveRuleAsXml(Rule rule)
	{
		if (rule == null) throw new ArgumentNullException("Es wurde keine Regel übergeben");

		var ruleString = SerializationHelper<Rule>.SerializeObjectToXml(rule, Encoding.UTF8);

		Console.WriteLine(ruleString);
	}

	public void SaveRuleAsXml(List<Rule> rules)
	{
		if (rules == null) throw new ArgumentNullException("Es wurde keine Regel übergeben");

		var ruleString = SerializationHelper<List<Rule>>.SerializeObjectToXml(rules, Encoding.UTF8);

		Console.WriteLine(ruleString);
	}

	public void SaveRulesAsJson(Rule rule)
	{
		if (rule == null) throw new ArgumentNullException("Es wurde keine Regel übergeben");

		var ruleString = SerializationHelper<Rule>.SerializeObjectToJson(rule, Encoding.UTF8);

		Console.WriteLine(ruleString);
	}

	public void SaveRulesAsJson(List<Rule> rules)
	{
		if (rules == null) throw new ArgumentNullException("Es wurde keine Regel übergeben");

		var ruleString = SerializationHelper<List<Rule>>.SerializeObjectToJson(rules, Encoding.UTF8);

		Console.WriteLine(ruleString);
	}

	public void DeleteRule(Rule rule)
	{
		throw new NotImplementedException();
	}

	public Rule GetRuleByName(string name)
	{
		if (Rules?.Count <= 0) return null;
		return Rules.Find(x => x.Name == name);
	}

	public bool ExecuteRuleByName<T>(string name, T value)
	{
		if (RuleEngine == null)
			RuleEngine = new RuleEngine();

		var rule = GetRuleByName(name);
		if (rule == null) throw new ArgumentNullException($"No such rule by the name '{name}' found!");
		
		var ruling = RuleEngine.CompileRule<T>(rule);
		return ruling(value);
	}
}

/// <summary>
/// Helper class to serialize objects to various formats.
/// Currenty we are supporting XML and JSON.
/// </summary>
public class SerializationHelper<T>
{
	#region Serialize Object XML
	/// <summary>
	///   Serialize an [object] to an Xml String.
	/// </summary>
	/// <typeparam name="T">Object Type to Serialize</typeparam>
	/// <param name="obj">Object Type to Serialize</param>
	/// <param name="encoding">System.Text.Encoding Type</param>
	/// <returns>Empty.String if Exception, XML string if successful</returns>
	/// <example>
	///   // UTF-16 Serialize
	///   string xml = SerializationHelper<ObjectType>SerializeObjectToXml( [object], new UnicodeEncoding( false, false ) );
	/// </example>
	/// <example>
	///   // UTF-8 Serialize
	///   string xml = SerializationHelper<ObjectType>SerializeObjectToXml( [object], Encoding.UTF8 );
	/// </example> 
	public static string SerializeObjectToXml(T obj, Encoding encoding)
	{
		if (obj == null) { return string.Empty; }

		try
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			using (var memoryStream = new MemoryStream())
			{
				var xmlWriterSettings = new XmlWriterSettings() { Encoding = encoding };
				using (var writer = XmlWriter.Create(memoryStream, xmlWriterSettings))
				{
					xmlSerializer.Serialize(writer, obj);
				}

				return encoding.GetString(memoryStream.ToArray());
			}
		}
		catch (InvalidOperationException ex)
		{
			// Write something in a logger if there is one...
			return string.Empty;
		}
		catch (Exception ex)
		{
			// Write something in a logger if there is one...
			return string.Empty;
		}
	}

	/// <summary>
	///   Deserialize an Xml String to an [object]
	/// </summary>
	/// <typeparam name="T">Object Type to Deserialize</typeparam>
	/// <param name="xml">Xml String to Deserialize</param>
	/// <param name="encoding">System.Text.Encoding Type</param>
	/// <returns>Default if Exception, Deserialize object if successful</returns>
	/// <example>
	///   // UTF-16 Deserialize
	///   [object] = SerializationHelper<ObjectType>DeserializeObjectFromXml( xml, Encoding.Unicode )
	/// </example>
	/// <example>
	///   // UTF-8 Deserialize
	///   [object] = SerializationHelper<ObjectType>DeserializeObjectFromXml( xml, Encoding.UTF8 )
	/// </example> 
	public static T DeserializeObjectFromXml(string xml, Encoding encoding)
	{
		if (string.IsNullOrEmpty(xml)) { return default(T); }

		try
		{
			using (var memoryStream = new MemoryStream(encoding.GetBytes(xml)))
			{
				var xmlSerializer = new XmlSerializer(typeof(T));
				var xmlReaderSettings = new XmlReaderSettings();
				using (XmlReader xmlReader = XmlReader.Create(memoryStream, xmlReaderSettings))
				{
					return (T)xmlSerializer.Deserialize(xmlReader);
				}
			}
		}
		catch (InvalidOperationException ex)
		{
			// Write something in a logger if there is one...
			return default(T);
		}
		catch (Exception ex)
		{
			// Write something in a logger if there is one...
			return default(T);
		}
	}
	#endregion

	#region Serialize Object JSON
	/// <summary>
	///   Serialize an [object] to a Json String.
	/// </summary>
	/// <typeparam name="T">Object Type to Serialize</typeparam>
	/// <param name="obj">Object Type to Serialize</param>
	/// <param name="encoding">System.Text.Encoding Type</param>
	/// <returns>Empty.String if Exception, JSON string if successful</returns>
	/// <example>
	///   // UTF-16 Serialize
	///   string json = SerializationHelper<ObjectType>SerializeObjectToJson( [object], new UnicodeEncoding( false, false ) );
	/// </example>
	/// <example>
	///   // UTF-8 Serialize
	///   string json = SerializationHelper<ObjectType>SerializeObjectToJson( [object], Encoding.UTF8 );
	/// </example> 
	public static string SerializeObjectToJson(T obj, Encoding encoding)
	{
		if (obj == null) { return string.Empty; }

		try
		{
			//Create a stream to serialize the object to.  
			using (MemoryStream ms = new MemoryStream())
			{
				var jsonSerializer = new DataContractJsonSerializer(typeof(T));
				jsonSerializer.WriteObject(ms, obj);
				byte[] json = ms.ToArray();
				return encoding.GetString(json, 0, json.Length);
			}
		}
		catch (SerializationException ex)
		{
			// Write something in a logger if there is one...
			return string.Empty;
		}
		catch (Exception ex)
		{
			// Write something in a logger if there is one...
			return string.Empty;
		}
	}

	/// <summary>
	///   Deserialize an Json String to an [object]
	/// </summary>
	/// <typeparam name="T">Object Type to Deserialize</typeparam>
	/// <param name="json">Json String to Deserialize</param>
	/// <param name="encoding">System.Text.Encoding Type</param>
	/// <returns>Default if Exception, Deserialize object if successful</returns>
	/// <example>
	///   // UTF-16 Deserialize
	///   [object] = SerializationHelper<ObjectType>DeserializeObjectFromJson( json, Encoding.Unicode )
	/// </example>
	/// <example>
	///   // UTF-8 Deserialize
	///   [object] = SerializationHelper<ObjectType>DeserializeObjectFromJson( json, Encoding.UTF8 )
	/// </example> 
	public static T DeserializeObjectFromJson(string json, Encoding encoding)
	{
		if (string.IsNullOrEmpty(json)) { return default(T); }

		try
		{
			using (MemoryStream ms = new MemoryStream(encoding.GetBytes(json)))
			{
				var jsonSerializer = new DataContractJsonSerializer(typeof(T));
				return (T)jsonSerializer.ReadObject(ms);
			}
		}
		catch (SerializationException ex)
		{
			// Write something in a logger if there is one...
			return default(T);
		}
		catch (Exception ex)
		{
			// Write something in a logger if there is one...
			return default(T);
		}
	}
	#endregion
}

public class RuleEngine
{
	private ExpressionType[] nestedOperators = { ExpressionType.And, ExpressionType.AndAlso, ExpressionType.Or, ExpressionType.OrElse };

	public bool PassesRules<T>(IList<Rule> rules, T toInspect)
	{
		return CompileRules<T>(rules).Invoke(toInspect);
	}

	public Func<T, bool> CompileRule<T>(Rule r)
	{
		var paramUser = Expression.Parameter(typeof(T));
		Expression expr = GetExpressionForRule<T>(r, paramUser);

		return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
	}

	Expression GetExpressionForRule<T>(Rule r, ParameterExpression param)
	{
		ExpressionType nestedOperator;
		if (ExpressionType.TryParse(r.Operation, out nestedOperator) && nestedOperators.Contains(nestedOperator) && r.Rules != null && r.Rules.Any())
			return BuildNestedExpression<T>(r.Rules, param, nestedOperator);

		return BuildExpr<T>(r, param);
	}

	public Func<T, bool> CompileRules<T>(IList<Rule> rules)
	{
		var paramUser = Expression.Parameter(typeof(T));
		var expr = BuildNestedExpression<T>(rules, paramUser, ExpressionType.And);
		return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
	}

	private Expression BuildNestedExpression<T>(IList<Rule> rules, ParameterExpression param, ExpressionType operation)
	{
		var expressions = new List<Expression>();
		foreach (var r in rules)
		{
			expressions.Add(GetExpressionForRule<T>(r, param));
		}

		var expr = BinaryExpression(expressions, operation);
		return expr;
	}

	private Expression BinaryExpression(IList<Expression> expressions, ExpressionType operationType)
	{
		Func<Expression, Expression, Expression> methodExp = (x1, x2) => Expression.And(x1, x2);
		switch (operationType)
		{
			case ExpressionType.Or:
				methodExp = (x1, x2) => Expression.Or(x1, x2);
				break;
			case ExpressionType.OrElse:
				methodExp = (x1, x2) => Expression.OrElse(x1, x2);
				break;
			case ExpressionType.AndAlso:
				methodExp = (x1, x2) => Expression.AndAlso(x1, x2);
				break;
		}

		if (expressions.Count == 1)
			return expressions[0];

		var exp = methodExp(expressions[0], expressions[1]);
		for (int i = 2; expressions.Count > i; i++)
		{
			exp = methodExp(exp, expressions[i]);
		}

		return exp;
	}

	private Expression AndExpressions(IList<Expression> expressions)
	{
		if (expressions.Count == 1)
			return expressions[0];

		var exp = Expression.And(expressions[0], expressions[1]);
		for (var i = 2; expressions.Count > i; i++)
		{
			exp = Expression.And(exp, expressions[i]);
		}

		return exp;
	}

	private Expression OrExpressions(IList<Expression> expressions)
	{
		if (expressions.Count == 1)
			return expressions[0];

		var exp = Expression.Or(expressions[0], expressions[1]);
		for (var i = 2; expressions.Count > i; i++)
		{
			exp = Expression.Or(exp, expressions[i]);
		}

		return exp;
	}

	private Expression BuildExpr<T>(Rule r, ParameterExpression param)
	{
		Expression propExpression;
		Type propType;

		if (string.IsNullOrEmpty(r.Property))//check is against the object itself
		{
			propExpression = param;
			propType = propExpression.Type;
		}
		else if (r.Property.Contains('.'))//Child property
		{
			var childProperties = r.Property.Split('.');
			var property = typeof(T).GetTypeInfo().GetDeclaredProperty(childProperties[0]);

			//// Original:
			//var property = typeof(T).GetProperty(childProperties[0]);

			var paramExp = Expression.Parameter(typeof(T), "SomeObject");

			propExpression = Expression.PropertyOrField(param, childProperties[0]);
			for (var i = 1; i < childProperties.Length; i++)
			{
				if (property == null) continue;

				var orig = property;

				property = property.PropertyType.GetRuntimeProperty(childProperties[i]);

				//// Original:
				//property = property.PropertyType.GetProperty(childProperties[i]);
				propExpression = Expression.PropertyOrField(propExpression, childProperties[i]);
			}
			propType = propExpression.Type;
		}
		else//Property
		{
			propExpression = Expression.PropertyOrField(param, r.Property);
			propType = propExpression.Type;
		}

		// is the operator a known .NET operator?
		if (Enum.TryParse(r.Operation, out ExpressionType tBinary))
		{
			var right = this.StringToExpression(r.Value, propType);
			return Expression.MakeBinary(tBinary, propExpression, right);
		}

		if (r.Operation == "IsMatch")
		{
			return Expression.Call(
				typeof(Regex).GetTypeInfo().GetDeclaredMethod("IsMatch"),
				propExpression,
				Expression.Constant(r.Value, typeof(string)),
				Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions))
				);

			//// Original:
			//return Expression.Call(
			//    typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string), typeof(RegexOptions) }),
			//    propExpression,
			//    Expression.Constant(r.TargetValue, typeof(string)),
			//    Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions))
			//);

		}

		//Invoke a method on the Property
		var inputs = r.Inputs.Select(x => x.GetType()).ToArray();
		var methodInfo = propType.GetTypeInfo().GetDeclaredMethod(r.Operation);

		//// Original:
		//var methodInfo = propType.GetMethod(r.Operator, inputs);

		if (!methodInfo.IsGenericMethod)
			inputs = null;//Only pass in type information to a Generic Method
		var expressions = r.Inputs.Select(x => Expression.Constant(x)).ToArray();
		return Expression.Call(propExpression, r.Operation, inputs, expressions);
	}

	private Expression StringToExpression(string value, Type propType)
	{
		return Expression.Constant(value.ToLower() == "null"
			? null
			: Convert.ChangeType(value, propType));
	}
}