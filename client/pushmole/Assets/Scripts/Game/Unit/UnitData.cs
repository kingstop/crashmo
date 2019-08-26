using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 	角色数据。
/// </summary>

public enum EUnitAttribute
{
	HP,
	Atk,
	Defense,
	Speed ,

	//	攻击范围
	AttackRange,
	//	防御距离
	DefenseRange,

	MAX,
}

public class UnitData
{
    public float[] mAttributes;

	public float this[EUnitAttribute attribute]
	{
		get 
		{
			float value = 0;
			return value;
		}
	}

    public void Init()
    {
        mAttributes = new float[(int)EUnitAttribute.MAX];
    }



}
