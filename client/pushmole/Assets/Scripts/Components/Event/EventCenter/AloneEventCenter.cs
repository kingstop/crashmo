using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

//			这里有个问题是：有些事件，比如用户操作事件，可能每帧都在触发，会造成每帧都在生成新的对象，造成时间和空间的分配；
//			TODO：使用对象池解决这个问题？！
//			对象池将解决3个问题：时间复杂度，空间复杂度，内存碎片。

public class AloneEventCenter <T> : Singleton<AloneEventCenter<T>> where T : IEventParam
{
	private List<INode> mListeners;

	Dictionary<INode,Action<T>> mListenerActionDic;

	private Queue<T> mEventQueue;

	private Queue<T> mWaitingQueue;



	public void AddListener (INode listener, Action<T> action)
	{
		if (mListeners == null)
			mListeners = new List<INode> ();
		
		if (!mListeners.Contains (listener))
			mListeners.Add (listener);

		if (mListenerActionDic == null)
			mListenerActionDic = new Dictionary<INode, Action<T>> ();

		mListenerActionDic [listener] = action;
	}


	public void RemoveListener (INode listener)
	{
		if ( mListeners == null )
			return;

		mListeners.Remove (listener);

		if ( mListenerActionDic != null )
			mListenerActionDic.Remove (listener);
	}


	public void Clear ()
	{
		if (mListeners != null)
			mListeners.Clear ();
		if (mListenerActionDic != null)
			mListenerActionDic.Clear ();
	}


	public void OnEvent (T Event)
	{
		if (mWaitingQueue == null)
		{
			mWaitingQueue = new Queue<T> ();
		}
		mWaitingQueue.Enqueue (Event);

		this.Update ();
	}


	public void Update ()
	{
		if (mListeners == null)
			return;

		while (mWaitingQueue != null && mWaitingQueue.Count > 0)
		{
			if (mEventQueue == null)
				mEventQueue = new Queue<T> ();
			
			mEventQueue.Enqueue (mWaitingQueue.Dequeue ());
		}

		while (mEventQueue != null && mEventQueue.Count > 0)
		{
			ProcessEvent (mEventQueue.Dequeue ());
		}
	}


	public void ProcessEvent (T Event)
	{
		if (mListeners != null)
		{
			INode listener = null;
			Action<T> action = null;
			for (int i = 0; i < mListeners.Count; i++)
			{
				listener = mListeners [i];
				action = mListenerActionDic[listener] ;
				action (Event);
			}
			listener = null;
			action = null;
		}
	}

}
