using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour/*, IEntity*/ where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    GameObject.DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    public virtual void Init()
    {
        Debug.Log(this.GetType().Name + " Init！");
    }

    public virtual void Release()
    {
        if (instance.gameObject != null)
        {
            GameObject.DestroyObject(instance.gameObject);
        }
    }

    public virtual void OnUpdate(float deltaTime)
    {
    }
}
