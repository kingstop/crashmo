using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UserInputEvent : IEventParam
{
	public enum UserInputCommand
	{
		Unknown,
		Up,
		Down,
		Left,
		Right,

		Drag,

		Attack,
	}

	public UserInputCommand mCommand ;

	public Vector3 mDirection;

	public float mDeltaTime ;


	public UserInputEvent()
	{
		
	}


	public UserInputEvent(UserInputCommand command , Vector3 direction , float deltaTime)
	{
		this.mCommand = command;
		this.mDirection = direction;
		this.mDeltaTime = deltaTime;
	}

}
