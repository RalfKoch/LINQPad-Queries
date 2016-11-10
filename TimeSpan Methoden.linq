<Query Kind="Statements" />

TimeSpan value;

value = TimeSpan.FromHours(2.00);
Console.WriteLine(value);
Console.WriteLine(value.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", value.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", value.TotalMinutes));
Console.WriteLine("---");

value = TimeSpan.FromHours(2.25);
Console.WriteLine(value);
Console.WriteLine(value.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", value.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", value.TotalMinutes));
Console.WriteLine("---");

value = TimeSpan.FromHours(2.50);
Console.WriteLine(value);
Console.WriteLine(value.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", value.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", value.TotalMinutes));
Console.WriteLine("---");

value = TimeSpan.FromHours(2.75);
Console.WriteLine(value);
Console.WriteLine(value.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", value.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", value.TotalMinutes));
Console.WriteLine("---");
