using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Unit
/// </summary>

public class Unit : LogicNode
{
	public Unit mTarget;

	public EAIState mAIState;

	public EUnitType mUnitType;

	public INode mBehaviorTree;

	//  组合
	public Transform mTransform;

	//  Move Component .
	UnitMove mMoveComponent;

	UnitData mData;

	//		NavMesh .
	//	PathFind mPathFind;

	//		TODO:以下数据将从配置中读取
	//		视野
	public float mDefenseDistance = 3;
	public float mAttackDistance = 1;
	public float mMaxChaseDistance = 5;
	public float mSpeed = 1;
	public float mHP = 100;
	public float mATK = 10;

	public float mFlightObjectSpeed = 5;

	public int mSize = 1;

	//		data


	Map mMap;

	public Unit ()
	{
		AddNode (mMoveComponent = new UnitMove ());
		mMoveComponent.OnPosition += this.OnSetPosition;
	}

	public Unit (Transform transform, Map map) : this ()
	{
		this.mTransform = transform;
		this.mMap = map;

		this.mData = new UnitData ();
		float hp = this.mData [EUnitAttribute.HP];
	}

	public override void Init ()
	{
		base.Init ();
		this.mTransform.name = this.GetType ().Name + " " + this.ID.ToString (); 
		this.SendPositionEvent (this.mTransform.position);
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		//		TODO:重构到基类?
		if (this.mBehaviorTree != null && !this.IsDead ()) {
			this.mBehaviorTree.Update (deltaTime);
		}
	}

	public override void Leave ()
	{
		base.Leave ();

		if (mTransform != null)
			mTransform.gameObject.SetActive (false);
	}

	public override void Release ()
	{
		if (mTransform != null)
			GameObject.DestroyObject (mTransform.gameObject);
		base.Release ();
	}

	//		发现敌人
	public bool IsSeeEnemy ()
	{
		Unit enemy = (Parent as UnitManager).GetNearestEnemy (this);
		if (enemy != null && Vector3.Distance (this.mTransform.position, enemy.mTransform.position) <= mDefenseDistance) {
			this.mTarget = enemy;
			return true;
		}

		return false;
	}

	public bool IsTargetInAttackDistance ()
	{
		if (mTarget != null && Vector3.Distance (mTransform.position, this.mTarget.mTransform.position) <= this.mAttackDistance)
			return true;
		return false;
	}

	public bool IsDead ()
	{
		return this.mAIState == EAIState.Death;
	}

	public void Move (Vector3 position)
	{
		Tile current = mMap.GetTile (mTransform.position, mSize);
		Tile target = mMap.GetTile (position, mSize);

		mMoveComponent.SetPosition (mTransform.position);
		mMoveComponent.SetSpeed (this.mSpeed);
		mMoveComponent.Move (mMap, current, target, mSize);
	}

	public void Move (Vector3 direction, float deltaTime)
	{
		mMoveComponent.SetSpeed (this.mSpeed);
		mMoveComponent.SetPosition (mTransform.position);
		mMoveComponent.Move (mMap, direction, deltaTime, mSize);
	}

	public void CancelMove ()
	{
		mMoveComponent.CancelMove ();
	}

	public void OnSetPosition (Vector3 position, Vector3 direction)
	{
		this.mTransform.position = position;

		if (direction != Vector3.zero) {	//		计算旋转值。
			Quaternion rotation = Quaternion.LookRotation (direction);
			this.mTransform.rotation = rotation;
		} else {
			Debug.LogWarning (this.mTransform.position + " " + " " + position + " " + direction);
		}

		this.SendPositionEvent (position);
	}

	//	void OnHit (LogicNode source, GameObject target)
	//	{
	//		this.Parent.OnEvent (EventTable.OnHit, new HitEvent (source, this, target, this.mATK));
	//	}

	public void DoDamage (float damage)
	{
		mHP -= damage;

		if (this.mHP <= 0) {
			this.mAIState = EAIState.Death;
			this.mTransform.gameObject.SetActive (false);
			this.DoDeathEffect (this.mTransform.position);
		}

		new UIDamageText (damage, this.mTransform, 3, 50);
	}

	public void Attack ()
	{
		if (mTarget != null)
			this.mTransform.forward = mTarget.mTransform.position - this.mTransform.position;

		GameObject flightGameObj = ResourceLoader.Create ("FlightObject");
		FlightObject flightObject = new FlightObject (flightGameObj.transform, this.mMap, this);

		flightGameObj.transform.position = this.mTransform.position + new Vector3 (0, 0.5f, 0);
		flightGameObj.transform.forward = this.mTransform.forward;
		flightGameObj.transform.localScale = 0.8f * Vector3.one;

		flightObject.mSpeed = this.mFlightObjectSpeed;
		flightObject.mDirection = this.mTransform.forward;

		AddNode (flightObject);
		flightObject.Init ();

		flightObject.SetGameObject (flightGameObj);
	}

	void DoDeathEffect (Vector3 position)
	{
		//GameObject effect = ResourceLoader.Create ("Art/Prefabs/Death_Effect");
		GameObject effect = ResourceLoader.Create ("Death_Effect");
		effect.transform.position = position;
		effect.AddComponent<AutoReleaseComponent> ();
	}

	public override void RemoveNode (INode node)
	{
		base.RemoveNode (node);
		if (node is FlightObject) {
			node.Release ();
		}
	}

	void SendPositionEvent (Vector3 position)
	{
		if (this.mUnitType == EUnitType.Player) {
			AloneEventCenter<PlayerPositionChangeEvent>.Instance.OnEvent (new PlayerPositionChangeEvent (position));
		}
	}
}
