<Query Kind="Program" />

void Main()
{
	for (int i = 0; i < 100; i++)
	{
		// Declare a new argument object.
		ThreadInfo threadInfo = new ThreadInfo();
		threadInfo.Message = "Jesus loves you";
		threadInfo.Title = i.ToString();

		// Send the custom object to the threaded method.
		ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile), threadInfo);
	}
}

// Code to execute which would take a while, therefore in threads...
// Lock on this object.
readonly object _countLock = new object();

private void ProcessFile(object a)
{
	// Constrain the number of worker threads
	while (true)
	{
		// Prevent other threads from changing this under us
		lock (_countLock)
		{
			if (_threadCount < 4)
			{
				// Start the processing
				_threadCount++;
				break;
			}
		}
		Thread.Sleep(50);
	}
	// We receive the threadInfo as an uncasted object.
	// Use the 'as' operator to cast it to ThreadInfo.
	ThreadInfo threadInfo = a as ThreadInfo;
	string message = threadInfo.Message;
	string title = threadInfo.Title;
	
	Console.WriteLine(title + ": " + message);
	Thread.Sleep(1500);
}


// Define other methods and classes here
// Special class that is an argument to the ThreadPool method.
class ThreadInfo
{
	public string Message { get; set; }
	public string Title { get; set; }
}