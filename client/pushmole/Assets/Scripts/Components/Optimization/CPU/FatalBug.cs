using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 严重Bug
/// </summary>

public class FatalBug : CPU
{
	List<int> mOriginals;

	void Init ()
	{
		mOriginals = new List<int> ();
		mOriginals.Add (1);
	}

	void OnUpdate ()
	{
		List<int> temp = this.mOriginals;
		temp.Add (2);
		//  这里的temp和this.mOriginals指向同一内存，会导致temp中添加数据的时候，对原数据造成影响。导致this.mOriginals占用的空间急剧上升。

		foreach (int i in temp)
		{
			// do something i . 
		}
	}

	public override void Presentation ()
	{
		//  SceneItem 10 比20 要消耗。
		//  火系陷阱场景物件消耗特别大。
		//  遇到大量的火系地块陷阱的时候，帧数会一直周期性下降。
	}

	public override void Reason ()
	{
		//  原因是RemoveAura的时候，不断在增加List的数据。
	}

	//public override void Effect()
	//{
	//    base.Effect();
	//    this.Remove();
	//}

	//private void Remove()
	//{
	//List<float> removeList = mSpellEffectInfo.mSpellEffectAppendParam;
	//removeList.Add(mSpellEffectInfo.mSpellEffectAppendType);
	//removeList.Add(mSpellEffectInfo.mSpellEffectParam);

	//List<float> removeList = new List<float>();

	//    removeList.Add(this.mSpellEffectInfo.mSpellEffectParam);
	//    removeList.Add(this.mSpellEffectInfo.mSpellEffectAppendType);
	//    removeList.AddRange(this.mSpellEffectInfo.mSpellEffectAppendParam);

	//    foreach (int removeID in removeList)
	//    {
	//        if (removeID == 0)
	//            continue;

	//        foreach (BaseObject target in mTargets)
	//        {
	//            Game.Instance.War.RemoveAura(target, removeID);
	//        }
	//    }
	//}
}
