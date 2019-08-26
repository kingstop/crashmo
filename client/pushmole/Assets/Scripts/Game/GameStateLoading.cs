using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loading behavior.
/// </summary>

public class GameStateLoading : Node
{
	Loading loading ;

	public override void Enter ()
	{
		base.Enter ();
		loading = new Loading ();
		loading.Init ();
		loading.Enter ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);
		loading.Update (deltaTime);
		this.RunningStatus = loading.RunningStatus;
	}

	public override void Leave ()
	{
		base.Leave ();
		loading.Leave ();
	}

}
