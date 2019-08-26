using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class NetState : MonoBehaviour
{
	/// <summary>
	///  StateType , NetDelay . 
	/// </summary>
	public Action<NetworkReachability,int> OnNetState;

	Ping mPing;
	string mServerIp;
	float mPingTime;

	public void Start()
	{
		mServerIp = "123.125.114.144"; 
		mPing = new Ping (mServerIp);
	}

	void Update ()
	{
		if (this.mPingTime > 0)
		{
			this.mPingTime -= Time.deltaTime;
			return; 
		}

		this.mPingTime = 1;

		this.SendNetStatus (Application.internetReachability, mPing.time);

		switch (Application.internetReachability)
		{
		case NetworkReachability.NotReachable:		//	断网
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:		//	 移动网络
			break;
		case NetworkReachability.ReachableViaLocalAreaNetwork:		//	 通过局域网络可达
			break;			
		}
	}

	public void Release ()
	{
		//		base.Release ();

		mPing = null;
	}


	void SendNetStatus (NetworkReachability netStatus, int delay)
	{
		Debug.LogError (netStatus + "" + delay);
		if (this.OnNetState != null)
		{
			this.OnNetState (netStatus, delay);
		}
	}

}
