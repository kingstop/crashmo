using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resource loader . 
/// </summary>

public class ResourceLoader
{
	
	//	TODO:统一的资源管理，名字-Prefab路径映射；id-路径映射
	//	资源对象池
	public static GameObject Create (string name)
	{
		GameObject prefab = ResourceManager.Instance.Load<GameObject> (name, EResourceType.Prefab);

		DebugFormat.Assert (prefab != null, "Load prefab fail : ", name);

		GameObject instance = null;

		if (prefab == null)
		{
			instance = GameObject.CreatePrimitive (PrimitiveType.Cube);
		}
		else
		{
			instance = GameObject.Instantiate<GameObject> (prefab);
		}

		return instance;
	}

}
