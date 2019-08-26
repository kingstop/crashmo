using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//			http://kou-yeung.hatenablog.com/entry/2015/12/31/014611


[System.Serializable]
public struct AnimationClipInfo
{
	public string ModelName;
	public string ClipName;
	public int StartFrame;
	public int EndFrame;
	public bool Loop;

	public AnimationClipInfo (string clipName, int startFrame, int endFrame, bool loop, string modelName = "")
	{
		this.ClipName = clipName;
		this.StartFrame = startFrame;
		this.EndFrame = endFrame;
		this.Loop = loop;
		this.ModelName = modelName;
	}
}


[System.Serializable]
public class AnimatorInfo
{
	public string mAnimatorName;
	public string mModelName;
	public List<AnimationClipInfo> mCilps;

	public AnimatorInfo ()
	{
		this.mModelName = "Assets/Art/model/__001__Peas_Ani.fbx";
		this.mCilps = new List<AnimationClipInfo> ();

		//		test data . 
		this.mCilps.Add (new AnimationClipInfo ("idle", 100, 145, true));
		this.mCilps.Add (new AnimationClipInfo ("attack", 77, 91, true));

	}

}



[System.Serializable]
public class AnimatorList
{
	public List<AnimatorInfo> List;

	public AnimatorList ()
	{
		List = new List<AnimatorInfo> ();

	}
}

