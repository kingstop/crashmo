using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data creator. 数据生成器，这里的所有数据生成，在实际项目中将从配置中读取。
/// </summary>

public class UnitCreater 
{

	/// <summary>
	/// Creates the player.
	/// </summary>
	/// <returns>The player.</returns>
	/// <param name="map">Map.</param>
	/// <param name="path">Path.</param>
	/// <param name="position">Position.</param>
	public static Unit CreatePlayer(Map map,string path,Vector3 position)
	{
		GameObject prefab = Resources.Load<GameObject> (path);
		GameObject go = GameObject.Instantiate<GameObject>(prefab) ;
		go.transform.position = position;

		Unit unit = new Unit (go.transform,map);
		unit.mUnitType = EUnitType.Player;
		unit.mSpeed = 1.2f;
		unit.mHP = 300;
		unit.mATK = 20;

//		#if UNITY_EDITOR
//		unit.mHP = 30 ;
//		#endif

		return unit;
	}



	/// <summary>
	/// Creates the guard. 守卫者
	/// </summary>
	/// <returns>The guard.</returns>

	public static Unit CreateGuard(Map map,string path,Vector3 position)
	{
		GameObject prefab = Resources.Load<GameObject> (path);
		GameObject go = GameObject.Instantiate<GameObject>(prefab) ;
		go.transform.position = position;
		go.transform.forward = Vector3.back;

		Unit unit = new Unit (go.transform,map);
		unit.mUnitType = EUnitType.Monster;
		unit.mDefenseDistance = 5;
		unit.mAttackDistance = 4;
		unit.mMaxChaseDistance = 6;
		unit.mSpeed = 0.5f;
		unit.mFlightObjectSpeed = 3f;

		//		behavior tree . 
		SelectorNode tree  = new SelectorNode() ;
		SelectorNode root = new  SelectorNode ();

		ConditionNode seeEnemy = new ConditionNode ();
		seeEnemy.ConditionFunction = unit.IsSeeEnemy;

		Chase chase = new Chase ();
		chase.mUnit = unit;

		Attack attack = new Attack ();
		attack.mUnit = unit;
		attack.AttackTime = 1;


		SequenceNode deathSequence = new SequenceNode ();
		ConditionNode deathCondition = new ConditionNode ();
		deathCondition.ConditionFunction = unit.IsDead;
		deathSequence.AddNode (deathCondition);

		SequenceNode attackSequence = new SequenceNode ();
		ConditionNode canAttack = new ConditionNode ();
		canAttack.ConditionFunction = unit.IsTargetInAttackDistance;
		attackSequence.AddNode (canAttack);
		attackSequence.AddNode (attack);

		SequenceNode chaseSequence = new SequenceNode ();
		chaseSequence.AddNode (seeEnemy);
		chaseSequence.AddNode (chase);

		root.AddNode (deathSequence);
		root.AddNode (attackSequence);
		root.AddNode (chaseSequence);


		//unit.mBehaviorTree = root;
		tree.AddNode(root);
		unit.mBehaviorTree = tree;

		return unit;


	}






}
