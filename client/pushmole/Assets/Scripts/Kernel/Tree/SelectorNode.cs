using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
	int mRunningNodeIndex = -1;

	public override void Enter ()
	{
		base.Enter ();
		this.Reset ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		if (this.Children == null || this.Children.Count == 0)
		{
			this.RunningStatus = RunningStatus.Failure;
			return;
		}

		if (RunningNode!=null&&RunningNode.RunningStatus == RunningStatus.Success)
		{
			this.Reset ();
			this.RunningStatus = RunningStatus.Success;
			return;
		}

		if (this.mRunningNodeIndex == -1)
		{
			this.mRunningNodeIndex++;
			this.RunningNode = Children [this.mRunningNodeIndex];
			this.RunningNode.Enter ();
		}

		this.RunningNode.Update (deltaTime);

		switch (this.RunningNode.RunningStatus)
		{
		case RunningStatus.Success:
			this.RunningNode.Leave ();
			this.Reset ();
			this.RunningStatus = RunningStatus.Success;
			break;

		case RunningStatus.Failure:
			RunningNode.Leave ();

			this.mRunningNodeIndex++;
			if (this.mRunningNodeIndex >= this.Children.Count)
			{
				this.Reset ();
				this.RunningStatus = RunningStatus.Failure;
			}
			else
			{
				RunningNode = this.Children [mRunningNodeIndex];
				RunningNode.Enter ();
			}

			break;
		}
	}

	public override void Leave ()
	{
		base.Leave ();

		if (RunningNode != null)
		{
			RunningNode.Leave ();
		}
		this.Reset ();	
	}

	void Reset ()
	{
		this.mRunningNodeIndex = -1;
		this.RunningNode = null;
	}

}
