using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene test.
/// </summary>

public class SceneTest : BehaviorNode
{

	public SceneTest()
	{
		SequenceNode root = new SequenceNode ();
		SceneLoader battleSceneLoader = new SceneLoader (EventTable.GamePlay.ToString ());
		Battle battle = new Battle ();

		root.AddNode (battleSceneLoader);
		root.AddNode (battle);

		this.AddNode (root);
	}

}
