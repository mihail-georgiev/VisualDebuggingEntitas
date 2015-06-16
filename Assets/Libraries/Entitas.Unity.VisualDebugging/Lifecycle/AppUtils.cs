using System;

public class AppUtils
{
	public static AppUtils singleton = new AppUtils();
	public static DateTime startAppTime = DateTime.UtcNow;
	public static LogWriter writer = LogWriter.Instance;
}