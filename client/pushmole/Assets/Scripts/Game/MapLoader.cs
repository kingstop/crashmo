using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图加载。
/// </summary>

public class MapLoader 
{
	Transform mRootTrans ;

	public MapLoader()
	{
		GameObject root = new GameObject (this.GetType().Name);
		mRootTrans = root.transform;
		mRootTrans.transform.position = Vector3.zero;
		mRootTrans.transform.localScale = Vector3.one;
	}

	public void Init(int rowCount ,int columnCount,float prefabLength,Vector3 origin)
	{
		#if NO_USE_SCENE_DATA
		GameObject prefabA = Resources.Load<GameObject> ("Art/Prefabs/tile_grass01");
		GameObject prefabB = Resources.Load<GameObject> ("Art/Prefabs/tile_grass02");
		GameObject prefabC = Resources.Load<GameObject> ("Art/Prefabs/tile_grass03");
		int cullOff = 5;

		GameObject prefab = prefabA;

		Vector3 pos = Vector3.zero;

		for(int row=0;row<rowCount;row++)
		{
			for(int column=0;column<columnCount;column++)
			{				
				if ((row <= cullOff||row >= GameInfo.MapRow - cullOff) && (column <= cullOff||column >= GameInfo.MapColumn - cullOff))
					continue;

				pos.x = origin.x + (row + 0.5f) * prefabLength;
				pos.z = origin.z +(column+0.5f)*prefabLength ;

				if (row % 3 == 0 || column % 3 == 0)
				{
					prefab = prefabA;
				}
				else
				{
					if ((row%2==0&&column % 2 != 0)||(row%2!=0&&column%2==0))
						prefab = prefabC;
					else
						prefab = prefabB;
				}

				GameObject tileObj = GameObject.Instantiate<GameObject> (prefab);
				tileObj.transform.SetParent (this.mRootTrans,false);
				tileObj.transform.localScale = Vector3.one;
				tileObj.name = prefab.name;
				tileObj.transform.position = pos;
			}
		}

		Debug.LogError ("See the art design !");
		this.mRootTrans.transform.position = new Vector3 (0,-1,0);

		#endif
	}

}
