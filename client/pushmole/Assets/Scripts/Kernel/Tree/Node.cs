using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : INode
{
	/// <summary>
	/// the unique id . 
	/// </summary>
	/// <value>The I.</value>
	public int ID{ get; set;}

	/// <summary>
	/// Parent of this node . 
	/// </summary>
	public INode Parent { get; set; }

	/// <summary>
	/// 运行状态
	/// </summary>
	public RunningStatus RunningStatus { get; set; }

	/// <summary>
	/// 运行节点
	/// </summary>
	public INode RunningNode { get; set; }

	/// <summary>
	/// 子节点
	/// </summary>
	/// <value>The children.</value>
	public List<INode> Children{ get; set; }

	private static int mAutoID = 0;

	/// <summary>
	/// Node new . 
	/// </summary>
	public Node ()
	{
		ID = ++mAutoID;
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public virtual void Init ()
	{
		Debug.Log (string.Format ("{0} Init!", this.GetType ().Name));
		if (this.Children != null)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children [i].Init ();
			}
		}
	}

	/// <summary>
	/// 添加节点
	/// </summary>
	/// <param name="node"></param>
	public virtual void AddNode (INode node)
	{
		if (node == null)
			return;
		if (this.Children == null)
			this.Children = new List<INode> ();

		if (!this.Children.Contains (node))
		{
			this.Children.Add (node);
			node.Parent = this;
		}
	}

	/// <summary>
	/// 移除节点
	/// </summary>
	/// <param name="node"></param>
	public virtual void RemoveNode (INode node)
	{
		if (this.Children == null || node == null)
			return;
		this.Children.Remove (node);
		node.Parent = null;
	}


	/// <summary>
	/// Enter . 
	/// </summary>
	public virtual void Enter ()
	{
		this.RunningStatus = RunningStatus.Running;
		//Debug.Log (string.Format ("【{0} 】Enter", this.GetType ().Name));
	}


	/// <summary>
	/// 更新
	/// </summary>
	/// <param name="deltaTime"></param>
	public virtual void Update (float deltaTime)
	{
	}

	/// <summary>
	/// Leave . 
	/// </summary>
	public virtual void Leave ()
	{
		this.RunningStatus = RunningStatus.Inactive;
		//Debug.Log (string.Format ("【{0}】Exit . {1}", this.GetType ().Name,this.ID));
	}

	#if ENABLE_TREE_EVENT
	/// <summary>
	/// 事件
	/// TODO:优化事件机制
	/// </summary>
	/// <param name="Event"></param>
	/// <param name="param"></param>
	public virtual void OnEvent (EventTable Event, object param)
	{
		if (Children != null)
		{
			for(int i=0;i<Children.Count;i++)
			{
				Children [i].OnEvent (Event,param);
			}
		}
	}
	#endif

	/// <summary>
	/// 释放
	/// </summary>
	public virtual void Release ()
	{
		if (this.Children != null)
		{
			for(int i=0;i<Children.Count;i++)
			{
				Children [i].Release ();
				Children [i] = null;
			}

			this.Children.Clear ();
			//			方便Mono内存的及时回收
			this.Children = null;
		}

		Debug.Log (string.Format ("{0} Release!", this.GetType ().Name));
	}
}
