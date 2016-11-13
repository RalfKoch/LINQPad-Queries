<Query Kind="Program" />

void Main()
{
	dynamic dynamicVariable = 100;
	Console.WriteLine("Dynamic variable value: {0}, Type: {1}", dynamicVariable, dynamicVariable.GetType().ToString());

	dynamicVariable = "Hello World!!";
	Console.WriteLine("Dynamic variable value: {0}, Type: {1}", dynamicVariable, dynamicVariable.GetType().ToString());

	dynamicVariable = true;
	Console.WriteLine("Dynamic variable value: {0}, Type: {1}", dynamicVariable, dynamicVariable.GetType().ToString());

	dynamicVariable = DateTime.Now;
	Console.WriteLine("Dynamic variable value: {0}, Type: {1}", dynamicVariable, dynamicVariable.GetType().ToString());
	
	Console.WriteLine("---");
	PrintValue("Now we are using a method with a dynamic method parameter:");
	PrintValue(100);
	PrintValue(100.50);
	PrintValue(true);
	PrintValue(DateTime.Now);
}

// A method with a dynamic parameter
void PrintValue(dynamic val)
{
	Console.WriteLine(val);
}
