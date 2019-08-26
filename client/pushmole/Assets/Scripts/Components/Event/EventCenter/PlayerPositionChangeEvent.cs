using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPositionChangeEvent : IEventParam
{
	public Vector3 mPosition ;

	public PlayerPositionChangeEvent()
	{
		
	}

	public PlayerPositionChangeEvent(Vector3 position)
	{
		this.mPosition = position;
	}
}
