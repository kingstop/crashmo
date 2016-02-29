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
    // Use this for initialization
    public Text _map_name;
    public Text _section_text;
    public Text _number_text;
    public Text _txt_msg;

	void Start () {
	
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
                        mapdata.Section = int.Parse(_section_text.text);
                        mapdata.number = int.Parse(_number_text.text);
                    }
                    break;
            }

            msg.map = mapdata;
            global_instance.Instance._client_session.send(msg);
            ButtonEnable(false);
        }
    }


    public void SaveMapOk()
    {
        setActive(false);
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
