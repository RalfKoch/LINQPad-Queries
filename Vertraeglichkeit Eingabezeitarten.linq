<Query Kind="Program" />

void Main()
{
	var kennziffern = FillKennziffern();
	var compatibilities = FillCompatibilities();
	
}

// Erzeugen einer Liste mit Kennziffern
IList<string> FillKennziffern()
{
	var query = new List<string>();
	query.Add("00");
	query.Add("01");
	query.Add("02");
	query.Add("03");
	query.Add("04");
	
	return query;
}

// Erzeugen einer Liste mit Kompatibilitäten
IList<Compatibility> FillCompatibilities()
{
	var query = new List<Compatibility>();
	var item = new Compatibility();
	item.BasisCode = "00";
	item.Code = "00";
	item.Compatible = "Y";
	item.Condition = string.Empty;
	query.Add(item);

	item = new Compatibility();
	item.BasisCode = "00";
	item.Code = "01";
	item.Compatible = "Y";
	item.Condition = string.Empty;
	query.Add(item);

	item = new Compatibility();
	item.BasisCode = "00";
	item.Code = "02";
	item.Compatible = "N";
	item.Condition = string.Empty;
	query.Add(item);

	item = new Compatibility();
	item.BasisCode = "00";
	item.Code = "03";
	item.Compatible = "C";
	item.Condition = "";
	query.Add(item);

	return query;
}

// Kompatibilität
// Eine Eingabezeitart mit Ihrer Kompatibilität mit einer anderen Eingabezeitart.
public class Compatibility{
	public string BasisCode {get;set;}
	public string Code {get;set;}
	public string Compatible {get;set;}
	public string Condition {get;set;}
}

// Das ist ein zeitlicher Eintrag im Stundennachweis
public class Termin {
	public DateTime StartInterval { get; set; }
	public DateTime EndInterval { get; set; }
	public bool GanzerTag { get; set; }
	public Eingabezeitart Eingabezeitart { get; set; }
}

// Die Eingabezeitart fuer einen bestimmten Bereich am Tag
public class Eingabezeitart {
	public string Code { get; set; }
	
}