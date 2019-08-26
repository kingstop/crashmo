using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack.
/// </summary>

public class Attack : UnitBehavior
{
	public float AttackTime;

	float mTime ;

	public override void Enter ()
	{
		base.Enter ();
		mTime = 0;
		mUnit.Attack ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		mTime += deltaTime;
		if (mTime >= AttackTime)
			this.RunningStatus = RunningStatus.Success;		
	}

}
