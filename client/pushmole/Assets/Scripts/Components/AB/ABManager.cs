using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


//		《AssetBundle打包流程》http://blog.csdn.net/ethuangfen/article/details/53762182
//		《将行为对象化》https://zhuanlan.zhihu.com/p/25655986


public class ABManager : MonoSingleton<ABManager>
{

	public override void Init ()
	{
		base.Init ();
		//StartCoroutine (this.CoroutineLoad ("Flight"));
	}

	public void Load<T> (string assetName, Action<T> OnLoadFinish) where T : UnityEngine.Object
	{
		
	}

	public IEnumerator LoadCoroutine<T> (string assetName, Action<T> onFinish) where T : UnityEngine.Object
	{
		Debug.LogWarning ("TODO:Replace assetName with assetID！");

		WWW wwwManifest = new WWW (PathUtility.GetWWWPath (PathUtility.AssetBundleSubDirectory));
		yield return wwwManifest;

		if (wwwManifest.error != null || wwwManifest.assetBundle == null) {
			Debug.LogErrorFormat ("{0}{1} {2}", "Get www Manifest fail：", PathUtility.AssetBundleSubDirectory, assetName);
			yield break;
		}

		AssetBundle abManifest = wwwManifest.assetBundle;
		AssetBundleManifest manifest = abManifest.LoadAsset<AssetBundleManifest> (PathUtility.AssetBundleManifestName); 
		abManifest.Unload (false);

		//		加载依赖资源
		string[] dependencies = manifest.GetAllDependencies (assetName);
		AssetBundle[] assetBundlesDependence = new AssetBundle[dependencies.Length];

		for (int i = 0; i < dependencies.Length; i++) {
			WWW wwwDepend = new WWW (PathUtility.GetWWWPath (dependencies [i]));
			yield return wwwDepend;

			Debug.AssertFormat (wwwDepend.error == null, "Get www fail：{0}-{1}", wwwDepend.url, wwwDepend.error);
			assetBundlesDependence [i] = wwwDepend.assetBundle;
		}

		//		加载目标资源
		WWW www = new WWW (PathUtility.GetWWWPath (assetName));
		yield return www;

		if (www.error != null) {
			Debug.LogErrorFormat ("{0}{1}", "get asset fail : ", assetName);
			yield break;
		}
		AssetBundle assetBundle = www.assetBundle;

		if (assetBundle != null) {
			if (onFinish != null) {
				onFinish (assetBundle.LoadAsset<T> (assetName));
			}
			//GameObject prefab = assetBundle.LoadAsset<GameObject> (assetName);
			//GameObject ins = Instantiate (prefab);
			//ins.name = assetName;

			assetBundle.Unload (false);
		}
	}


	public IEnumerator CoroutineLoad (string assetName)
	{
		//加载assetBundleManifest文件

		//		加载全局资源包信息
		WWW wwwManifest = new WWW (PathUtility.GetWWWPath (PathUtility.AssetBundleSubDirectory));

		DebugFormat.Assert (wwwManifest != null, "Get www fail：", assetName);

		yield return wwwManifest;

		DebugFormat.Assert (wwwManifest.error == null, wwwManifest.error);
		DebugFormat.Assert (wwwManifest.assetBundle != null, "www is null !", assetName);

		if (string.IsNullOrEmpty (wwwManifest.error)) {
			AssetBundle abManifest = wwwManifest.assetBundle;
			AssetBundleManifest manifest = abManifest.LoadAsset<AssetBundleManifest> (PathUtility.AssetBundleManifestName); 
			abManifest.Unload (false);

			//		加载依赖资源
			string[] dependencies = manifest.GetAllDependencies (assetName);
			AssetBundle[] assetBundlesDependence = new AssetBundle[dependencies.Length];

			for (int i = 0; i < dependencies.Length; i++) {
				WWW w = new WWW (PathUtility.GetWWWPath (dependencies [i]));
				yield return w;

				Debug.AssertFormat (w.error == null, "Get www fail：{0}-{1}", w.url, w.error);

				assetBundlesDependence [i] = w.assetBundle;
			}

			//		加载当前资源
			string assetBundlePath = PathUtility.GetWWWPath (assetName);
			WWW www = new WWW (assetBundlePath);
			yield return www;

			DebugFormat.Assert (www.error == null, "get asset fail : ", assetBundlePath);

			if (www.error == null) {
				AssetBundle assetBundle = www.assetBundle;

				if (assetBundle != null) {
					GameObject prefab = assetBundle.LoadAsset<GameObject> (assetName);
					GameObject ins = Instantiate (prefab);
					ins.name = assetName;

					assetBundle.Unload (false);
				}
			}
		}
	}



	public static void Load (string assetName)
	{
		//加载assetBundleManifest文件  
		AssetBundle assetBundleManifest = AssetBundle.LoadFromFile (Path.Combine (PathUtility.AssetBundlePath, PathUtility.AssetBundleSubDirectory));

		if (assetBundleManifest != null) {
			AssetBundleManifest manifest = (AssetBundleManifest)assetBundleManifest.LoadAsset (PathUtility.AssetBundleManifestName);

			//先加载ui1的依赖文件
			string[] dependencies = manifest.GetAllDependencies (assetName);
			AssetBundle[] dependsAssetbundle = new AssetBundle[dependencies.Length];
			for (int index = 0; index < dependencies.Length; index++) {
				dependsAssetbundle [index] = AssetBundle.LoadFromFile (PathUtility.GetAssetBundlePath (dependencies [index]));
			}

			//后加载ui1
			AssetBundle bundle = AssetBundle.LoadFromFile (PathUtility.GetAssetBundlePath (assetName));

			GameObject prefab = bundle.LoadAsset<GameObject> (assetName);
			if (prefab != null) {
				Instantiate (prefab).name = assetName;
			}

		}
	}


}
