using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//		http://gad.qq.com/article/detail/17336%20target=
//		https://www.cnblogs.com/yaoh/p/5149568.html
//		http://blog.csdn.net/dingd_158/article/details/51852194

public class AnimationEditor : Editor
{

	public static void CreateAnimatorController ()
	{

		EditorLog.Init ();

		//		0读取配置数据；
		//		或者是：美术首次分割好之后，将相应的资源数据信息保存数据，以后美术每次提交之后，只需要点击菜单“扩展工具/生成角色动画”;动画事件也可以如此处理。

		//		1自动分割动画

		//		2自动生成AnimatorController 

		//		3生成上述操作的log文件

		Debug.LogWarning ("生成动画！");

		CutModelAnimation ();
		CreateAnimatorController ("Assets/Art/Animation/__001__Peas_Ani.controller");

		EditorLog.WritToFile (typeof(AnimationEditor).Name);
	}

	static void CreateAnimatorController (string destinationPath)
	{
		SimpleFile.CheckDirectory (destinationPath);

		//		create animator controller asset . 
		UnityEditor.Animations.AnimatorController ac = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (destinationPath);
		Debug.Log (ac.name);

		AddStateTranstation (ac.layers [0], "Assets/Art/model/__001__Peas_Ani.fbx");

		Selection.SetActiveObjectWithContext (ac, null);

		//UnityEngine.AnimatorControllerParameter param = new UnityEngine.AnimatorControllerParameter();
		//ac.layers[0]
		//ac.AddParameter ("Reset");
	}

	static void AddStateTranstation (UnityEditor.Animations.AnimatorControllerLayer layer, string path)
	{
		AddStateTranstation (layer.stateMachine, path);
	}

	//		切割动画文件. 获取模型中的动画文件。 https://docs.unity3d.com/ScriptReference/ModelImporter.html
	static void CutModelAnimation ()
	{
		AnimatorInfo config = new AnimatorInfo ();

		SimpleFile.Write (Application.dataPath + "/Resources/Data/animation.json", JsonUtility.ToJson (config, true));

		ModelImporter mi = ModelImporter.GetAtPath (config.mModelName) as ModelImporter;

		Debug.LogWarning ("一定要调用AssetDatabase.ImportAsset 重新导入资源；否则这里对资源的更改，对工程视图下的文件不会生效!");
		if (!mi.importAnimation)
		{
			mi.importAnimation = true;
			AssetDatabase.ImportAsset (config.mModelName);
		}

		ModelImporterClipAnimation[] animations = mi.clipAnimations;

		TakeInfo[] takeInfos = mi.importedTakeInfos;
		DebugFormat.LogWarning (config.mModelName, takeInfos.Length);

		if (takeInfos == null || takeInfos.Length == 0)
			return;

		DebugFormat.Log (takeInfos [0].name, takeInfos [0].defaultClipName, takeInfos [0].startTime, takeInfos [0].stopTime);

		//		设置动画类型
		mi.animationType = ModelImporterAnimationType.Generic;
		mi.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;

		ModelImporterClipAnimation[] cuttedAnimations = new ModelImporterClipAnimation[config.mCilps.Count];
		AnimationClipInfo d;

		for (int i = 0; i < cuttedAnimations.Length; i++)
		{
			d = config.mCilps [i];
			cuttedAnimations [i] = CutAnimationClip (d.ClipName, d.StartFrame, d.EndFrame, d.Loop);
		}

		mi.clipAnimations = cuttedAnimations;

		//		一定要调用AssetDatabase.ImportAsset 重新导入资源，否则这里对资源的更改，对工程视图下的文件不会生效。
		Debug.LogWarning ("一定要调用AssetDatabase.ImportAsset 重新导入资源；否则这里对资源的更改，对工程视图下的文件不会生效!");
		AssetDatabase.ImportAsset (config.mModelName);

		AssetDatabase.Refresh ();
	}


	//		分割动画文件
	static ModelImporterClipAnimation CutAnimationClip (string name, int startFrame, int endFrame, bool loop)
	{
		ModelImporterClipAnimation clip = new ModelImporterClipAnimation ();
		clip.name = name;
		clip.firstFrame = startFrame;
		clip.lastFrame = endFrame;
		clip.loop = loop;
		clip.wrapMode = clip.loop ? WrapMode.Loop : WrapMode.Default;
		return clip;
	}



	//		为动画控制器添加动画状态
	static void AddStateTranstation (UnityEditor.Animations.AnimatorStateMachine sm, string path)
	{
		Object[] assets = AssetDatabase.LoadAllAssetsAtPath (path);

		Debug.LogWarning (assets.Length);

		foreach (Object asset in assets)
		{
			if (!(asset is AnimationClip))
			{
				continue; 
			}
			if (asset.name.StartsWith ("__"))
			{
				continue;
			}

			SetUpAnimatorState (sm, asset as AnimationClip);
		}
	}

	static void SetUpAnimatorState (UnityEditor.Animations.AnimatorStateMachine sm, AnimationClip clip, string stateName = "Empty")
	{
		Debug.Log (string.Format ("AddStateTranstation：{0}", clip == null ? stateName : clip.name));

		UnityEditor.Animations.AnimatorState state = sm.AddState (clip == null ? stateName : clip.name);
		state.motion = clip;
		UnityEditor.Animations.AnimatorStateTransition transition = sm.AddAnyStateTransition (state);
		transition.duration = 0;
	}
}


