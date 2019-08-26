using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Unit move.
/// </summary>

public class UnitMove : LogicNode
{
	//		bool : true成功到达目标点；false未达到目标点，但角色运动结束。
	public Action<bool> OnArrive;
	//		position , rotation
	public Action<Vector3,Vector3> OnPosition;

	AStar mAStar;

	EMoveState mMoveState;

	List<Tile> mPath;

	int mCurrentTargetIndex;
	Vector3 mCurrentPosition;

	float mSpeed;

	public enum EMoveState
	{
		UnMoving,
		Moving,
		MoveFinish,
	}

	public UnitMove ()
	{
		mAStar = new AStar ();
		mAStar.OnFindPathFinish += this.OnFindPathFinish;
	}

	public void SetPosition (Vector3 position)
	{
		this.mCurrentPosition = position;
	}

	public void SetSpeed (float speed)
	{
		this.mSpeed = speed;
	}

	public void Move (Map map, Tile start, Tile target, int size)
	{
		this.Reset ();
		mAStar.FindPath (map, start, target, size);
	}

	public void Move (Map map, Vector3 direction, float deltaTime, int size)
	{
		this.Reset ();
		Vector3 position = this.mCurrentPosition + direction.normalized * deltaTime * mSpeed;
		Tile tile = map.GetTile (position, size);

		if (map.IsWalkable (tile, size,position))
		{
			SendPosition (position, direction);
		}
		else
		{
			SendPosition (this.mCurrentPosition, direction);
		}
	}

	public void CancelMove ()
	{
		this.Reset ();
	}

	void OnFindPathFinish (bool success, List<Tile> path, Map map)
	{
		switch (success)
		{
		case true:
			if (path == null || path.Count == 0)
				SendEvent (success);
			else
				StartMove (path, map);
			break;
		default:
			break; 
		}
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		switch (mMoveState)
		{
		case EMoveState.Moving:
			Tile realtimeTargetTile = mPath [mCurrentTargetIndex];

			Vector3 direction = (realtimeTargetTile.mPosition - mCurrentPosition).normalized;
			float distance = Vector3.Distance (mCurrentPosition, realtimeTargetTile.mPosition);
			float movingDis = deltaTime * this.mSpeed;

			if (this.mSpeed == 0)
			{
				Debug.LogError ("Warning:Speed is zero !");
				this.mMoveState = EMoveState.UnMoving;
			}

			if (distance <= movingDis)
			{
				mCurrentTargetIndex++;
				mCurrentPosition = realtimeTargetTile.mPosition;
			}
			else
			{
				mCurrentPosition = mCurrentPosition + movingDis * direction;
			}
			SendPosition (mCurrentPosition, direction);

			if (mCurrentTargetIndex >= mPath.Count)
			{
				SendEvent (true);
				this.Reset ();
				this.mMoveState = EMoveState.MoveFinish;
			}
			break;
		}
	}

	void StartMove (List<Tile> path, Map map)
	{
		mMoveState = EMoveState.Moving;
		this.mPath = path;
	}

	//		TODO:针对Size修正路线
	void CorrectPath (List<Tile> path, List<Vector3> positions, Map map, int size)
	{		
	}

	//		Event 1
	void SendEvent (bool successArrive)
	{
		if (OnArrive != null)
			OnArrive (successArrive);
	}

	//		Event 2
	void SendPosition (Vector3 position, Vector3 direction)
	{
		if (OnPosition != null)
			OnPosition (position, direction);
	}

	void Reset ()
	{
		this.mCurrentTargetIndex = 0;
		if (this.mPath != null)
			this.mPath.Clear ();
		this.mMoveState = EMoveState.UnMoving;
	}

	public override void Release ()
	{
		base.Release ();

		this.Reset() ;

		//		由于保证AStar的独立性，AStar没有继承自LogicNode,；所以这里需要单独释放。
		mAStar = null;
		mPath = null;
	}

}
