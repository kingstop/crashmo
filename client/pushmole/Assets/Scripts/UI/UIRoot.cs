using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class UIRoot : LogicTree
{
	public INode mRunningUI;
	public static Canvas mCanvas;

	public UIRoot()
	{
		mCanvas = GameObject.FindObjectOfType<Canvas> ();
	}

    public override void Init()
    {
        base.Init();
    }

	//	TODO:资源管理，资源名字映射到路径；资源id映射到路径。

	public Text CreateText(string content,Vector3 worldPosition)
	{
		GameObject textObj = ResourceLoader.Create ("UIText");
		Text text = textObj.GetComponent<Text> ();
		text.transform.SetParent (mCanvas.transform,false);
		return text;
	}


}
