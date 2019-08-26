using UnityEngine;
using System.Collections;

///  <!--
///  EventManager扩展
///  支持委托函数有返回值的情况
///  -->

public class EventManagerExtension : EventManager
{
    #region Extend to Support ReturnResult：有返回值的事件支持

    /// <summary>
    /// 作为具有返回值类型的委托试用
    /// </summary>
    public delegate TResult Func<in T, in U, in V, in W, in X, out TResult>(T arg1, U arg2, V arg3, W arg4, X arg5);


    public static void RegisterEvent<TResult>(string name, System.Func<TResult> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, TResult>(string name, System.Func<T, TResult> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, TResult>(string name, System.Func<T, U, TResult> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, V, TResult>(string name, System.Func<T, U, V, TResult> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, V, W, TResult>(string name, System.Func<T, U, V, W, TResult> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, V, W, X, TResult>(string name, Func<T, U, V, W, X, TResult> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<TResult>(EventTable name, System.Func<TResult> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, TResult>(EventTable name, System.Func<T, U, TResult> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, V, TResult>(EventTable name, System.Func<T, U, V, TResult> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, V, W, TResult>(EventTable name, System.Func<T, U, V, W, TResult> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, V, W, X, TResult>(EventTable name, Func<T, U, V, W, X, TResult> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent<TResult>(string name, System.Func<TResult> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, TResult>(string name, System.Func<T, TResult> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U, TResult>(string name, System.Func<T, U, TResult> handler)
    {
        _UnregisterEvent(name, handler);
    }
    public static void UnregisterEvent<T, U, V, TResult>(string name, System.Func<T, U, V, TResult> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U, V, W, TResult>(string name, System.Func<T, U, V, W, TResult> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U, V, W, X, TResult>(string name, Func<T, U, V, W, X, TResult> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static TResult SendEventHasReturn<TResult>(string name)
    {
        System.Func<TResult> func = GetDelegate(name, typeof(System.Func<TResult>)) as System.Func<TResult>;
        if (func != null)
        {
            return func();
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, TResult>(string name, T arg1)
    {
        System.Func<T, TResult> func = GetDelegate(name, typeof(System.Func<T, TResult>)) as System.Func<T, TResult>;
        if (func != null)
        {
            return func(arg1);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, TResult>(string name, T arg1, U arg2)
    {
        System.Func<T, U, TResult> func = GetDelegate(name, typeof(System.Func<T, U, TResult>)) as System.Func<T, U, TResult>;
        if (func != null)
        {
            return func(arg1, arg2);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, V, TResult>(string name, T arg1, U arg2, V arg3)
    {
        System.Func<T, U, V, TResult> func = GetDelegate(name, typeof(System.Func<T, U, V, TResult>)) as System.Func<T, U, V, TResult>;
        if (func != null)
        {
            return func(arg1, arg2, arg3);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, V, W, TResult>(string name, T arg1, U arg2, V arg3, W arg4)
    {
        System.Func<T, U, V, W, TResult> func = GetDelegate(name, typeof(System.Func<T, U, V, W, TResult>)) as System.Func<T, U, V, W, TResult>;
        if (func != null)
        {
            return func(arg1, arg2, arg3, arg4);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, V, W, X, TResult>(string name, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Func<T, U, V, W, X, TResult> func = GetDelegate(name, typeof(Func<T, U, V, W, X, TResult>)) as Func<T, U, V, W, X, TResult>;

        if (func != null)
        {
            return func(arg1, arg2, arg3, arg4, arg5);
        }
        return default(TResult);
    }


    public static TResult SendEventHasReturn<TResult>(EventTable name)
    {
        System.Func<TResult> func = GetDelegate(name.ToString(), typeof(System.Func<TResult>)) as System.Func<TResult>;
        if (func != null)
        {
            return func();
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, TResult>(EventTable name, T arg1)
    {
        System.Func<T, TResult> func = GetDelegate(name.ToString(), typeof(System.Func<T, TResult>)) as System.Func<T, TResult>;
        if (func != null)
        {
            return func(arg1);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, TResult>(EventTable name, T arg1, U arg2)
    {
        System.Func<T, U, TResult> func = GetDelegate(name.ToString(), typeof(System.Func<T, U, TResult>)) as System.Func<T, U, TResult>;
        if (func != null)
        {
            return func(arg1, arg2);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, V, TResult>(EventTable name, T arg1, U arg2, V arg3)
    {
        System.Func<T, U, V, TResult> func = GetDelegate(name.ToString(), typeof(System.Func<T, U, V, TResult>)) as System.Func<T, U, V, TResult>;
        if (func != null)
        {
            return func(arg1, arg2, arg3);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, V, W, TResult>(EventTable name, T arg1, U arg2, V arg3, W arg4)
    {
        System.Func<T, U, V, W, TResult> func = GetDelegate(name.ToString(), typeof(System.Func<T, U, V, W, TResult>)) as System.Func<T, U, V, W, TResult>;
        if (func != null)
        {
            return func(arg1, arg2, arg3, arg4);
        }
        return default(TResult);
    }

    public static TResult SendEventHasReturn<T, U, V, W, X, TResult>(EventTable name, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Func<T, U, V, W, X, TResult> func = GetDelegate(name.ToString(), typeof(Func<T, U, V, W, X, TResult>)) as Func<T, U, V, W, X, TResult>;

        if (func != null)
        {
            return func(arg1, arg2, arg3, arg4, arg5);
        }
        return default(TResult);
    }
    #endregion
}
