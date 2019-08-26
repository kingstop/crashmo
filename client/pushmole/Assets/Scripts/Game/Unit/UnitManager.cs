using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit manager.
/// </summary>

public class UnitManager : LogicTree
{
	Dictionary<int, Unit> mUnitDic;
	Dictionary<GameObject,Unit> mObjUnitDic;

	Unit mPlayer;

	Map mMap;

	public UnitManager ()
	{
	}

	public UnitManager (Map map) : this ()
	{
		this.mMap = map;

		this.mObjUnitDic = new Dictionary<GameObject, Unit> ();

		mPlayer = UnitCreater.CreatePlayer (mMap, "Art/Prefabs/Player", new Vector3 (10, 0, 2));
		AddNode (mPlayer);

		Unit unit = UnitCreater.CreateGuard (mMap, "Art/Prefabs/Enemy_Flyer", new Vector3 (5.5f, 0, 10));
		AddNode (unit);

		unit = UnitCreater.CreateGuard (mMap, "Art/Prefabs/Enemy_Flyer", new Vector3 (14.5f, 0, 10));
		AddNode (unit);

		unit = UnitCreater.CreateGuard (mMap, "Art/Prefabs/Enemy_Flyer", new Vector3 (10, 0, 15));
		AddNode (unit);

		unit = UnitCreater.CreateGuard (mMap, "Art/Prefabs/Enemy_Guard", new Vector3 (4, 0, 10));
		unit.mDefenseDistance = 3;
		unit.mMaxChaseDistance = 3;
		unit.mAttackDistance = 3;
		unit.mFlightObjectSpeed = 4; 
		unit.mSpeed = 0;
		AddNode (unit);

		unit = UnitCreater.CreateGuard (mMap, "Art/Prefabs/Enemy_Guard", new Vector3 (16, 0, 10));
		unit.mDefenseDistance = 3;
		unit.mMaxChaseDistance = 3;
		unit.mAttackDistance = 3;
		unit.mFlightObjectSpeed = 4; 
		unit.mSpeed = 0;
		AddNode (unit);

		unit = UnitCreater.CreateGuard (mMap, "Art/Prefabs/Enemy_Go", new Vector3 (10, 0, 18));
		unit.mDefenseDistance = 5;
		unit.mMaxChaseDistance = 5;
		unit.mAttackDistance = 5;
		unit.mFlightObjectSpeed = 5; 
		unit.mATK = 50;
		unit.mSpeed = 0;
		AddNode (unit);
	}

	public override void AddNode (INode node)
	{
		base.AddNode (node);

		Unit unit = node as Unit;
		this.mObjUnitDic [unit.mTransform.gameObject] = unit;
		unit.mTransform.gameObject.AddComponent<BoxCollider> ().isTrigger = true;
		unit.mTransform.gameObject.AddComponent<Rigidbody> ().useGravity = false;
	}

	public override void Init ()
	{
		base.Init ();
		AloneEventCenter<OnTriggerEnterEvent>.Instance.AddListener (this, this.OnEvent);
		AloneEventCenter<UserInputEvent>.Instance.AddListener (this,this.OnEvent);
	}

	public override void Release ()
	{		
		base.Release ();
		AloneEventCenter<OnTriggerEnterEvent>.Instance.RemoveListener (this);
		AloneEventCenter<UserInputEvent>.Instance.RemoveListener (this);
	}

	//		TODO:改成主动下发的方式。
	public Unit GetNearestEnemy (Unit source)
	{
		if (source == null)
			return null;

		Unit enemy = null;
		switch (source.mUnitType)
		{
		case EUnitType.Monster:
			float min = float.MaxValue;

			for (int i = 0; i < Children.Count; i++)
			{
				Unit unit = Children [i] as Unit;
				if (unit.mUnitType == source.mUnitType)
					continue;

				float distance = Vector3.Distance (unit.mTransform.position, source.mTransform.position);
				if (distance < min)
				{
					min = distance;
					enemy = unit;
				}
			}

			break;
		}
		return enemy;
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);
	}

	public bool IsPlayerDead ()
	{
		return mPlayer != null && mPlayer.IsDead ();
	}

	public bool IsAllMonstersDead ()
	{
		foreach (Unit unit in Children)
		{
			if (unit != null && unit.mUnitType == EUnitType.Monster && !unit.IsDead ())
				return false;
		}

		return true;
	}


	void OnEvent (OnTriggerEnterEvent Event)
	{
		Debug.Log ("OnTriggerEnterEvent：");

		if (this.mObjUnitDic.ContainsKey (Event.mTarget))
		{
			Unit owner = Event.mOwner;
			Unit target = this.mObjUnitDic [Event.mTarget];

			if (owner.mUnitType != target.mUnitType)
			{
				this.mObjUnitDic [Event.mTarget].DoDamage (Event.mDamage);

				Debug.LogWarning ("Damage：" + Event.mDamage);

				owner.RemoveNode (Event.mSource);		//		销毁子弹。
			}
		}
	}



	public void OnEvent (UserInputEvent Event)
	{
		switch (Event.mCommand)
		{
		case UserInputEvent.UserInputCommand.Up:
		case UserInputEvent.UserInputCommand.Down:
		case UserInputEvent.UserInputCommand.Left:
		case UserInputEvent.UserInputCommand.Right:
			mPlayer.Move (Event.mDirection, Event.mDeltaTime);
			break;

		case UserInputEvent.UserInputCommand.Drag:
			Vector2 direction = Utility.RotateMatrix ((Vector2)Event.mDirection, -45);
			mPlayer.Move (new Vector3(direction.x,0,direction.y),Time.fixedDeltaTime);
			break;

		case UserInputEvent.UserInputCommand.Attack:
			mPlayer.Attack ();
			break;

		default:
			Debug.LogWarning ("UserInput:"+Event.mCommand);
			break;
		}
	}

}
