using UnityEngine;
using UnityEngine.UI;


public class SectionEditPanel : MonoBehaviour {
    public Text _section;
    public Text _section_name;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onOkClick()
    {
        message.MsgModifySectionNameReq msg = new message.MsgModifySectionNameReq();
        msg.section = int.Parse(_section.text);
        msg.section_name = _section_name.text;
        global_instance.Instance._client_session.send(msg);
        setActive(false);
    }

    public void onCancelClick()
    {
        global_instance.Instance._ngui_edit_manager.show_main_panel();
        setActive(false);
    }

    public void setActive(bool b)
    {
        this.gameObject.SetActive(b);
    }

}
