<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>


for (int i = 1; i <= 12; i++)
{
	Console.WriteLine(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i));
	Console.WriteLine(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
}

