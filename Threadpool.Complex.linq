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
private void ProcessFile(object a)
{
	// Constrain the number of worker threads
	// (Omitted here.)

	// We receive the threadInfo as an uncasted object.
	// Use the 'as' operator to cast it to ThreadInfo.
	ThreadInfo threadInfo = a as ThreadInfo;
	string message = threadInfo.Message;
	string title = threadInfo.Title;
	
	Console.WriteLine(title + ": " + message);
	Thread.Sleep(1500);
}


// We are doing a file conversion and report back to the UI
private void ConvertFile(object a)
{
	// Constrain the number of worker threads
	// (Omitted here.)

	// We receive the threadInfo as an uncasted object.
	// Use the 'as' operator to cast it to ThreadInfo.
	var threadInfo = a as ThreadInfo;
	if (threadInfo == null) return;

	var node = threadInfo.SourceObject as TreeListNode;
	if (node == null) return;

	var sourceFile = GetFullPathOfNode(node);
	var targetFile = sourceFile.Replace(node.GetDisplayText("Extension"), ".mp3");
	using (var reader = new AudioFileReader(sourceFile))
	{
		using (var writer = new LameMP3FileWriter(targetFile, reader.WaveFormat, 32))
		{
			UpdateFileInfo(node, "converting...");
			reader.CopyTo(writer);
			UpdateFileInfo(node, "done!");
		}
	}
}

// Update something on a winform in example
// In this example we have the treelistnode we use in the thread
// We want to update some values in the node in the main treelist view
public delegate void NodeProgressDelegate(TreeListNode node, string message);

// Update the node in the treelist view
private void UpdateFileInfo(TreeListNode node, string progressString)
{

	if (node == null) return;
	
	// Check for invoke on the treelist view [main control our node is a member of]
	if (treeList1.InvokeRequired)
	{
		var callback = new NodeProgressDelegate(UpdateFileInfo);
		Invoke(callback, node, progressString);
	}
	else
	{
		// Update the value on the node in UI Thread
		node.SetValue("Progress", progressString);
	}
}

// Define other methods and classes here
// Special class that is an argument to the ThreadPool method.
class ThreadInfo
{
	public string Message { get; set; }
	public string Title { get; set; }
}