using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 并行节点
/// 只要有一个失败，则失败，所有成功才算成功。
/// </summary>

public class ParallelNode : Node
{

	public override void Update (float deltaTime)
	{
		//  base.Update(deltaTime);

		int successCount = 0;
		int failCount = 0;

		for (int i = 0; i < Children.Count; i++) {
			Children [i].Update (deltaTime);

			switch (Children [i].RunningStatus) {
			case RunningStatus.Success:
				successCount++;
				Children [i].Leave ();
				break;
			case RunningStatus.Failure:
				failCount++;
				Children [i].Leave ();

				break;
			default:
				break;
			}
		}

		if (failCount > 0) {
			this.RunningStatus = RunningStatus.Failure;
		} else if (successCount == Children.Count) {
			this.RunningStatus = RunningStatus.Success;
		} else {
			this.RunningStatus = RunningStatus.Running;
		}
	}


	public override void Leave ()
	{
		if (Children != null) {
			for (int i = 0; i < Children.Count; i++) {
				if (Children [i].RunningStatus == RunningStatus.Running)
					Children [i].Leave ();
			}
		}
		base.Leave ();
	}


}
