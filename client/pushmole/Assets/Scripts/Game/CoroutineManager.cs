using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coroutine manager.
/// </summary>

public class CoroutineManager : Node
{
	int mCoroutineCount ;		//		统一计数器。
	CoroutineComponent mComponent ;

	public CoroutineManager()
	{
		mComponent = new GameObject (typeof(CoroutineManager).Name).AddComponent<CoroutineComponent> ();
	}

	public Coroutine StartCoroutine(IEnumerator routine)
	{
//		mCoroutineCount++;
		return mComponent.StartCoroutine (routine);
	}

	public override void Release ()
	{
		base.Release ();
		GameObject.Destroy (mComponent.gameObject);
		mComponent = null;
	}

}

public class CoroutineComponent : MonoBehaviour
{

}
