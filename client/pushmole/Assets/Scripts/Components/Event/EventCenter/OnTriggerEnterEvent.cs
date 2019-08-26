using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OnTriggerEnterEvent : IEventParam
{
	public INode mSource;
	public Unit mOwner;
	public GameObject mTarget;
	public float mDamage;


	public OnTriggerEnterEvent (INode source, Unit owner, GameObject target, float damage)
	{
		mSource = source;
		mOwner = owner;
		mTarget = target;
		mDamage = damage;
	}

}
