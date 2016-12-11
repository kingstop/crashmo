using UnityEngine;
using UnityEngine.UI;
//using System.Collections;
public enum SaveMapTitleType
{
    Admin,
    Customer
}
public class SaveMapPanel : MonoBehaviour {
    SaveMapTitleType _enMapType = SaveMapTitleType.Customer;
    public Button[] _btns;
    public SaveMapTitleButton[] _title_btns;
    public GameObject _admin_obj;
	public GameObject _input_chapter_obj;
	public GameObject _input_section_obj;
	public InputField _input_chapter;
	public InputField _input_section;
    // Use this for initialization
    public Text _map_name;
    public Text _chapter_text;
    public Text _section_text;
    public Text _txt_msg;

	void Start () {
	
		bool is_admin = false;

		if (global_instance.Instance._player.isadmin ()) 
		{
			is_admin = true;
		}

		foreach (Button entry in _btns) 
		{
			entry.gameObject.SetActive (is_admin);
		}

		if (is_admin == true && global_instance.Instance._ngui_edit_manager.getPageType() == page_type.page_type_official) 
		{
			_input_chapter.text = global_instance.Instance._ngui_edit_manager.GetChapterID ().ToString ();
			_input_section.text = global_instance.Instance._ngui_edit_manager.getSection ().ToString ();
		}
			
	}
	
	// Update is called once per frame
	void Update () {	
	}
    
    void UpdateTitleType(SaveMapTitleType type)
    {
        int length = _title_btns.Length;
        for(int i = 0; i < length; i ++)
        {
            if(i == (int)type)
            {
                _title_btns[i].select();
            }
            else
            {
                _title_btns[i].unselect();
            }
        }

        switch (type)
        {
            case SaveMapTitleType.Admin:
                {
                    _input_section_obj.SetActive(true);
                    _input_chapter_obj.SetActive(true);
                }
                break;
            case SaveMapTitleType.Customer:
                {
                    _input_section_obj.SetActive(false);
					_input_chapter_obj.SetActive(false);
                }
                break;
        }
    }

    public void OnTitleButtonClick(GameObject click)
    {
        SaveMapTitleType type = new SaveMapTitleType();
        int length = _title_btns.Length;
        for (int i = 0; i < length; i++)
        {
            if(_title_btns[i].gameObject == click)
            {
                type = (SaveMapTitleType)i;                
            }                
        }
        UpdateTitleType(type);
        _enMapType = type;
    }

    void ButtonEnable(bool b)
    {
        foreach(Button entry in _btns)
        {
            entry.enabled = b;
        }
    }

    void OnEnable()
    {
        if(global_instance.Instance._player.isadmin() == true)        
        {
            _admin_obj.SetActive(true);
            UpdateTitleType(_enMapType);
        }
        else
        {
            _admin_obj.SetActive(false);
        }

        ButtonEnable(true);
        global_instance.Instance._can_set_group = false;
    }
    

    void OnDisable()
    {
        global_instance.Instance._can_set_group = true;
    }

    public void onOKClick()
    {
        MapData temp_data = global_instance.Instance._crash_mole_grid_manager.save_crash_mole_grid();
        message.CrashMapData mapdata = temp_data.get_info();
        message.CrashPlayerInfo msginfo = global_instance.Instance._player.GetInfo();
        bool have_map_name = false;
        foreach(message.CrashMapData entry in msginfo.CompleteMap)
        {           
            if(entry.MapName == getMapName())
            {
                have_map_name = true;
                break;
            }            
        }

        if(have_map_name == false)
        {
            foreach (message.CrashMapData entry in msginfo.IncompleteMap)
            {
                if (entry.MapName == getMapName())
                {
                    have_map_name = true;
                    break;
                }
            }
        }

        if(have_map_name == true)
        {
            _txt_msg.text = "have same map name";
        }
        else
        {
            message.MsgSaveMapReq msg = new message.MsgSaveMapReq();
            
            switch (_enMapType)
            {
                case SaveMapTitleType.Customer:
                    {
                        msg.save_type = message.MapType.CompleteMap;
                    }
                    break;
                case SaveMapTitleType.Admin:
                    {
                        msg.save_type = message.MapType.OfficeMap;
                        mapdata.Section = int.Parse(_chapter_text.text);
						mapdata.number = int.Parse(_section_text.text);
                    }
                    break;
            }

            msg.map = mapdata;
            msg.map.MapName = getMapName();
            global_instance.Instance._client_session.send(msg);
            ButtonEnable(false);
        }
    }


    public void SaveMapOk()
    {
        setActive(false);
        global_instance.Instance._ngui_edit_manager.ClearGameObj();
        global_instance.Instance._ngui_edit_manager.show_main_panel();
    }

    public void onCancelClick()
    {
        setActive(false);
		global_instance.Instance._ngui_edit_manager.set_edit_type_btns_active (true);
        global_instance.Instance._ngui_edit_manager.set_edit_btns_active(true);
    }

    public string getMapName()
    {
        return _map_name.text;
    }

    public void setActive(bool b)
    {
        this.gameObject.SetActive(b);
    }
}
