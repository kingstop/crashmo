using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// Event center. 
/// </summary>

public class EventCenter : Node
{
	Queue<IEventParam> mEventQueue = new Queue<IEventParam> ();

	Queue<IEventParam> mWaitingQueue = new Queue<IEventParam> ();

	public override void Init ()
	{
		base.Init ();
		Debug.LogWarning ("TODO:将之前的OnEvent统一改成EventCenter!");
	}

	public void OnEvent (IEventParam Event)
	{
		mWaitingQueue.Enqueue (Event);
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		while (mWaitingQueue.Count > 0)
		{
			mEventQueue.Enqueue (mWaitingQueue.Dequeue ());
		}

		while (mEventQueue.Count > 0)
		{
			IEventParam Event = mEventQueue.Dequeue ();
			ProcessEvent (Event);
		}
	}

	void ProcessEvent (IEventParam Event)
	{
		
	}

	public void AddListener (Node node)
	{
		
	}
}
