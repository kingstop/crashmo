using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 二维字典
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
/// <typeparam name="V"></typeparam>

public class DDictionary<T, U, V>
{
	Dictionary<T, Dictionary<U, V>> mDDic = new Dictionary<T, Dictionary<U, V>> ();

	public Dictionary<U,V> this [T t]
	{
		get
		{
			if (!mDDic.ContainsKey (t))
				mDDic [t] = new Dictionary<U, V> ();
			
			return mDDic [t];
		}
		set
		{
			this.mDDic [t] = value;
		}
	}

	public void Clear ()
	{
		mDDic.Clear ();
	}

	public bool ContainsKey (T t, U u)
	{
		if (!mDDic.ContainsKey (t))
			return false;

		if (!mDDic [t].ContainsKey (u))
			return false;

		return true;
	}

}

