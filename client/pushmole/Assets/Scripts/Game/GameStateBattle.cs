using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game state battle.
/// </summary>

public class GameStateBattle : Node
{
	Battle battle;

	public override void Enter ()
	{
		base.Enter ();
		battle = new Battle ();
		battle.Init ();
		battle.Enter ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);
		battle.Update (deltaTime);

		if (battle.RunningStatus == RunningStatus.Failure || battle.RunningStatus == RunningStatus.Success)
			this.RunningStatus = RunningStatus.Success;
	}

	public override void Leave ()
	{
		base.Leave ();
		battle.Leave ();
		battle.Release ();
		battle = null;
	}

	public override void Release ()
	{
		base.Release ();
	}
}
