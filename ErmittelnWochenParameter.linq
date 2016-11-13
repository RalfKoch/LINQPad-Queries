<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	var currentDate = new DateTime(2016, 7, 18);
	var beginningWeek = DateTime.MinValue;
	var endWeek = DateTime.MinValue;

	GetWeek(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche: {0} - {1}", beginningWeek, endWeek));
	GetWeekPreserveMonth(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche [Monat bleibt]: {0} - {1}", beginningWeek, endWeek));
	Console.WriteLine("-------------");

	currentDate = new DateTime(2016,7,2);
	GetWeek(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche: {0} - {1}", beginningWeek, endWeek));
	GetWeekPreserveMonth(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche [Monat bleibt]: {0} - {1}", beginningWeek, endWeek));
	Console.WriteLine("-------------");

	currentDate = new DateTime(2016, 7, 29);
	GetWeek(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche: {0} - {1}", beginningWeek, endWeek));
	GetWeekPreserveMonth(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche [Monat bleibt]: {0} - {1}", beginningWeek, endWeek));
	Console.WriteLine("-------------");

	currentDate = new DateTime(2016, 6, 29);
	GetWeek(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche: {0} - {1}", beginningWeek, endWeek));
	GetWeekPreserveMonth(currentDate, CultureInfo.CurrentCulture, out beginningWeek, out endWeek);
	Console.WriteLine(string.Format("Die Woche [Monat bleibt]: {0} - {1}", beginningWeek, endWeek));
	Console.WriteLine("-------------");

}

// Define other methods and classes here
public static void GetWeek(DateTime now, CultureInfo cultureInfo, out DateTime begining, out DateTime end)
{
	if (now == null)
		throw new ArgumentNullException("now");
	if (cultureInfo == null)
		cultureInfo = new CultureInfo("de-DE");

	var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
	var offset = firstDayOfWeek - now.DayOfWeek;

	if (offset != 1)
	{
		var weekStart = now.AddDays(offset);
		var endOfWeek = weekStart.AddDays(6);
		begining = weekStart;
		end = endOfWeek;
	}
	else
	{
		begining = now.AddDays(-6);
		end = now;
	}
}

public static void GetWeekPreserveMonth(DateTime now, CultureInfo cultureInfo, out DateTime begining, out DateTime end)
{
	if (now == null)
		throw new ArgumentNullException("now");
	if (cultureInfo == null)
		cultureInfo = new CultureInfo("de-DE");

	var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
	var offset = firstDayOfWeek - now.DayOfWeek;
	var month = now.Month;

	if (offset != 1)
	{
		var weekStart = now.AddDays(offset);
		var endOfWeek = weekStart.AddDays(6);
		begining = weekStart;
		end = endOfWeek;
	}
	else
	{
		begining = now.AddDays(-6);
		end = now;
	}
	
	if (begining.Month != month)
		begining = new DateTime(end.Year, end.Month, 1);
	else
		if (end.Month != month)
			end = new DateTime(begining.Year, month, 1).AddMonths(1).AddDays(-1);
}