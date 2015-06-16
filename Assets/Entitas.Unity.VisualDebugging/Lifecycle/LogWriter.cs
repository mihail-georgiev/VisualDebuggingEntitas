using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

public class LogWriter
{
	private static LogWriter instance;
	private static Queue<Log> logQueue;
	private static string logDir = "Assets/Logs/";
	private static string logFile;
	private static int queueSize = int.Parse("10");

	private LogWriter() {
		UnityEngine.Debug.Log("LogWriter created");
		int count = Directory.GetFiles(logDir,"*.txt").Length;
		logFile = "TestLog(" + count + ").txt";
	}
	
	public static LogWriter Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LogWriter();
				logQueue = new Queue<Log>();
			}
			return instance;
		}
	}
	
	public void WriteToLog(string message)
	{
		Log logEntry = new Log(message);
		logQueue.Enqueue(logEntry);
				
		if (logQueue.Count >= queueSize)
		{
			Thread t = new Thread(FlushLog);
			t.Start();
		}
	}

		
	private void FlushLog()
	{
		Log entry;
		string logPath = "";
		List<string> entries = new List<string>();
		lock(logQueue)
		{	
			while (logQueue.Count > 0)
			{
				entry = logQueue.Dequeue();
				logPath = logDir + entry.LogDate + "_" + logFile;
				entries.Add(entry.Message);
			}
		}

		foreach(string message in entries)
		{
			using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
			{
				using (StreamWriter log = new StreamWriter(fs))
				{
					log.WriteLine(message);
				}
			}
		}
	}

	public void CloseLog()
	{
		FlushLog();
	}
}
	
public class Log
{
	public string Message { get; set; }
	public string LogDate { get; set; }

	public Log(string message)
	{
		Message = message;
		LogDate = DateTime.Now.ToString("yyyy-MM-dd");
	}
}