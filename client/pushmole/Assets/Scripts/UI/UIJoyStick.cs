using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 虚拟摇杆。
/// </summary>


public class UIJoyStick : UINode
{
	UIJoyStickComponent mJoyStickComponent ;

	public override void Enter ()
	{
		base.Enter ();

		mJoyStickComponent = uicomponent.GetComponentInChildren<UIJoyStickComponent> ();
		mJoyStickComponent.OnTouch += this.OnDrag;
	}

	void OnDrag(Vector2 delta)
	{
		//Parent.OnEvent (EventTable.UserInputDrag,delta);
		AloneEventCenter<UserInputEvent>.Instance.OnEvent( new UserInputEvent( UserInputEvent.UserInputCommand.Drag,delta,0)) ;
	}

	public override void Leave ()
	{
		base.Leave ();
		mJoyStickComponent.OnTouch -= this.OnDrag;
	}

	public override void Release ()
	{
		mJoyStickComponent.OnTouch -= this.OnDrag;
		base.Release ();
	}

}
