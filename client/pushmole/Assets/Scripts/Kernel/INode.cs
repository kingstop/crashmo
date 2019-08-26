using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode : IEntity
{
	int ID{ get; set; }

	RunningStatus RunningStatus { get; set; }

	INode RunningNode { get; set; }

	INode Parent { get; set; }

	List<INode> Children{ get; set; }

	void AddNode (INode node);

	void RemoveNode (INode node);

	void Enter ();

	void Leave ();

	#if ENABLE_TREE_EVENT
	void OnEvent (EventTable Event, object param);
	#endif
}
