using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 
 Cache All Data , Process them and Send Data Refresh Event  . 
 
 DataModel Frame : 
 Solution 1 : All Data Cached in a DataManager . 
 Solution 2 : Each Data Cached in a AloneDataManager . See AloneDataManager.cs . 

*/

/// <summary>
///     每一个数据项，包含3个：字段，属性，数据变更后的回调
///     包括网络数据，和本地Config数据。
/// </summary>

public class DataManager : LogicTree
{
	bool mInit = false;

	public override void Init ()
	{
		if (mInit)
		{
			return;
		}
		mInit = true;

		base.Init ();

		Debug.LogWarning ("TODO:在适当的位置用Struct代替Class；比如技能数据，在角色死亡时，技能数据信息，会被直接复制下来而不会丢失。");

		AloneDataManager<GameInfo>.Instance.Data = new GameInfo ();
		AloneDataManager<GameInfo>.Instance.Data.mCurrentTimeLeft = GameInfo.GameTimeLimit;
		AloneDataManager<GameInfo>.Instance.Data.mBattleStatus = EBattleStatus.Playing;

		AloneDataManager<ResourceList>.Instance.Data = new ResourceList ();
		AloneDataManager<ResourceList>.Instance.Data.Init ();



	}

	public override void Enter ()
	{
		base.Enter ();
		this.Reset ();
	}

	public void Reset ()
	{
		AloneDataManager<GameInfo>.Instance.Data.mCurrentTimeLeft = GameInfo.GameTimeLimit;
		AloneDataManager<GameInfo>.Instance.Data.mBattleStatus = EBattleStatus.Playing;
	}


	#region	旧的实现方式

	public Action<GameInfo> OnUserInfoChanged;
	private GameInfo mUserInfo;

	public GameInfo UserInfo
	{
		get
		{
			return mUserInfo;
		}
		set
		{
			mUserInfo = value;
			if (OnUserInfoChanged != null)
				OnUserInfoChanged.Invoke (mUserInfo);
		}
	}

	#endregion


}
