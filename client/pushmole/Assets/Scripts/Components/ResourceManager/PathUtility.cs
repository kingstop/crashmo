using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;



public class PathUtility
{

	public static string ExcelPath = Application.dataPath + "/Design/Excel";
	public static string JsonPath = Application.dataPath + "/Design/Json";
	public static string ResourcePath = Application.dataPath + "/Resources";

	public static string AssetBundleManifestName = "AssetBundleManifest";
	public static string AssetBundleSubDirectory = Application.platform.ToString ();
	public static string AssetBundlePath = Path.Combine (Application.streamingAssetsPath, AssetBundleSubDirectory);



	public static string GetAssetBundlePath (string assetName)
	{
		return Path.Combine (AssetBundlePath, assetName);
	}


	public static string GetWWWPath (string assetName)
	{
		return GetWWWUrl(assetName) ;
	}


	public static string GetWWWUrl(string assetName)
	{
		string assetBundlePath = Path.Combine(AssetBundlePath,assetName) ;
		return "file://" + assetBundlePath;
	}


	public static string UniformPath(string path)
	{
		return path.Replace (Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
	}


}
