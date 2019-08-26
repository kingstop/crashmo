using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据处理
/// </summary>

public class DataProcess : SequenceNode
{
	SequenceNode mProcessQueue;
	enum DataState
	{
		Inactive,
		Processing,
		Success,
		Failure
	}
	DataState mState =  DataState.Inactive ;

	public DataProcess ()
	{
		mProcessQueue = new SequenceNode ();
		mProcessQueue.AddNode (new DataProcessNode<GameInfo> ("GameInfo", AloneDataManager<GameInfo>.Instance.Data));
		//mProcessQueue.AddNode (new DataProcessNode<GameInfo>("GameInfo",AloneDataManager<GameInfo>.Instance.Data));

		this.AddNode (mProcessQueue);
	}

	public override void Enter ()
	{
		base.Enter ();
		mState = DataState.Processing;
	}

	/// <summary>
	/// 更新：抽象出更高层级的节点。
	/// </summary>
	/// <param name="deltaTime"></param>
	public override void Update (float deltaTime)
	{
		if (this.mState != DataState.Processing)
			return;

		base.Update (deltaTime);

		switch (this.RunningStatus)
		{
		case RunningStatus.Success:
			this.mState = DataState.Success ;
			break;
		case RunningStatus.Failure:
			this.mState = DataState.Failure;
			this.RunningStatus = RunningStatus.Running;
			UIMessageBox uiMessageBox = new UIMessageBox ("Reminder", "初始化失败，是否重试？", ReProcessData, ExitGame);
			uiMessageBox.Init ();
			uiMessageBox.Enter ();
			break;
		default:
			break;
		}
	}

	private void ReProcessData (UIMessageBox ui)
	{
		ui.Release ();
		this.mState = DataState.Processing;
		mProcessQueue.Enter ();
	}

	private void ExitGame (UIMessageBox ui)
	{
		ui.Release ();
		Game.Instance.mEventCenter.OnEvent (new ExitGameEvent ());
	}
}
