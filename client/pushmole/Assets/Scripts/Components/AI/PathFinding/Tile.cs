using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile.
/// </summary>

public class Tile
{
	public int mRow;
	public int mColumn;

	public Vector3 mPosition;
	public bool mIsWalkable;

	public Tile mParent;

	public int mGcost ;		//	从起点到该点的消耗
	public int mHcost ;		//	从该点到终点的估值消耗
	public int mFcost ;		//	mGcost+mHcost . 

	public void ResetCache()
	{
		mGcost = 0;
		mHcost = 0;
		mFcost = 0;
		mParent = null;
	}
}
