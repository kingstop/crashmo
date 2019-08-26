using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追逐
/// </summary>

public class Chase : UnitBehavior
{
	//		攻击距离
	float mAttackDistance;
	//		放弃距离
	float mAbandonDistance;

	Vector3 mTargetPos;

	public override void Enter ()
	{
		base.Enter ();	
		mAttackDistance = mUnit.mAttackDistance;
		mAbandonDistance = mUnit.mMaxChaseDistance; 

		this.mTargetPos = Vector3.zero * int.MaxValue;
		this.CheckTargtPosition ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		this.CheckTargtPosition ();

		float distance = Vector3.Distance (mUnit.mTransform.position, mUnit.mTarget.mTransform.position);

		if (distance < mAttackDistance)
		{
			this.RunningStatus = RunningStatus.Success;
			mUnit.CancelMove ();
		}
		else if (distance > mAbandonDistance)
		{
			this.RunningStatus = RunningStatus.Failure;
			mUnit.CancelMove ();
		}

	}

	public override void Leave ()
	{
		base.Leave ();
	}

	void CheckTargtPosition()
	{
		if (this.mTargetPos != mUnit.mTarget.mTransform.position)
		{
			this.mTargetPos = mUnit.mTarget.mTransform.position;
			mUnit.Move (this.mTargetPos);
		}
	}
}
