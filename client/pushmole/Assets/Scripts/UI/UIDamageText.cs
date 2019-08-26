using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// User interface text animation.
/// </summary>

public class UIDamageText : UINode
{
	Text mText;
	Transform mTarget;
	Vector3 mPosition;
	float mSpeed;

	float mLifeTime;
	float mTime;

	public UIDamageText (float damage, Transform target, float lifeTime, float speed) : base ()
	{
		//Text text = Game.Instance.mUIRoot.CreateText (((int)damage).ToString (), target.position);
		this.mText = uicomponent.GetText("UIDamageText");
		this.mText.text = damage.ToString ();
		this.mTarget = target;
		this.mLifeTime = lifeTime;
		this.mSpeed = speed;

		this.OnSourcePosition ();
		Game.Instance.mUIRoot.AddNode (this);
	}

	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		mTime += deltaTime;
		if (mTime < mLifeTime)
		{
			this.OnSourcePosition ();
		}
		else
		{
			Parent.RemoveNode (this);
			this.Release ();
		}
	}

	public override void Release ()
	{
		GameObject.DestroyObject (this.mText.gameObject);
		base.Release ();
	}

	void OnSourcePosition()
	{
		if (mTarget == null)
			return;

		Vector2 uipos = Coord.WorldToUGUI (mTarget.transform.position, UIRoot.mCanvas);
		this.mText.rectTransform.anchoredPosition = uipos + new Vector2 (0, 1) * this.mSpeed * mTime + new Vector2(50,10) ;
	}
}
