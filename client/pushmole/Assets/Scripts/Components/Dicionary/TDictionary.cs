using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 三维字典
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
/// <typeparam name="V"></typeparam>
/// <typeparam name="W"></typeparam>

public class TDictionary<T, U, V, W>
{
	Dictionary<T, DDictionary<U, V, W>> mTDic = new Dictionary<T, DDictionary<U, V, W>> ();
	DDictionary<U, V, W> mDDic;

	public DDictionary<U,V,W> this [T t]
	{
		get
		{
			if (!mTDic.ContainsKey (t))
				mTDic [t] = new DDictionary<U, V, W> ();
			
			return mTDic [t];
		}
	}
	//
	//	public W this [T t, U u, V v]
	//	{
	//		get
	//		{
	//			if (mTDic.ContainsKey (t))
	//			{
	//				mDDic = mTDic [t];
	//
	//				if(mDDic.ContainsKey(u,v))
	//					return mDDic[u][v] ;
	//			}
	//
	//			return default(W);
	//		}
	//		set
	//		{
	//			if (!mTDic.ContainsKey (t))
	//			{
	//				mTDic [t] = new DDictionary<U, V, W> ();
	//			}
	//			mDDic = mTDic [t];
	//			mDDic [u, v] = value;
	//		}
	//	}
	//
	public void Clear ()
	{
		mTDic.Clear ();
	}

	public bool Contains (T t, U u, V v)
	{
		if (!mTDic.ContainsKey (t))
			return false;

		if (!mTDic [t].ContainsKey (u, v))
			return false;

		return true;
	}

}
