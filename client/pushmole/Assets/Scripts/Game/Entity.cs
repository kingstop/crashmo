using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏实体
/// </summary>

public class Entity : MonoBehaviour
{
    static int mAutoID;

    public int mID;

    GameObject mGameObj;

    public Entity()
    {
        mAutoID++;
        mID = mAutoID;
    }

    public virtual void Init()
    {
    }

    public virtual void OnUpdate(float deltaTime)
    {
    }

    public virtual void Release()
    {
    }

}
