using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗。
/// </summary>

public class Battle : LogicTree
{
	Tree mBattleTree;

	CameraControl mCameraControl;
	UnitManager mUnitManager;
	Map mMap;

	UIBattle mUIBattle;
	UIBattleEnd mUIBattleEnd;
	UIJoyStick mUIJoyStick ;

	//		帧周期，两帧之间的间隔；取值为deltatime .
	public float mFrameInterval = 0;

	public Battle()
	{
		//	behavior phase .  Current is null . 

		mBattleTree = new Tree ();

		//	logic tree . 

		AddNode (mMap = new Map (GameInfo.MapRow, GameInfo.MapColumn, GameInfo.MapTileLength, Vector3.zero));
		mMap.SetTileWalkable (GameObject.FindObjectsOfType<TileComponent>());
		mMap.InitMapResource (GameInfo.MapRow, GameInfo.MapColumn, GameInfo.MapTileLength, Vector3.zero);

		AddNode (mUnitManager = new UnitManager (mMap));

		AddNode (mUIBattle = new UIBattle ());
		AddNode (mCameraControl = new CameraControl ());
		mCameraControl.SetCamera (GameObject.FindObjectOfType<Camera> (), GameInfo.CameraOffset);

		AddNode (mUIJoyStick = new UIJoyStick() );
		AddNode (new BattleCountDown ());
	}

	public override void Init ()
	{
		base.Init ();
		AloneEventCenter<ExitBattleEvent>.Instance.AddListener (this,this.DoExitBattle);
	}

	public override void Release ()
	{
		base.Release ();
		Game.Instance.mDataManager.Reset ();
		AloneEventCenter<ExitBattleEvent>.Instance.RemoveListener (this);
	}

	public override void Update (float deltaTime)
	{
		if (!this.IsGameOver ())
		{
			this.mFrameInterval = deltaTime;

			base.Update (deltaTime);

			mBattleTree.Update (deltaTime);
		}
	}

	//  TODO:可以放到行为子节点判断游戏结束标志 .
	public bool IsGameOver ()
	{
		GameInfo info = AloneDataManager<GameInfo>.Instance.Data;

		if (info.mBattleStatus != EBattleStatus.Playing)
			return true;
		
		if (info.mCurrentTimeLeft < 0 || mUnitManager.IsPlayerDead ())
			info.mBattleStatus = EBattleStatus.Lose;
		else if (mUnitManager.IsAllMonstersDead ())
			info.mBattleStatus = EBattleStatus.Win;
		else
			info.mBattleStatus = EBattleStatus.Playing;

		AloneDataManager<GameInfo>.Instance.Data = info;

		if (info.mBattleStatus == EBattleStatus.Lose || info.mBattleStatus == EBattleStatus.Win)
		{
			this.BattleEnd ();
		}

		return info.mBattleStatus != EBattleStatus.Playing;
	}

	//	TODO:可以独立出去作为一个行为节点。
	void BattleEnd ()
	{
		this.AddNode (this.mUIBattleEnd = new UIBattleEnd ());

		this.mUIBattle.Leave ();
		this.mUIJoyStick.Leave ();

		this.mUIBattleEnd.Init ();
		this.mUIBattleEnd.Enter ();
	}

	public override void Leave ()
	{
		base.Leave ();

		mBattleTree.Leave ();
	}

	void DoExitBattle(ExitBattleEvent Event)
	{
		this.RunningStatus = RunningStatus.Success;
	}
}
