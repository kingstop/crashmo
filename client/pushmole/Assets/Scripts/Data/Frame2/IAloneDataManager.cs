using System.Collections;
using System;

public interface IAloneDataManager<T> where T : new()
{
    //  Cache data of this type . Property "Set" used for ServerResponse . When data from server changed , it will send the OnDataUpdate event . 
    T Data { get; set; }

    //  当收到服务器数据的时候 . 
    //void OnReceiveData(T data);

    //  本地静态数据的初始化 . 
    void InitFromLocal();

    //  通知相应的逻辑和界面更新。
    event Action<T> OnDataUpdate;

}
