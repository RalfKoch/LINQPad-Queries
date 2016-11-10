<Query Kind="Statements" />

string test = "Stichtag < 28.02.05 ungültig                                               ";
test = test.Trim();

string date = test.Replace("Stichtag < ","").Replace(" ungültig","");
string date2 = test.Substring(11,8);
DateTime datum = DateTime.Parse(date2);

Console.WriteLine(test);
Console.WriteLine(date);
Console.WriteLine(date2);
Console.WriteLine(datum);