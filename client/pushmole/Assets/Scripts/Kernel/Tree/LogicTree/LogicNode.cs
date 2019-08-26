using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LogicNode : Node
{

	// Name
	public string mName;


	// ID dictionary . 
	public Dictionary<int, LogicNode> mIDDic;


	// Name dictionary . 
	public Dictionary<string, LogicNode> mNameDic;


	public Dictionary<GameObject,LogicNode> mChildNodeGoDic;


	public GameObject mGameObject ;


	public override void Init ()
	{
		base.Init ();
	}

	public override void Enter ()
	{
		base.Enter ();

		if (Children != null)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children [i].Enter ();
			}
		}
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		if (Children != null)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children [i].Update (deltaTime);
			}
		}
	}

	public override void Leave ()
	{
		base.Leave ();

		if (Children != null)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children [i].Leave ();
			}
		}
	}

	public virtual void SetGameObject(GameObject go)
	{
		this.mGameObject = go;
	}

	public virtual void RegisterToParent(LogicNode node , GameObject sourceGo)
	{
		if (this.Parent != null && this.Parent is LogicNode)
		{
			(this.Parent as LogicNode).AddChildSource (node,sourceGo);
		}
	}


	public virtual void AddChildSource(LogicNode child , GameObject source)
	{
		if (this.mChildNodeGoDic == null)
		{
			this.mChildNodeGoDic = new Dictionary<GameObject, LogicNode> ();
		}
		this.mChildNodeGoDic [source] = child;

		//		一直注册，直到Root根节点
		this.RegisterToParent (child,source);
		DebugFormat.Log (this.ID,child.ID,source.name);
	}

	public override void Release ()
	{
		base.Release ();

		if (this.mIDDic != null)
			this.mIDDic.Clear ();
		if (this.mNameDic != null)
			this.mNameDic.Clear ();
	}
}
