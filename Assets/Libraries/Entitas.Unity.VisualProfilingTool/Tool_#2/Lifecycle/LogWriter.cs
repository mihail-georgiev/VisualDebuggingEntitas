using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace Entitas.Unity.VisualProfilingTool {
	public class LogWriter {
		private static LogWriter instance;
		private static Queue<string> logQueue;
		private static string logDir = "Assets/Logs/";
		private static string logPath;
		private static int queueSize = 10;
		private static DateTime startAppTime;

		private LogWriter() {
			int count = Directory.GetFiles(logDir,"*.txt").Length;
			startAppTime = DateTime.UtcNow;
			logPath = logDir + startAppTime.ToString ("yyyy-MM-dd") + "_TestLog(" + count + ").txt";
		}
		
		public static LogWriter Instance {
			get {
				if (instance == null) {
					instance = new LogWriter();
					logQueue = new Queue<string>();
				}
				return instance;
			}
		}
		
		public void WriteToLog(string message) {
			double timePassed = (DateTime.UtcNow - startAppTime).TotalMilliseconds;
			message += " at " + timePassed;
			logQueue.Enqueue(message);
					
			if (logQueue.Count >= queueSize) {
				Thread t = new Thread(FlushLog);
				t.Start();
			}
		}

		private void FlushLog() {
			List<string> entries = new List<string>();
			lock(logQueue) {	
				while (logQueue.Count > 0) {
					entries.Add(logQueue.Dequeue());
				}
			

				foreach(string message in entries) {
					using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write)) {
						using (StreamWriter log = new StreamWriter(fs)) {
							log.WriteLine(message);
						}
					}
				}
			}
		}

		public void CloseLog() {
			FlushLog();
		}
	}
}