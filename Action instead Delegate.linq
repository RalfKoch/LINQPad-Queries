<Query Kind="Program" />

// Using Action instead of Delegate
void Main()
{
	Action<int> printActionDel = ConsolePrint;
	printActionDel(10);
}

// Define other methods and classes here
static void ConsolePrint(int i)
{
	Console.WriteLine(i);
}

//// this would use delegate as usual
//public delegate void Print(int val);
//
//static void ConsolePrint(int i)
//{
//	Console.WriteLine(i);
//}
//
//static void Main(string[] args)
//{
//	Print prnt = ConsolePrint;
//	Prnt(10);
//}