using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FradeText : MonoBehaviour {
	public Text _title;
    protected ngui_edit_manager _parent;
    protected long _start_time;

	// Use this for initialization
	void Start () {
        _start_time = global_instance.Instance.getTime();
        Component[] comps = GetComponentsInChildren<Component>();  
		foreach (Component c in comps)  
		{  
			if (c is Graphic)  
			{  
				(c as Graphic).CrossFadeAlpha(0, 4, true);  
			}  

		}  
	//	FadeOpenMenu (this.gameObject);
	}

    public void setParent(ngui_edit_manager parent)
    {
        _parent = parent;
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
	
        long cur_time = global_instance.Instance.getTime();
        long spwan_time =  cur_time - _start_time;
        if(spwan_time > 5)
        {
            _parent.DestroyFrade(this);
        }
        else
        {
            Vector3 vc_position = this.gameObject.transform.position;
            vc_position.y += 0.15f;
            this.gameObject.transform.position = vc_position;
        }

    }



}
