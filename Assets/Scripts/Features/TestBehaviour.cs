using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System;

public class AppUtils
{
//	public static startAppTime singleton = new startAppTime();
	public static AppUtils singleton = new AppUtils();
	public static DateTime startAppTime = DateTime.UtcNow;
	public static LogWriter writer = LogWriter.Instance;
}

public class TestBehaviour : MonoBehaviour {
	Pool pool;

	void Start () {
		pool = new Pool(CoreComponentIds.TotalComponents);
		Entity e = pool.CreateEntity();
		e.AddFirst(0,1);
		e.isSecond = true;
		e.ReplaceFirst(2f,3);
		e.isSecond = false;
		e.RemoveFirst();

		e = pool.CreateEntity();
		e.AddFirst(3f,3);
		e.ReplaceFirst(4f,5);
		e.RemoveFirst();
		e.ReplaceFirst(0,0);
		e.AddThird(new Dictionary<string, int>());
		Dictionary<string, int> newDict = new Dictionary<string, int>();
		newDict.Add("aaaa",4);
		e.ReplaceThird(newDict);
		e.RemoveThird();
		pool.DestroyAllEntities();
	}
	
	void Update () {

	}

	void OnApplicationQuit()
	{
		AppUtils.writer.CloseLog();
	}
}
