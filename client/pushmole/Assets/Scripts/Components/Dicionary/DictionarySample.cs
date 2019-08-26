using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DictionarySample : MonoBehaviour
{

	public void Test()
	{
		DDictionary<int,int,int> ddic = new DDictionary<int, int, int> ();
		ddic [1] [1] = 10;
		Debug.LogWarning ("二维字典：" + ddic [1] [1]);

		TDictionary<int,int,int,int> tDic = new TDictionary<int, int, int, int> ();
		tDic [1] [0] [1] = 101;
		Debug.LogWarning ("三维字典：" + tDic [1] [0] [1]);
	}


}
