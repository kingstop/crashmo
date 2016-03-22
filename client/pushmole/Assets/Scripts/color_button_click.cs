using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class color_button_click : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void on_color_button_click(Button obj)
    {
        ngui_edit_manager entry = this.GetComponentInParent<ngui_edit_manager>();
        entry.message_on_button_click(obj);
    }
}
