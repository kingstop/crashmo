using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battle count down.倒计时
/// </summary>

public class BattleCountDown : LogicNode
{
	float mTimeLeft = 0 ;

	public override void Init ()
	{
		base.Init ();
		mTimeLeft = AloneDataManager<GameInfo>.Instance.Data.mCurrentTimeLeft;
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		GameInfo info = AloneDataManager<GameInfo>.Instance.Data;

		mTimeLeft -= deltaTime;
		info.mCurrentTimeLeft = mTimeLeft;

		AloneDataManager<GameInfo>.Instance.Data = info;
	}


}
