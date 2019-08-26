using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System ;

/// <summary>
/// Condition node.
/// </summary>

public class ConditionNode : Node
{
	public Func<bool> ConditionFunction ;

	public override void Enter ()
	{
		base.Enter ();

		//		if condition is true ,
		if (ConditionFunction != null && ConditionFunction () == true)
		{
			RunningStatus = RunningStatus.Success;
		}
		else
		{
			RunningStatus = RunningStatus.Failure;
		}
	}

	public override void Leave ()
	{
		base.Leave ();
	}

}
