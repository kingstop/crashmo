using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : IEntity where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public virtual void Init()
    {
        Debug.Log(string.Format("[{0}] Init ", this.GetType().Name));
    }

    public virtual void Update(float deltaTime)
    {
    }

    public virtual void Release()
    {
        Debug.Log(string.Format("[{0}] Release ", this.GetType().Name));
    }
}
