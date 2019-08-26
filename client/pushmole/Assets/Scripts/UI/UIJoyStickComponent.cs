using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System ;

/// <summary>
/// User interface joy stick.
/// </summary>

public class UIJoyStickComponent : ScrollRect
{
	bool mTouching = false ;
	public Action<Vector2> OnTouch ;

	float mRadius = 0;

	protected override void Awake ()
	{
		base.Awake ();

		mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
	}

	public override void OnBeginDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnBeginDrag (eventData);
		this.mTouching = true;
	}

	public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnDrag (eventData);

		Vector2 contentPostion = this.content.anchoredPosition;
		if (contentPostion.magnitude > mRadius)
		{
			contentPostion = contentPostion.normalized * mRadius;
			SetContentAnchoredPosition (contentPostion);
		}
	}

	public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnEndDrag (eventData);
		SetContentAnchoredPosition(Vector3.zero) ;

		this.mTouching = false;
	}

	void SendEvent(Vector2 contentOffset)
	{
		if (OnTouch != null)
		{
			OnTouch(contentOffset) ;
		}
	}

	void FixedUpdate()
	{
		if (this.mTouching)
		{
			SendEvent (this.content.anchoredPosition);
		}
	}

}
