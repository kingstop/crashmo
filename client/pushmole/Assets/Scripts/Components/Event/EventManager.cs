/*
 * EventManager.cs
 * 事件机制。
 * */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager
{
    private static Dictionary<Type, Dictionary<string, Delegate>> mEventTable;

    public delegate void Action<T, U, V, W, X>(T arg1, U arg2, V arg3, W arg4, X arg5);

    #region Event
    //注册事件
    protected static void _RegisterEvent(string name, Delegate handler)
    {
        if (mEventTable == null)
        {
            mEventTable = new Dictionary<Type, Dictionary<string, Delegate>>();
        }
        Dictionary<string, Delegate> dictionary;
        if (!mEventTable.TryGetValue(handler.GetType(), out dictionary))
        {
            dictionary = new Dictionary<string, Delegate>();
            mEventTable.Add(handler.GetType(), dictionary);
        }
        Delegate a;
        if (dictionary.TryGetValue(name, out a))
        {
            dictionary[name] = Delegate.Combine(a, handler);
        }
        else
        {
            dictionary.Add(name, handler);
        }
    }

    protected static void _UnregisterEvent(string name, Delegate handler)
    {
        if (mEventTable == null)
            return;

        Dictionary<string, Delegate> dictionary;
        Delegate source;
        if (mEventTable.TryGetValue(handler.GetType(), out dictionary) && dictionary.TryGetValue(name, out source))
        {
            dictionary[name] = Delegate.Remove(source, handler);
        }
    }

    public static void SendEvent(EventTable name)
    {
        Action action = GetDelegate(name.ToString(), typeof(Action)) as Action;
        if (action != null)
        {
            action.Invoke();
        }
    }

    protected static Delegate GetDelegate(string name, Type type)
    {
        Dictionary<string, Delegate> dictionary;
        Delegate result;
        if (mEventTable != null && mEventTable.TryGetValue(type, out dictionary) && dictionary.TryGetValue(name, out result))
        {
            return result;
        }
        return null;
    }

    #endregion

    public static void RegisterEvent(string name, System.Action handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T>(string name, Action<T> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U>(string name, Action<T, U> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, V>(string name, Action<T, U, V> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, V, W>(string name, Action<T, U, V, W> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void RegisterEvent<T, U, V, W, X>(string name, Action<T, U, V, W, X> handler)
    {
        _RegisterEvent(name, handler);
    }

    public static void SendEvent<T>(string name, T arg1)
    {
        Action<T> action = GetDelegate(name, typeof(Action<T>)) as Action<T>;
        if (action != null)
        {
            action.Invoke(arg1);
        }
    }

    public static void SendEvent<T, U>(string name, T arg1, U arg2)
    {
        Action<T, U> action = GetDelegate(name, typeof(Action<T, U>)) as Action<T, U>;
        if (action != null)
        {
            action.Invoke(arg1, arg2);
        }
    }

    public static void SendEvent<T, U, V>(string name, T arg1, U arg2, V arg3)
    {
        Action<T, U, V> action = GetDelegate(name, typeof(Action<T, U, V>)) as Action<T, U, V>;
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3);
        }
    }

    public static void SendEvent<T, U, V, W>(string name, T arg1, U arg2, V arg3, W arg4)
    {
        Action<T, U, V, W> action = GetDelegate(name, typeof(Action<T, U, V, W>)) as Action<T, U, V, W>;
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3, arg4);
        }
    }

    public static void SendEvent<T, U, V, W, X>(string name, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Action<T, U, V, W, X> action = GetDelegate(name, typeof(Action<T, U, V, W, X>)) as Action<T, U, V, W, X>;
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3, arg4, arg5);
        }
    }

    public static void UnregisterEvent(string name, System.Action handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T>(string name, Action<T> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U>(string name, Action<T, U> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U, V>(string name, Action<T, U, V> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U, V, W>(string name, Action<T, U, V, W> handler)
    {
        _UnregisterEvent(name, handler);
    }

    public static void UnregisterEvent<T, U, V, W, X>(string name, Action<T, U, V, W, X> handler)
    {
        _UnregisterEvent(name, handler);
    }

    #region Extend to Support EventTable.cs 
    public static void RegisterEvent(EventTable name, System.Action handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T>(EventTable name, Action<T> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, TResult>(EventTable name, System.Func<T, TResult> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U>(EventTable name, Action<T, U> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, V>(EventTable name, Action<T, U, V> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, V, W>(EventTable name, Action<T, U, V, W> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void RegisterEvent<T, U, V, W, X>(EventTable name, Action<T, U, V, W, X> handler)
    {
        _RegisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent(EventTable name, System.Action handler)
    {
        _UnregisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent<T>(EventTable name, Action<T> handler)
    {
        _UnregisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent<T, U>(EventTable name, Action<T, U> handler)
    {
        _UnregisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent<T, U, V>(EventTable name, Action<T, U, V> handler)
    {
        _UnregisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent<T, U, V, W>(EventTable name, Action<T, U, V, W> handler)
    {
        _UnregisterEvent(name.ToString(), handler);
    }

    public static void UnregisterEvent<T, U, V, W, X>(EventTable name, Action<T, U, V, W, X> handler)
    {
        _UnregisterEvent(name.ToString(), handler);
    }

    public static void SendEvent<T>(EventTable name, T arg1)
    {
        Action<T> action = GetDelegate(name.ToString(), typeof(Action<T>)) as Action<T>;
        if (action != null)
        {
            action.Invoke(arg1);
        }
    }

    public static void SendEvent<T, U>(EventTable name, T arg1, U arg2)
    {
        Action<T, U> action = GetDelegate(name.ToString(), typeof(Action<T, U>)) as Action<T, U>;
        if (action != null)
        {
            action.Invoke(arg1, arg2);
        }
    }

    public static void SendEvent<T, U, V>(EventTable name, T arg1, U arg2, V arg3)
    {
        Action<T, U, V> action = GetDelegate(name.ToString(), typeof(Action<T, U, V>)) as Action<T, U, V>;
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3);
        }
    }

    public static void SendEvent<T, U, V, W>(EventTable name, T arg1, U arg2, V arg3, W arg4)
    {
        Action<T, U, V, W> action = GetDelegate(name.ToString(), typeof(Action<T, U, V, W>)) as Action<T, U, V, W>;
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3, arg4);
        }
    }

    public static void SendEvent<T, U, V, W, X>(EventTable name, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Action<T, U, V, W, X> action = GetDelegate(name.ToString(), typeof(Action<T, U, V, W, X>)) as Action<T, U, V, W, X>;
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3, arg4, arg5);
        }
    }

    #endregion
}
