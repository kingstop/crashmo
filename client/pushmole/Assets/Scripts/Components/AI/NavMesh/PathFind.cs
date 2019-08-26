using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <寻路
/// Nav Mesh Obstacle: Carve 选中会自动绕路
/// Unity多单位战斗寻路问题的一种解决办法 : http://www.cnblogs.com/crazylights/p/4033474.html
/// Unity5.6新功能High-level NavMesh之实时动态烘培NavMesh ：http://blog.csdn.net/Dracoooo/article/details/73929201>
/// </summary>

public class PathFind
{
    public Action OnArrive;

	Transform mTarget;

    NavMeshAgent mAgent;

	/// <summary>
	/// 移动状态 . 
	/// </summary>

	public enum EMoveState
	{
		Still,
		Moving,
	}

    EMoveState mState;

    public PathFind(Transform target)
    {
        this.mTarget = target;
        this.mAgent = this.mTarget.gameObject.AddComponent<NavMeshAgent>();
    }

    public void SetRadius(float radius)
    {
        this.mAgent.radius = radius;
    }

    public void Move(Vector3 targetPosition)
    {
        this.mAgent.SetDestination(targetPosition);
        this.mState = EMoveState.Moving;
    }

    public void Update(float deltaTime)
    {
        switch (this.mState)
        {
            case EMoveState.Still:
                break;
            case EMoveState.Moving:
                if (this.IsMoving() == false)
                {
                    this.mState = EMoveState.Still;
                    this.OnArrive();
                }
                break;
        }
    }

    bool IsMoving()
    {
        if (this.mAgent.pathPending)
            return false;
        if (this.mAgent.remainingDistance > mAgent.stoppingDistance)
            return false;
        if (this.mAgent.velocity != Vector3.zero)
            return false;

        return true;
    }

    public void SetSpeed(float speed)
    {
        this.mAgent.speed = speed;
    }

}
