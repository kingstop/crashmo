using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// See Interface IAloneDataManager . 
/// </summary>
/// <typeparam name="T"></typeparam>

public class AloneDataManager<T> : IAloneDataManager<T> where T : new()
{
    public event Action<T> OnDataUpdate;

    static IAloneDataManager<T> mInstance;

    public static IAloneDataManager<T> Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new AloneDataManager<T>();
            return mInstance;
        }
    }

    T mData;

    public virtual T Data
    {
        get
        {
            return this.mData;
        }
        set
        {
            this.mData = value;
            if (this.OnDataUpdate != null)
            {
                this.OnDataUpdate.Invoke(this.mData);           //  抛出数据更新事件，相应的UI监听该事件后自动刷新界面显示给玩家。
            }
        }
    }

    public virtual void InitFromLocal()
    {
        string file = typeof(T).Name;
        string content = Resources.Load<TextAsset>(file).text;
    }

#if EARLY_VERSION

    //  Use Set Property "Data" Instead : From line 33 to 40. 
    public virtual void OnReceiveData(T data)
    {
        this.Data = data;

        if (this.OnDataUpdate != null)
        {
            this.OnDataUpdate.Invoke(this.Data);
        }
    }

#endif
}
