using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceInfo
{

	public int ID;
	public string Name;
	public string Path;

	public string BundleName;
	public string BundlePath;
	public EResourceType ResourceType;

}



[System.Serializable]
public class ResourceList
{
	public List<ResourceInfo> List;
	DDictionary<EResourceType,string,ResourceInfo> mResourceNameDDic;
	DDictionary<EResourceType,int,ResourceInfo> mResourceIdDDic;

	public ResourceList ()
	{
		mResourceNameDDic = new DDictionary<EResourceType, string, ResourceInfo> ();
		mResourceIdDDic = new DDictionary<EResourceType, int, ResourceInfo> ();
	}

	public void Init (string path=null)
	{
		TextAsset asset = Resources.Load<TextAsset> ("Data/resource");
		DebugFormat.Assert (asset != null, "Load resouce config Fail！");

		ResourceList list = JsonUtility.FromJson<ResourceList> (asset.text);

		foreach (ResourceInfo info in list.List)
		{
			mResourceIdDDic [info.ResourceType] [info.ID] = info;
			mResourceNameDDic [info.ResourceType] [info.Name] = info;
		}
	}



	public ResourceInfo GetResourceInfo (string name, EResourceType type = EResourceType.Prefab)
	{
		if (mResourceNameDDic.ContainsKey (type, name))
		{
			return mResourceNameDDic [type] [name];
		}

		DebugFormat.LogError ("Found No Prefab Name:", name, "Type:", type);
		return null;	
	}

	public ResourceInfo GetResourceInfo (int ID, EResourceType type = EResourceType.Prefab)
	{
		if (mResourceIdDDic.ContainsKey (type, ID))
		{
			return mResourceIdDDic [type] [ID];
		}

		DebugFormat.LogError ("Found No Prefab ID:", ID, "Type:", type);
		return  null;
	}


}
