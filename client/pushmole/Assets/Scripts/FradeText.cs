using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FradeText : MonoBehaviour {
	public Text _title;
	protected long _start_time;

	/*
	public static void FadeOpenMenu(GameObject targetGO)  
	{  
		Component[] comps = targetGO.GetComponentsInChildren<Component>();  
		for (int index = 0; index < comps.Length; index++)  
		{  
			Component c = comps[index];  
			if (c is Graphic)  
			{  
				//(c as Graphic).color = new Color(1, 1, 1, 0);  
				// (c as Graphic).CrossFadeAlpha(1f, MENU_FADE_IN_TIME, true);  
				(c as Graphic).CrossFadeAlpha  
					.DoFade(0f, MENU_FADE_IN_TIME)  
					.SetDelay(CAMERA_ZOOM_IN_DELAY)  
					.SetEase(MENU_SCALE_OPEN_TYPE)  
					.From()  
					.OnComplete(  
						() =>  
						{  
							MenuSignalManager.OpenedMenuSignal.Dispatch();  
						});  
			}  
		}  
		// 执行完毕的回调  
	}  
	/// <summary>  
	/// 渐隐菜单(无销毁操作)  
	/// </summary>  
	/// <param name="targetGO">菜单游戏对象</param>  
	public static void FadeCloseMenu(GameObject targetGO)  
	{  
		Component[] comps = targetGO.GetComponentsInChildren<Component>();  
		for (int index = 0; index < comps.Length; index++)  
		{  
			Component c = comps[index];  
			if (c is Graphic)  
			{  
				//(c as Graphic).CrossFadeAlpha(0, MENU_FADE_OUT_TIME, true);      // 当然了如果认为不方便的话，可以使用dotween的Graphic的DoColor、DoFade  
				(c as Graphic).  
				DoFade(0, MENU_FADE_OUT_TIME)  
					.SetEase(MENU_FADE_OUT_TYPE)  
					.OnComplete(() =>  
						{  
							MenuSignalManager.CloseedMenuSignal.Dispatch(targetGO);  
						});  
			}  
		}  
		// 执行完毕的回调  
	} 
	*/
	// Use this for initialization
	void Start () {	
		
		Component[] comps = GetComponentsInChildren<Component>();  
		foreach (Component c in comps)  
		{  
			if (c is Graphic)  
			{  
				(c as Graphic).CrossFadeAlpha(0, 6, true);  
			}  

		}  
	//	FadeOpenMenu (this.gameObject);
	}

	public void setText(string txt)
	{
		_title.text = txt;
	}

	public void FradeOut()
	{
		Component[] comps = GetComponentsInChildren<Component>();  
		foreach (Component c in comps)  
		{  
			if (c is Graphic)  
			{  
				(c as Graphic).CrossFadeAlpha(0, 2, true);  
			}  

		}  
	//	FadeCloseMenu (this.gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}



}
