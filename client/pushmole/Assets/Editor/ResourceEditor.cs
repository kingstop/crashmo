using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

//		遍历所有文件，生成资源信息文件 resource.json 

public class ResourceEditor : MonoBehaviour
{

	public static void VisitAllResources (string parentDir)
	{
		Debug.Log ("Path.AltDirectorySeparatorChar:"+Path.AltDirectorySeparatorChar);
		Debug.Log ("Path.DirectorySeparatorChar:"+Path.DirectorySeparatorChar);
		Debug.Log ("Path.VolumeSeparatorChar:"+Path.VolumeSeparatorChar);



		int ID = 1; 
		List<ResourceInfo> resourceInfos = new List<ResourceInfo> ();
		string[] filePaths = Directory.GetFiles (parentDir, "*", SearchOption.AllDirectories);

		foreach (string filePath in filePaths)
		{
			if (filePath.ToLower ().EndsWith (".meta"))
				continue;

			for (int i = 0; i < (int)EResourceType.Max; i++)
			{
				if (filePath.ToLower ().EndsWith (((EResourceType)i).ToString ().ToLower ()))
				{
					ResourceInfo info = new ResourceInfo ();
					info.ResourceType = (EResourceType)i;
					info.ID = ID++;
					info.Name = Path.GetFileNameWithoutExtension (filePath);
					info.Path = PathUtility.UniformPath( filePath);
					resourceInfos.Add (info);

					DebugFormat.LogWarning (info.ID, info.Name, info.Path, info.ResourceType);
				}
			}
		}

		ResourceList config = new ResourceList (){ List = resourceInfos };
		SimpleFile.Write (string.Format ("{0}/{1}.json",PathUtility.JsonPath, "resource"), JsonUtility.ToJson (config, true));
	}

}
