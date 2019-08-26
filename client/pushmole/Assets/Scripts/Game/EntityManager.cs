using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏实体
/// </summary>

public class EntityManager : MonoBehaviour
{

    //List<Entity> mEntitys = new List<Entity>();
    //Dictionary<int, Entity> mEntityDic = new Dictionary<int, Entity>();

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
