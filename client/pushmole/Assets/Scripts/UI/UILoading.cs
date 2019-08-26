using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : UINode
{

	float mLifeTime = 3;
	float mCurrentTime = 0;

	public override void Enter ()
	{
		base.Enter ();
		this.Reset ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		mCurrentTime += deltaTime;

		if (mCurrentTime >= mLifeTime)
		{
			this.RunningStatus = RunningStatus.Success;
		}	
	}

	public override void Leave ()
	{
		base.Leave ();
		this.Reset ();
	}

	void Reset ()
	{
		this.mCurrentTime = 0;
	}


}
