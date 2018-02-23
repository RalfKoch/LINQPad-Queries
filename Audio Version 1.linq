<Query Kind="Program">
  <NuGetReference>NAudio</NuGetReference>
  <NuGetReference>NAudio.Lame</NuGetReference>
  <Namespace>NAudio.Wave</Namespace>
</Query>

void Main()
{
	Console.Read();
	StartRecording();

	Console.Read();
	StopRecording();

//	using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(inputStream)))
//	using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
//	{
//		byte[] bytes = new byte[waveStream.Length];
//		waveStream.Position = 0;
//		waveStream.Read(bytes, 0, waveStream.Length);
//		waveFileWriter.WriteData(bytes, 0, bytes.Length);
//		waveFileWriter.Flush();
//	}
}

public WaveInEvent waveSource = null;
public WaveFileWriter waveFile = null;

private void StartRecording()
{
	waveSource = new WaveInEvent();
	waveSource.WaveFormat = new WaveFormat(44100, 1);

	waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
	waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

	var defaultAudioPath = @"C:\Temp\";
	var current = DateTime.UtcNow;
	var fileName = string.Format("{0}{1}{2}.{3}{4}{5}.wav", current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second);
	var waveFileName = Path.Combine(defaultAudioPath, fileName);

	output = new FileStream(waveFileName, FileMode.Append);
	waveSource.StartRecording();
}

private void StopRecording()
{
	waveSource.StopRecording();
	output.Flush();
	output.Close();
}

private Stream output = null;

void waveSource_DataAvailable(object sender, WaveInEventArgs e)
{
	//	if (waveFile != null)
	//	{
	//		waveFile.Write(e.Buffer, 0, e.BytesRecorded);
	//		waveFile.Flush();
	//	}

	// Der Stream [byte array] wird in den output stream geschrieben
	while (e.BytesRecorded > 0)
	{
		output.Write(e.Buffer, 0, e.BytesRecorded);
		output.Flush();
	}
}

void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
{
	if (waveSource != null)
	{
		waveSource.Dispose();
		waveSource = null;
	}

	if (waveFile != null)
	{
		waveFile.Dispose();
		waveFile = null;
	}
}

private void SaveStream()
{
	// Define other methods and classes here
	using (var fileStream = File.Create("C:\\Path\\To\\File"))
	{
//		myOtherObject.InputStream.Seek(0, SeekOrigin.Begin);
//		myOtherObject.InputStream.CopyTo(fileStream);
	}
}