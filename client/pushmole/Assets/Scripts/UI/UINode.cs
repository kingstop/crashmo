using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// User interface node.
/// </summary>

public class UINode : LogicNode
{
	protected UIComponent uicomponent;

	public UINode()
	{
		UIComponent component = Create ();
		this.Init (component);
	}

	UIComponent Create ()
	{
		string name = this.GetType ().Name;
		GameObject ui = ResourceLoader.Create(name) ;
		ui.name = name;
		ui.transform.SetParent (UIRoot.mCanvas.transform, false);

		return ui.GetComponent<UIComponent> ();
	}

	public override void Init ()
	{
		base.Init ();
		//uicomponent.transform.SetParent (Game.Instance.mUIRoot.mCanvas.transform, false);
	}

	private void Init (UIComponent component)
	{
		this.uicomponent = component;
		this.uicomponent.Init ();
	}

	public override void Leave ()
	{
		base.Leave ();

		if (this.uicomponent != null)
		{
			GameObject.DestroyObject (uicomponent.gameObject);
			uicomponent = null;
		}
	}

	//	TODO:重构到基类中。
	public override void Release ()
	{
		base.Release ();

		if (this.uicomponent != null)
		{
			GameObject.DestroyObject (uicomponent.gameObject);
			uicomponent = null;
		}
	}

}
