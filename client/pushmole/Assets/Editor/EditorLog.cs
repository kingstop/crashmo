using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;



public class EditorLog
{
	public static LogCallback mLogCallback;
	public static string mLogStr;

	public static void Init ()
	{
		if (mLogCallback != null)
			return;
		LogCallback.Max = int.MaxValue;
		LogCallback.OnLog -= OnLog;
		LogCallback.OnLog += OnLog;

		mLogCallback = new LogCallback ();
		mLogCallback.Init ();
	}

	static void OnLog (string message)
	{
		mLogStr = message;
	}

	public static void WritToFile (string name)
	{
		string parentDir = string.Format ("{0}/Debug/_log/{1}", Application.dataPath, name);
		string filePath = string.Format ("{0}/{1}{2}.txt", parentDir, name, System.DateTime.Now.ToString ("yyyyMMdd_hhmmss"));

		DebugFormat.Log (parentDir, System.Environment.NewLine, filePath);

		SimpleFile.Write (filePath,mLogStr);

		mLogStr = null;
		AssetDatabase.Refresh ();
	}
}
