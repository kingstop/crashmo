#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RunFrameWork
{
    [MenuItem("GameObject/Run NoFrameWork", false, 0)]
    public static void RunFuture()
    {
        Main main = GameObject.FindObjectOfType<Main>();
        if (main == null)
        {
            main = new GameObject(typeof(Main).Name).AddComponent<Main>();
        }
    }
}


#endif