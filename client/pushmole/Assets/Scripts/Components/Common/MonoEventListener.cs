using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Mono event listener.
/// </summary>

public class MonoEventListener : MonoBehaviour
{
	public INode mSource;

	public Unit mOwner;

	public float mDamage ;

	void OnTriggerEnter (Collider collider)
	{
		Debug.LogWarning ("TODO:优化该类的成员变量.");
		SendEvent (new OnTriggerEnterEvent (mSource,mOwner, collider.gameObject,mDamage));
	}


	void SendEvent<T> (T Event) where T : IEventParam
	{
		AloneEventCenter<T>.Instance.OnEvent (Event);

		Debug.LogWarning ("MonoEventListener:222");

//		if (Game.Instance != null && Game.Instance.mEventCenter != null)
//		{
//			//Game.Instance.mEventCenter.OnEvent (EventParam);
//
//		}
	}

}
