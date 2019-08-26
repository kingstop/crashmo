using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System ;
using UnityEngine.UI ;




public class UIMessageBox : UINode
{
	Text mTitle ;
	Text mContent ;
	Text mConfirmButtonContent ;
	Text mCancelButtonContent ;

	Button mConfirmButton ;
	Button mCancelButton ;

	System.Action<UIMessageBox> ConfirmCallback ;
	System.Action<UIMessageBox> CancelCallback;

	public UIMessageBox()
	{
		
	}

	public UIMessageBox(string title,string content,System.Action<UIMessageBox> confirm ,System.Action<UIMessageBox> cancel):this()
	{
		mTitle = uicomponent.GetText ("TextTitle");
		mContent = uicomponent.GetText ("TextContent");

		mConfirmButton = uicomponent.GetButton ("ButtonConfirm");
		mCancelButton = uicomponent.GetButton ("ButtonCancel");

		mTitle.text = title;
		mContent.text = content;

		this.ConfirmCallback = confirm;
		this.CancelCallback = cancel;

		mConfirmButton.onClick.AddListener (this.OnConfirmButtonClick);
		mCancelButton.onClick.AddListener (this.OnCancelButtonClick);
	}

	void OnConfirmButtonClick()
	{
		if (this.ConfirmCallback != null)
			this.ConfirmCallback (this);
	}

	void OnCancelButtonClick()
	{
		if (this.CancelCallback != null)
			this.CancelCallback (this);	
	}



}
