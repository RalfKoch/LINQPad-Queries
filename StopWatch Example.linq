<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading</Namespace>
</Query>

class Program
{
	static void Main()
	{
		// Create new stopwatch.
		Stopwatch stopwatch = new Stopwatch();

		// Begin timing.
		stopwatch.Start();


		// Do something.
		for (int i = 0; i < 1000; i++)
		{
			Thread.Sleep(1);
		}

		// Stop timing.
		stopwatch.Stop();

		// Write result.
		Console.WriteLine("Seconds elapsed: {0}", stopwatch.Elapsed.TotalSeconds);
		Console.WriteLine("Minutes elapsed: {0}", stopwatch.Elapsed.TotalMinutes);
	}
}