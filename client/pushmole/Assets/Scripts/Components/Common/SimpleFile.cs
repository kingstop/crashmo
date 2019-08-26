using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SimpleFile : MonoBehaviour
{
	DirectoryInfo mDirectoryInfo;

	public static bool CheckDirectory (string filePath)
	{
		string dir = Path.GetDirectoryName (filePath);

		if (!Directory.Exists (dir))
		{
			Directory.CreateDirectory (dir);
		}

		return true;
	}

	public static void Write (string filePath, string content)
	{
		string parentDir = Path.GetDirectoryName (filePath);

		DebugFormat.LogWarning (parentDir, System.Environment.NewLine, filePath);

		if (!Directory.Exists (parentDir))
		{
			Directory.CreateDirectory (parentDir);
		}

		if (File.Exists (filePath))
		{
			File.Delete (filePath);
		}

		File.WriteAllText (filePath, content);
	}






}
