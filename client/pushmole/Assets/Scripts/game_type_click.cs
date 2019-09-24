using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class game_type_click : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void on_game_type_click(Button obj)
    {
        ngui_edit_manager entry = this.GetComponentInParent<ngui_edit_manager>();
       // entry.check_game_type_click(obj);
    }
}
