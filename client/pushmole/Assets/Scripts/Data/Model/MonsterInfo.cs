using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct MonsterInfo
{
	public int ID;

	public string Name;

	public int HP;

	public int ATK;
}



[System.Serializable]
public class MonsterList
{
	public List<MonsterInfo> List;

	public MonsterList ()
	{
		List = new List<MonsterInfo> ();
	}

	public MonsterList (List<MonsterInfo> monsterDatas)
	{
		this.List = monsterDatas;
	}

}
