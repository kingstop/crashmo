using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A star pathFinding .
/// </summary>

public class AStar
{
	//		开启列表：待检测列表，等待测试的格子
	List<Tile> mOpen;			

	//		关闭列表，已经检测过的格子
	List<Tile> mClose;			

	List<Tile> mTilesWillBeReset;

	//	bool：success or fail；List<Tile>: target path , null if fail .
	public Action<bool,List<Tile>,Map> OnFindPathFinish;

	public AStar ()
	{
		mOpen = new List<Tile> ();
		mClose = new List<Tile> ();
		mTilesWillBeReset = new List<Tile> ();
	}

	public void FindPath (Map map, int startX, int startY, int endX, int endY, int size)
	{
		Tile start = map.GetTile (startX, startY);
		Tile end = map.GetTile (endX, endY);
		FindPath (map, start, end, size);
	}

	public void FindPath (Map map, Tile start, Tile target, int size)
	{
		int attempts = 0;

		this.Reset ();

		if (!PreCheck (map, start, target, size))
		{
			SendEvent (false, null,map);
		}
		else if (start == target)
		{
			SendEvent (true, null,map);
		}
		else
		{
			//		start search path .  
			start.ResetCache() ;

			//		step 1 .
			this.mOpen.Add (start);

			while (mOpen.Count > 0)
			{
				//		step 2 . TODO:优化
				Tile current = mOpen [0];
				mOpen.RemoveAt (0);
				mClose.Add (current);

				List<Tile> neighbors = map.GetNeighborsCanReach (current, size, false);

				//		step 3 . 
				for (int i = 0; i < neighbors.Count; i++)
				{
					Tile neighbor = neighbors [i];

					if (mClose.Contains (neighbor))
						continue;
					
					if (mOpen.Contains (neighbor))
						NeighborExistInOpen (current, neighbor);
					else
						NeighborNotExistInOpen (current, neighbor, target);
				}

				//		step 4. 
				if (mOpen.Contains (target))
				{
					//Debug.Log ("Success ！");
					this.SendEvent (true, GeneratePath (start, target),map);
					break;
				}

				//		step 5 . 
				attempts++;
				if (attempts >= GameInfo.PathFindingLimit)
				{
					Debug.LogError ("FindPath Fail！");
					this.SendEvent (false, null,map);
					break;
				}
			}
		}
	}

	//	neighbor 已经存在于开启列表中，表明从起点到neighbor已经找到一条路径；从current又找到一条路径到neighbor，所以此处需要比较2条路径，那条路径距离起点更近；
	void NeighborExistInOpen (Tile current, Tile neighbor)
	{
		int newGCost = AStarFormula.GCost (current, neighbor);

		//		如果新的路径比较近，则覆盖老的路径；neighbor.Parent 指向current
		if (newGCost < neighbor.mGcost)
		{
			neighbor.mParent = current;
			neighbor.mGcost = newGCost;
			neighbor.mFcost = neighbor.mGcost + neighbor.mHcost;

			AddToOpen (neighbor, mOpen);
		}
	}

	void NeighborNotExistInOpen (Tile current, Tile neighbor, Tile target)
	{
		neighbor.mParent = current;
		neighbor.mGcost = AStarFormula.GCost (current, neighbor);
		neighbor.mHcost = AStarFormula.HCost (neighbor, target);
		neighbor.mFcost = neighbor.mGcost + neighbor.mHcost;

		AddToOpen (neighbor, mOpen);
	}

	void AddToOpen (Tile tile, List<Tile> open)
	{
		int insertIndex = 0;

		for (int i = open.Count - 1; i >= 0; i--)
		{
			if (tile.mFcost > open [i].mFcost)
			{
				insertIndex = i + 1;
				break;
			}
		}

		open.Insert (insertIndex, tile);
	}

	List<Tile> GeneratePath (Tile start, Tile target)
	{
		List<Tile> temp = new List<Tile> ();

		Tile current = target;

		while (current != null)
		{
			//		TODO: 排除了角色当前所在的那个地块，避免造成角色的起步异常。
			if (current != start)
				temp.Add (current);
			current = current.mParent;
		}

		List<Tile> path = new List<Tile> (temp.Count);
		for (int i = temp.Count - 1; i >= 0; i--)
		{
			path.Add (temp [i]);
		}

		return path;
	}

	bool PreCheck (Map map, Tile start, Tile end, int size)
	{
		bool condition = false;

		if (map == null || start == null || end == null)
		{
			Debug.LogError ("Error！");
		}
		else if (!start.mIsWalkable || !end.mIsWalkable)
		{
			Debug.LogError ("Error!");
		}
		else
		{
			condition = true;
		}

		return condition;
	}


	void SendEvent (bool success, List<Tile> path,Map map)
	{
		if (this.OnFindPathFinish != null)
			this.OnFindPathFinish (success, path,map);
	}


	void Reset ()
	{
		for (int i = mTilesWillBeReset.Count - 1; i >= 0; i--)
		{
			mTilesWillBeReset [i].ResetCache ();
		}

		this.mOpen.Clear ();
		this.mClose.Clear ();
		mTilesWillBeReset.Clear ();
	}

}
