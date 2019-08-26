using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
using UnityEditor;



//		两种打包选择：一、只打包被设置AssetLabels>AssetBundle名称的资源；
//		二、对于所有资源进行一键打包，
//		打成的AssetBundle包名字为路径的拼接：比如AssetBundle/Prefabs/Monster1.prefab => assetbundle_prefabs_monster1.asset
//		http://blog.csdn.net/ithot/article/details/75128535


public class AssetBundleEditor : MonoBehaviour
{
	public static void BuildWithName ()
	{
		Build ();
	}

	public static void BuildAll ()
	{
		ClearAssetBundleName ();
		SetAssetBundleName (Application.dataPath);
		Build ();
	}

	public static void Build ()
	{
		if (Directory.Exists (PathUtility.AssetBundlePath))
		{
			Directory.Delete (PathUtility.AssetBundlePath, true);
		}

		Directory.CreateDirectory (PathUtility.AssetBundlePath);
		
		BuildPipeline.BuildAssetBundles (PathUtility.AssetBundlePath, BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
	}

	//		删除所有的AssetBundle包的名字
	public static void ClearAssetBundleName ()
	{
		string[] names = AssetDatabase.GetAllAssetBundleNames ();

		foreach (string name in names)
		{
			AssetDatabase.RemoveAssetBundleName (name, true);
		}
	}

	public static void SetAssetBundleName (string parentDirectory)
	{
		string[] filePaths = Directory.GetFiles (parentDirectory, "*", SearchOption.AllDirectories);

		foreach (string filePath in filePaths)
		{
			if (filePath.ToLower ().EndsWith (".meta"))
				continue;

			for (int i = 0; i < (int)EResourceType.Max; i++)
			{
				if (filePath.ToLower ().EndsWith ("." + ((EResourceType)(i)).ToString ().ToLower ()))
				{
					NameAssetBundle (filePath);
					break;
				}
			}
		}
	}

	//		命名
	static void NameAssetBundle (string assetFullPath)
	{
		//		这个路径要求必须以"Asset"开始的路径
		string importerPath = assetFullPath.Replace (Application.dataPath, "Assets");

		Debug.Log ("Imporer Path：" + importerPath);

		AssetImporter importer = AssetImporter.GetAtPath (importerPath) as AssetImporter;

		DebugFormat.Assert (importer != null, "Name Assetbundle Error：find nothing with path : ", importerPath);

		string assetName = Path.GetFileNameWithoutExtension (assetFullPath);

		importer.assetBundleName = assetName;
	}

}
