using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auto release component.
/// </summary>

public class AutoReleaseComponent : MonoBehaviour
{
	public float mLifeTime = 10;

	void Awake ()
	{
		this.Invoke ("Release", mLifeTime);
	}

	void Release ()
	{
		GameObject.DestroyObject (gameObject);
		GameObject.DestroyObject (this);
	}
}
