<Query Kind="Statements" />

// What is TimeSpan?
// It represents a length of time

// Konstruktoren
Console.WriteLine("TimeSpan(Hours, Minutes, Seconds)");
Console.WriteLine("TimeSpan(Int32, Int32, Int32)");
Console.WriteLine();
Console.WriteLine("TimeSpan(Days, Hours, Minutes, Seconds)");
Console.WriteLine("TimeSpan(Int32, Int32, Int32, Int32)");
Console.WriteLine();
Console.WriteLine("TimeSpan(Days, Hours, Minutes, Seconds, Milliseconds)");
Console.WriteLine("TimeSpan(Int32, Int32, Int32, Int32, Int32)");
Console.WriteLine();
Console.WriteLine("TimeSpan(Ticks)");
Console.WriteLine("TimeSpan(Int64)");
Console.WriteLine();

// Methoden der TimeSpan Struktur
TimeSpan baseValue;
TimeSpan value1;
TimeSpan value2;

// Add
// Return a new TimeSpan object whose value is the sum of the specified TimeSpan object and this instance.
baseValue = new TimeSpan(1,0,0);
value1 = new TimeSpan(0,30,0);
Console.WriteLine("baseValue = new TimeSpan(1,0,0);");
Console.WriteLine("value1 = new TimeSpan(0,30,0);");
Console.WriteLine("value.Add(value1).TotalHours");
Console.WriteLine(string.Format("Addiere {0} Stunden zu diesen Wert {1}: {2} Stunden.", value1.TotalHours, baseValue.TotalHours, baseValue.Add(value1).TotalHours));
Console.WriteLine();

// Compare
// 

// Duration

// FromDays
baseValue = TimeSpan.FromDays(2.5);
Console.WriteLine("Code: baseValue = TimeSpan.FromDays(2.5);");
Console.WriteLine("Auswertungen:");
Console.WriteLine(string.Format("baseValue.TotalHours: {0}", baseValue.TotalHours));
Console.WriteLine(string.Format("baseValue.TotalMinutes: {0}", baseValue.TotalMinutes));
Console.WriteLine("---");

// FromHours
baseValue = TimeSpan.FromHours(2.25);
Console.WriteLine("Code: baseValue = TimeSpan.FromHours(2.25);");
Console.WriteLine("Auswertungen:");
Console.WriteLine(baseValue);
Console.WriteLine(baseValue.ToString("h\\,mm"));
Console.WriteLine(string.Format("baseValue.TotalHours: {0}", baseValue.TotalHours));
Console.WriteLine(string.Format("baseValue.TotalMinutes: {0}", baseValue.TotalMinutes));
Console.WriteLine("---");

// FromMilliseconds

// FromMinutes

// FromSeconds

// FromTicks

// Parse

// ParseExact [mit Überladungen]

// ToString


// Felder der TimeSpan Struktur
// Zero
// Stellt den Timespan-Wert für 0 (null) dar. Schreibgeschützt.
Console.WriteLine(string.Format("TimeSpan Zero: {0}", TimeSpan.Zero));

// MaxValue
// Stellt den maximalen TimeSpan-Wert dar. Schreibgeschützt.
Console.WriteLine(string.Format("TimeSpan MaxValue: {0}", TimeSpan.MaxValue));

// MinValue
// stellt den minimalen TimeSpan-Wert dar. Schreibgeschützt.
Console.WriteLine(string.Format("TimeSpan MinValue: {0}", TimeSpan.MinValue));

// TicksPerDay
// Stellt die Anzahl der Ticks prop Tag dar. Konstant.
Console.WriteLine(string.Format("TimeSpan TicksPerDay: {0}", TimeSpan.TicksPerDay));

// TicksPerHour
// Stellt die Anzahl der Ticks pro Stunde dar. Konstant.
Console.WriteLine(string.Format("TimeSpan TicksPerHour: {0}", TimeSpan.TicksPerHour));

// TicksPerMinute
// Stellt die Anzahl der Ticks pro Minute dar. Konstant.
Console.WriteLine(string.Format("TimeSpan TicksPerMinute: {0}", TimeSpan.TicksPerMinute));

// TicksPerSecond
// Stellt die Anzahl der Ticks prop Sekunde dar. Konstant.
Console.WriteLine(string.Format("TimeSpan TicksPerSecond: {0}", TimeSpan.TicksPerSecond));

// TicksPerMillisecond
// Stellt die Anzahl der Ticks pro Millisekunde dar. Konstant.
Console.WriteLine(string.Format("TimeSpan TicksPerMillisecond: {0}", TimeSpan.TicksPerMillisecond));



baseValue = TimeSpan.FromHours(2.00);
Console.WriteLine(baseValue);
Console.WriteLine(baseValue.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", baseValue.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", baseValue.TotalMinutes));
Console.WriteLine("---");

baseValue = TimeSpan.FromHours(2.25);
Console.WriteLine(baseValue);
Console.WriteLine(baseValue.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", baseValue.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", baseValue.TotalMinutes));
Console.WriteLine("---");

baseValue = TimeSpan.FromHours(2.50);
Console.WriteLine(baseValue);
Console.WriteLine(baseValue.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", baseValue.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", baseValue.TotalMinutes));
Console.WriteLine("---");

baseValue = TimeSpan.FromHours(2.75);
Console.WriteLine(baseValue);
Console.WriteLine(baseValue.ToString("h\\,mm"));
Console.WriteLine(string.Format("Totale Stunden: {0}", baseValue.TotalHours));
Console.WriteLine(string.Format("Totale Minuten: {0}", baseValue.TotalMinutes));
Console.WriteLine("---");