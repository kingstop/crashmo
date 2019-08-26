using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO:数据读取节点。
/// 读取本地数据，支持异步读取，每读取一个数据文件后，自动跳转至下一个数据文件。
/// </summary>

public class DataProcessNode<T> : Node where T : new()
{

	//		The on process complete.
	public System.Action<bool> OnProcessComplete;


	//		The name of the m file.
	string mFileName;


	//		if save when exit.
	bool mSaveWhenExit;


	//		The m data source.
	T mDataSource;

	//		处理百分比
	float mProcessPercent;

	public DataProcessNode ()
	{
	}

	public DataProcessNode(string fileName, T target):this()
	{
		this.mFileName = fileName;
		this.mDataSource = target;
		this.mSaveWhenExit = false;
	}

	public override void Enter ()
	{
		base.Enter ();
		Game.Instance.mCoroutineManager.StartCoroutine (this.ReadData ());
	}

	public override void Leave ()
	{
		base.Leave ();

		if (mSaveWhenExit)
		{
			string json = JsonUtility.ToJson(mDataSource);
			Debug.LogWarning (json);
		}
	}

	public virtual IEnumerator ReadData ()
	{
		ResourceRequest req = Resources.LoadAsync<TextAsset> (mFileName);

		if (req == null)
		{
			Debug.LogWarning (string.Format ("Null Resource {0}", mFileName));
			this.SendEvent (false);
			yield break;
		}
		
		while (!req.isDone)
		{
			Debug.LogWarning (req.progress);
			yield return req;
		}

		if (req.asset == null)
		{
			Debug.LogWarning (string.Format( "Null Resource {0}",mFileName));
			SendEvent (false);
		}
		else
		{
			Debug.LogWarning ("TODO : Process data success ! ");
			SendEvent (true);
		}
		yield break;
	}

	void SendEvent (bool success)
	{
		switch (success)
		{
		case true:
			this.RunningStatus = RunningStatus.Success;
			break;
		default:
			this.RunningStatus = RunningStatus.Failure;
			break;
		}

		if (this.OnProcessComplete != null)
		{
			this.OnProcessComplete (success);
		}
	}


}
