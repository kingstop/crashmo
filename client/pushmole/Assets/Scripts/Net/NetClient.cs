using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//		http://blog.csdn.net/linshuhe1/article/details/51386559



public class NetClient
{

	/// <summary>
	/// 收到服务器消息时，将修改相应的Data缓存，并由Data相应的管理器AloneDataManager抛出数据更新的消息。
	/// </summary>
	/// <param name="type"></param>

	public void OnProtocol (EProtocolType type)
	{
		switch (type)
		{
		case EProtocolType.TYPE_GetUserInfo:
			Game.Instance.mDataManager.UserInfo = new GameInfo ();         //  msg.Deserialize<UserInfo>() ;
			AloneDataManager<GameInfo>.Instance.Data = new GameInfo ();
			break;
		}
	}

	#if COMPLETE_PROTOCOL
	
		public void OnProtocol (Protocol p)
		{
			switch ((EProtocolType)p.GetMsgType ()) {
			case EProtocolType.TYPE_Behavior:
				DataManager.Instance.UserInfo = p.Deserialize<UserInfo> ();
				break;
			}
		}

		#endif

}
