using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input manager.
/// </summary>

public class InputManager : LogicTree
{


	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		if (Input.GetKey (KeyCode.A))
		{
			AloneEventCenter<UserInputEvent>.Instance.OnEvent (new UserInputEvent (UserInputEvent.UserInputCommand.Left, Vector3.left, deltaTime));
		}
		else if (Input.GetKey (KeyCode.S))
		{
			AloneEventCenter<UserInputEvent>.Instance.OnEvent (new UserInputEvent (UserInputEvent.UserInputCommand.Down, Vector3.back, deltaTime));
		}
		else if (Input.GetKey (KeyCode.D))
		{
			AloneEventCenter<UserInputEvent>.Instance.OnEvent (new UserInputEvent (UserInputEvent.UserInputCommand.Right, Vector3.right, deltaTime));
		}
		else if (Input.GetKey (KeyCode.W))
		{
			AloneEventCenter<UserInputEvent>.Instance.OnEvent (new UserInputEvent (UserInputEvent.UserInputCommand.Up, Vector3.forward, deltaTime));
		}
	}



}
