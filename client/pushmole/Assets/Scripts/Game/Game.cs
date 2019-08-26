using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// No FrameWork . 
/// 逻辑决定渲染，而不是反之
/// 在这样的框架前提下，UI脚本控制或者战斗逻辑控制都被平等看待；都属于逻辑节点。
/// </summary>

public class Game : LogicTree
{
	public UIRoot mUIRoot;

	public DataManager mDataManager;
	public InputManager mInputManager;
	public CoroutineManager mCoroutineManager;
	public EventCenter mEventCenter;

	INode mBehaviorTree;

	private Game ()
	{
		Debug.LogWarning ("TODO:  所有加载方式改为异步StartCoroutine！");
		ResourceManager.Instance.Init ();

		Canvas canvas = GameObject.FindObjectOfType<Canvas> ();

		//	behavior tree . 
		SequenceNode gameSequence = new SequenceNode ();

		//gameSequence.AddNode (new DataProcess());
		gameSequence.AddNode (new GameStateLoading ());
		gameSequence.AddNode (new GameStateBattle ());

		mBehaviorTree = new SequenceNode ();
		mBehaviorTree.AddNode (gameSequence);

		//	logic tree . 

		AddNode (mDataManager = new DataManager ());
		mDataManager.Init ();			//		TODO:这里需要单独提前初始化，设计有问题，待解决。
		Debug.LogWarning ("TODO:这里需要单独提前初始化，设计有问题，待解决。");

		AddNode (mInputManager = new InputManager ());
		AddNode (mEventCenter = new EventCenter ());

		AddNode (mUIRoot = new UIRoot ());
		UIRoot.mCanvas = canvas;

		AddNode (new LogCallback ());
		AddNode (new UILog ());
		AddNode (mCoroutineManager = new CoroutineManager ());

		//	init global values . 
		//mUIRoot.mCanvas = GameObject.FindObjectOfType<Canvas> ();

		Debug.LogWarning ("TODO:ABTest！");
		ABManager.Instance.Init ();
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		if (this.mBehaviorTree != null)
			this.mBehaviorTree.Update (deltaTime);
	}

	/// TODO:优化事件机制
	#if ENABLE_TREE_EVENT
	public override void OnEvent (EventTable Event, object param)
	{
		this.mBehaviorTree.OnEvent (Event, param);
	}
	#endif

	public override void Leave ()
	{
		base.Leave ();
		mBehaviorTree.Leave ();
	}

	public override void Release ()
	{
		base.Release ();
		this.mBehaviorTree.Release ();
		this.mBehaviorTree = null;
		ResourceManager.Instance.Release ();
	}

	private static Game mInstance;
	private static readonly object mLock = new object ();

	public static Game Instance
	{
		get
		{
			if (mInstance == null)
			{
				lock (mLock)
				{
					if (mInstance == null)
						mInstance = new Game ();					
				}
			}

			return mInstance;
		}
	}
}
