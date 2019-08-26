using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 顺序节点。
/// 顺序执行每个子节点，直到遇到一个执行状态是Failure的子节点，停止执行，并将自身执行状态设为Failure.
/// 如果执行完所有的节点都是成功执行的子节点，则自身状态置为Success . 
/// </summary>

public class SequenceNode : Node
{
	int mRunningNodeIndex = 0;

	public override void Enter ()
	{
		base.Enter ();
		this.Reset ();
	}

	public override void Update (float deltaTime)
	{
		//base.Update(deltaTime);

		if (this.Children == null)
		{
			this.RunningStatus = RunningStatus.Failure;
			return;
		}

		if (this.RunningNode != null && RunningNode.RunningStatus == RunningStatus.Failure)
		{
			this.Reset ();
			this.RunningStatus = RunningStatus.Failure;
			return; 
		}

		if (RunningNode == null)
		{
			mRunningNodeIndex = 0;
			RunningNode = this.Children [mRunningNodeIndex];
			RunningNode.Enter ();
		}


		RunningNode.Update (deltaTime);

		switch (RunningNode.RunningStatus)
		{
		case RunningStatus.Inactive:
			Debug.LogError ("Error Running！");
			break;

		case RunningStatus.Success:
			this.RunningNode.Leave ();

			mRunningNodeIndex++;

			if (mRunningNodeIndex <= Children.Count - 1)
			{
				this.RunningNode = Children [mRunningNodeIndex];
				this.RunningNode.Enter ();
			}
			else
			{
				this.Reset ();
				this.RunningStatus = RunningStatus.Success;
			}
			break;

		case RunningStatus.Failure:
			this.RunningNode.Leave ();
			this.Reset ();
			this.RunningStatus = RunningStatus.Failure;
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
		RunningNode = null;

		mRunningNodeIndex = -1;
	}

}
