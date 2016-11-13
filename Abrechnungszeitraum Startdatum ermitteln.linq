<Query Kind="Statements" />

for (int i = 1; i <= 12; i++)
{
	var value = new DateTime(2016, i, 10);
	var month = value.Month;

	if (month % 2 == 0)
		month -= 1;

	Console.WriteLine(string.Format("Datum: {0}   Abrechnungszeitraum Start: {1}", value, new DateTime(value.Year, month, 1)));
}
