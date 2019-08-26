using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System ;

//	将行为对象化
//	该句引用自：《https://zhuanlan.zhihu.com/p/25655986》

public class ABLoader : MonoBehaviour
{
	AssetBundle mAB ;

	public void Load<T>(string path,Action<T> OnLoadFinish) where T : UnityEngine.Object
	{
		this.StartCoroutine (this.LoadCortoutine(path,OnLoadFinish));
	}

	public IEnumerator LoadCortoutine<T>(string path , Action<T> OnLoadFinish) where T : UnityEngine.Object
	{
		WWW www = new WWW (path);
		yield return www;

		if (www.error != null) {
			Debug.LogErrorFormat ("get www asset fail : {0}", path);
			yield break;
		}

		this.mAB = www.assetBundle;
		if (this.mAB == null) {
			Debug.LogErrorFormat ("get asset bundle fail : {0}", path);
			yield break;
		}

		if (OnLoadFinish != null) {
			OnLoadFinish (this.mAB.LoadAsset<T>(path));
		}

		yield return null;
	}

	public void Unload(bool unloadAllLoadedObjects)
	{
		mAB.Unload (unloadAllLoadedObjects);
	}


}
