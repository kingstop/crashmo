using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile component ： 优化成存储到数据配置中。
/// </summary>

public class TileComponent : MonoBehaviour
{
	public enum ETileWalkable
	{
		Walkable,
		UnWalkable,
	}

	public int mSize ;

	public ETileWalkable mWalkable  ;
}
