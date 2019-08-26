using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Flying objects . 
/// </summary>

public class FlightObject : LogicNode
{
	//public Action<LogicNode, GameObject> OnHit;

	public Unit mOwner;

	public float mSpeed;
	public Vector3 mDirection;

	Transform mTransform;
	Map mMap;

	MonoEventListener mMonoEventListener;

	public FlightObject (Transform transform, Map map,Unit owner)
	{
		this.mTransform = transform;
		this.mMap = map;
		this.mOwner = owner;

		this.mMonoEventListener = this.mTransform.GetComponent<MonoEventListener> ();
		if (this.mMonoEventListener == null)
			this.mMonoEventListener = this.mTransform.gameObject.AddComponent<MonoEventListener> ();

		this.mMonoEventListener.mSource = this;
		this.mMonoEventListener.mOwner = mOwner;
		this.mMonoEventListener.mDamage = mOwner.mATK;

		this.mTransform.name = this.GetType ().Name + " " + this.ID.ToString ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);
		this.Flying (deltaTime);
		this.OutOfMap ();
	}

	void Flying (float deltaTime)
	{
		this.mTransform.position = this.mTransform.position + mDirection * mSpeed * deltaTime;
	}

//	void OnHitTarget (Collider target)
//	{
////		if (OnHit != null)
////		{
////			OnHit (this, target.gameObject);
////		}
//	}

	void OutOfMap ()
	{
		if (mMap.IsOutOfMap (this.mTransform.position))
		{
			Parent.RemoveNode (this);
			Release ();
		}
	}

	public override void Release ()
	{
		base.Release ();

//		OnHit = null;
		GameObject.DestroyObject (mMonoEventListener);
		if (mTransform != null)
			GameObject.DestroyObject (mTransform.gameObject);
	}
}
