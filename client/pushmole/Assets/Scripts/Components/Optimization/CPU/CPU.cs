using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;

/// <summary>
/// <CPU>
/// </summary>

    public class CPU
    {
        public virtual void Presentation()
        {
        }

        public virtual void Reason()
        {
        }

        public static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }

        public static void BeginSample(string name)
        {
            Profiler.BeginSample(name);
            //GameObject go;
            //go.GetComponent<>();
        }

        public static void EndSample()
        {
            Profiler.EndSample();
        }
    }

