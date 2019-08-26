using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior Tree .  Should be renamed as Behavior Tree .  
/// </summary>

public class Tree : SelectorNode, ITree
{

	public override void Update (float deltaTime)
	{
		//Debug.LogError (this.ID+" " +Time.frameCount);

		base.Update (deltaTime);
	}

	#if ENABLE_TREE_EVENT
	//		TODO:行为树这里需要自动将消息传给所有子节点，但是逻辑树的事件却是按照逻辑自上向下发送，或者自下向上发送，这里如何统一？
	public override void OnEvent (EventTable Event, object param)
	{
		base.OnEvent (Event, param);
	}
	#endif

}
