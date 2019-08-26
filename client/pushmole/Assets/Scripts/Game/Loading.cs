using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loading : 管理加载界面的逻辑，数据和界面。
/// </summary>

public class Loading : LogicNode
{
	UILoading uiLoading ;

	public override void Enter ()
	{
		base.Enter ();

		uiLoading = new UILoading ();
		uiLoading.Init ();
		uiLoading.Enter ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);
		uiLoading.Update (deltaTime);
		this.RunningStatus = uiLoading.RunningStatus ;
	}

	public override void Leave ()
	{
		base.Leave ();
		uiLoading.Leave ();
	}


}
