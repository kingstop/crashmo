using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


/// <summary>
/// Resource manager.
/// </summary>


//		http://blog.csdn.net/qq_19399235/article/details/51702964
//		http://blog.csdn.net/u013108312/article/details/60583898
//		http://blog.csdn.net/ring0hx/article/details/46376709



/*		同步和异步的操作方式：

加载AssetBundle，我们直接使用WWW类而不用WWW.LoadFromCacheOrDownload, 因为我们的资源在游戏开始的时候已经下载到外部存储了，
不要再Download也不要再Cache。注意WWW类加载是异步的，在游戏中我们需要同步加载资源的地方就要注意把资源预加载好存在ResourceManager中，
不然等用的时候加载肯定要写异步代码了。大部分时候我们应该在一个场景初始化时就预加载好所有资源，用的时候直接从ResourceManager的缓存取就可以了。
*/



public class ResourceManager : Singleton<ResourceManager>
{
	

	public override void Init ()
	{
		base.Init ();

		Debug.LogWarning ("针对方案AssetBundle，游戏中有时候需要直接使用/实例化AssetBundle中的相应资源，类似于Resources.Load<T>(); 该如何实现？");
		Debug.LogWarning("预加载相应的资源包AssetBundle，这样在需要使用的地方，就可以从缓存中直接取出。");
		Debug.LogWarning ("比如王者荣耀中的玩家在选好角色，进入游戏加载界面时，资源管理器就可以将相应的角色AssetBundle预先加载，并缓存，在游戏开始时直接实例化。");

		DebugFormat.Log ("PersistentPath:", Application.persistentDataPath);
		DebugFormat.Log ("StreamingPath:", Application.streamingAssetsPath);

		this.InitConfig ();
	}

	public override void Release ()
	{
		base.Release ();
	}

	public void InitConfig ()
	{
		//			预加载，所有数据文件
		Debug.LogError ("预加载，所有数据文件，在初始化进入场景之后，首先加载完成！这里就可以从缓存中直接取出。");
	}


	public T Load<T> (string fullPath) where T : UnityEngine.Object
	{
		T t = Resources.Load<T> (fullPath);
		DebugFormat.Assert (t != null, "Resources.Load Fail：", fullPath);
		return t;
	}


	public T Load<T> (string assetName, EResourceType type) where T:UnityEngine.Object
	{
		ResourceInfo info = AloneDataManager<ResourceList>.Instance.Data.GetResourceInfo (assetName, type);

		Debug.Assert (info != null, "Get asset error：" + assetName + " " + type.ToString ());

		T entity = Resources.Load<T> (ToRelativeResourcePath (info.Path));

		Debug.AssertFormat (entity != null, "Resources.Load Error : {0},{1}", assetName, type);

		return entity;
	}


	public T AsyncLoad<T> (string name, EResourceType type) where T :UnityEngine.Object
	{
		WWW www = new WWW (AloneDataManager<ResourceList>.Instance.Data.GetResourceInfo (name).Path);
		T entity = www.assetBundle.LoadAsset<T> (name);

		return entity;
	}


	string ToRelativeResourcePath (string fullPath)
	{
		string relativePath = fullPath;
		int endIndex = fullPath.IndexOf (Path.GetExtension (fullPath));
		int startIndex = 0;

		if (fullPath.Contains (PathUtility.ResourcePath))
		{
			startIndex = PathUtility.ResourcePath.Length + 1;
		}

		DebugFormat.Assert (startIndex != -1 && endIndex != -1, "startIndex:", startIndex, "endIndex:", endIndex);

		relativePath = fullPath.Substring (startIndex, endIndex - startIndex);
		DebugFormat.Log ("relativePath:", relativePath, "startIndex:", startIndex, "endIndex", endIndex);

		return relativePath;
	}


	//		加载方式一：
	//		加载方式二：
	//		加载方式三：
	public  T LoadResource<T> () where T : UnityEngine.Object
	{
		return null;
	}

}
