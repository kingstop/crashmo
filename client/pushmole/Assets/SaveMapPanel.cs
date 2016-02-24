using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

public class SaveMapPanel : MonoBehaviour {

    // Use this for initialization
    public Text _map_name;
    public Text _txt_msg;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
            global_instance.Instance._crash_mole_grid_manager.clear_edit_crash_mole_grid();
            msginfo.IncompleteMap.Add(mapdata);
            global_instance.Instance._player.SetInfo(msginfo);
            setActive(false);
            global_instance.Instance._ngui_edit_manager.show_main_panel();            
        }
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
