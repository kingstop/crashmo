using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using System.Text; 
using System.Linq;
using message;

class msg
{
    public System.IO.MemoryStream stream_ = null;
    public string name_ = null;
}
public enum map_def{
    map_def_min_width = 10,
    map_def_max_width = 40,
    map_def_min_height = 15,
    map_def_max_height = 60
}

public class ngui_edit_manager : MonoBehaviour {
    public Button[] _Buttons_sliced;
    public Button[] _Buttons_simple;
    public Button[] _Buttons_sliced_game_type;
    public Button[] _Buttons_simple_game_type;
    public Dictionary<int, Color> _group_color = new Dictionary<int,Color>();
    public Color _current_color;
    public Button[] _game_btns;
    public Button[] _dir_btn;
    public Button[] _catch;
    public Button _open_create_btn;
    public Button _create_ok_btn;
    public bool[] _dir_btn_down = new bool[4];
    public GameObject _edit_scrollbar;
    public Text _text_width;
    public Text _text_height;

    public GameObject _edit_scrollbar_width;
    public GameObject _edit_scrollbar_height;
    public GameObject _static_edit_width;
    public GameObject _static_edit_height;
    public crashmolegrid _flag_grid;
    public MainPanel _main_panle;
    public GameObject _login_obj;
    public GameObject _edit_obj_btns;
    public GameObject _edit_obj_draw_btns;
    public GameObject _edit_obj_create_btns;

    public GameObject _game_obj_btns;

    public Text _account;
    public Text _password;
    
    void Awake()
    {
        global_instance.Instance._ngui_edit_manager = this;
        int count_temp = _Buttons_simple.Length;
        for (int i = 0; i < count_temp; i++)
        {
            _group_color.Add(i, global_instance.Instance._current_color = _Buttons_simple[i].GetComponent<Image>().color);
        }
        hide_edit_btn();
        hide_game_btns();
        show_create_btns();
        global_instance.Instance._net_client = new u3dclient();

    }
    public Color get_color_by_group(int group)
    {
        Color color = new Color(0, 0, 0);
        if(_group_color.ContainsKey(group))
        {
            color = _group_color[group];
        }
        return color;
    }
    public void show_main_panel()
    {
       // _login_obj.SetActive(false);
        _main_panle.gameObject.SetActive(true);
    }

    public void set_login_btns_active(bool b)
    {
        _login_obj.SetActive(b);
    }

    public void set_edit_btns_active(bool b)
    {
        _edit_obj_btns.SetActive(b);
    }

    public void set_edit_draw_btns_active(bool b)
    {
        _edit_obj_draw_btns.SetActive(b);
    }

    public void set_edit_create_btns_active(bool b)
    {
        _edit_obj_create_btns.SetActive(b);
    }
    
    public void set_flag_grid(crashmolegrid grid)
    {
        if(_flag_grid != null)
        {
            if(grid != _flag_grid)
            {
                _flag_grid.set_group(11);
            }
            
        }
        _flag_grid = grid;
    }
    public void Log(string str)
    {
        Debug.Log(str);
    }

    void set_game_btns_state(bool show)
    {
        foreach (Button entry in _game_btns)
        {
            entry.gameObject.SetActive(show);
        }

        int length = _dir_btn_down.Length;
        for (int i = 0; i < length; i++)
        {
            _dir_btn_down[i] = show;
        }
    }

    void set_edit_btns_state(bool show)
    {
        if (_Buttons_simple.Length > 0 && _Buttons_sliced.Length > 0)
        {
            foreach (Button entry in _Buttons_simple)
            {
                //entry.hideFlags = HideFlags.HideAndDontSave;
                entry.gameObject.SetActive(show);
            }

            foreach (Button entry in _Buttons_sliced)
            {
                entry.gameObject.SetActive(show);
            }
        }
        _open_create_btn.gameObject.SetActive(show);
        _edit_scrollbar.SetActive(show);
    }

    void set_create_btns_state(bool show)
    {
        _text_width.gameObject.SetActive(show);
        _text_height.gameObject.SetActive(show);
        _edit_scrollbar_width.gameObject.SetActive(show);
        _edit_scrollbar_height.gameObject.SetActive(show);
        _static_edit_width.gameObject.SetActive(show);
        _static_edit_height.gameObject.SetActive(show);
        _create_ok_btn.gameObject.SetActive(show);
        if(show)
        {
            int min_width = (int)map_def.map_def_min_width;
            int min_height = (int)map_def.map_def_min_height;
            _text_width.text = min_width.ToString();
            _text_height.text = min_height.ToString();
        }
    }

    public void on_login_btn_click()
    {
       // Debug.LogError("on_login_btn_click");
        string account = _account.text;
        string password = _account.text;
        CrashmoClientInit msg = new CrashmoClientInit();
        CrashPlayerInfo playerinfo = new CrashPlayerInfo();
        global_instance.Instance._player.SetInfo(playerinfo);
        set_login_btns_active(false);
        global_instance.Instance._ngui_edit_manager.show_main_panel();
        //
        
    }

    public void on_register_btn_click()
    {
        string account = _account.text;
        string password = _account.text;
        global_instance.Instance._net_client.register_account(account, password);
    }

    public void on_create_btn_click()
    {
        update_game_type(game_type.create);
    }

	public void on_save_btn_click ()
	{
		message.CrashMapData mapdata = global_instance.Instance._crash_mole_grid_manager.save_crash_mole_grid ();
        message.CrashPlayerInfo msginfo = global_instance.Instance._player.GetInfo();
       
        msginfo.IncompleteMap.Add(mapdata);
        global_instance.Instance._player.SetInfo(msginfo);
        set_edit_btns_active(false);
        show_main_panel();

	}

    public void on_create_empty_map_click()
    {
        global_instance.Instance._crash_mole_grid_manager.set_max_width(int.Parse(_text_width.text));
        global_instance.Instance._crash_mole_grid_manager.set_max_height(int.Parse(_text_height.text));
        update_game_type(game_type.edit);
    }

    public void edit_map()
    {

    }
    void hide_game_btns()
    {
        set_game_btns_state(false);
    }

    public void show_create_btns()
    {
        set_create_btns_state(true);
    }

    public void hide_create_btns()
    {
        set_create_btns_state(false);
    }
    void show_game_btns()
    {
        set_game_btns_state(true);
    }

    public void click_jump()
    {
        global_instance.Instance._crash_manager.creature_jump();
    }
    public void btn_down(Button entry)
    {
        int length = _dir_btn.Length;
        for(int i = 0; i < length; i ++)
        {
            if(_dir_btn[i] == entry)
            {
                _dir_btn_down[i] = true;                
            }
            else
            {
                _dir_btn_down[i] = false;                
            }       
        }
        global_instance.Instance._crash_manager.update_dir_btn();
    }

    public void catch_btn_down(Button entry)
    {
        int count = _catch.Length;
        int temp_ = 0;
        for(int i = 0; i < count; i ++)
        {
            if(_catch[i] == entry)
            {
                temp_ = i;
                _catch[i].gameObject.SetActive(false);
            }
            else
            {
                _catch[i].gameObject.SetActive(true);
            }                      
        }
        global_instance.Instance._crash_manager.catch_click(temp_);        
    }
    public void btn_up(Button entry)
    {
        int length = _dir_btn.Length;
        for (int i = 0; i < length; i++)
        {
            if (_dir_btn[i] == entry)
            {
                _dir_btn_down[i] = false;
            }
        }
        global_instance.Instance._crash_manager.update_dir_btn();
    }

	// Use this for initialization
	void Start () {        
      //  update_game_type(game_type.create);
        _login_obj.SetActive(true);
	}

    void hide_edit_btn()
    {
        set_edit_btns_state(false);
    }
    void show_edit_btn()
    {
        set_edit_btns_state(true);       
    }
	
	// Update is called once per frame
	void Update () {
        global_instance.Instance._client_session.update();
        if (global_instance.Instance._crash_mole_grid_manager == null)
        {
            return;
        }
        switch(global_instance.Instance._crash_mole_grid_manager.get_game_type())
        {
            case game_type.edit:
                {

                }
                break;
            case game_type.game:
                {
                    global_instance.Instance._crash_manager.update();
                }
                break;
        }
        global_instance.Instance._net_client.update();
       
	
	}

    public int get_scrollbar_value(GameObject obj, int min, int max)
    {
        Scrollbar bar_temp = obj.GetComponent<Scrollbar>();
        float value = bar_temp.value;
        float min_ = (float)min;
        float max_ = (float)max;
        float temp_distance = (max - min) * value;
        float target_ = temp_distance + min;
        int target = max - (int)target_ + min;
        
        return target;
    }
    public void on_scrollbar_width_change(GameObject obj)
    {
        int number_width = get_scrollbar_value(obj, (int)map_def.map_def_min_width, (int)map_def.map_def_max_width);
        _text_width.text = number_width.ToString();              
    }

    public void on_scrollbar_height_change(GameObject obj)
    {
        int number_height = get_scrollbar_value(obj, (int)map_def.map_def_min_height, (int)map_def.map_def_max_height);
        _text_height.text = number_height.ToString();
    }

    public void change_to_edit_state()
    {
        hide_edit_btn();
        hide_game_btns();
        show_create_btns();
    }

    public void gamestate_btn(game_type type)
    {
        _Buttons_simple_game_type[(int)game_type.edit].gameObject.SetActive(false);
        _Buttons_sliced_game_type[(int)game_type.edit].gameObject.SetActive(false);
        _Buttons_simple_game_type[(int)game_type.game].gameObject.SetActive(false);
        _Buttons_sliced_game_type[(int)game_type.game].gameObject.SetActive(false);
        switch(type)
        {
            case game_type.create:
                {
                }
                break;
            case game_type.edit:
                {
                    _Buttons_simple_game_type[(int)game_type.edit].gameObject.SetActive(true);
                    _Buttons_sliced_game_type[(int)game_type.edit].gameObject.SetActive(false);

                    _Buttons_simple_game_type[(int)game_type.game].gameObject.SetActive(false);
                    _Buttons_sliced_game_type[(int)game_type.game].gameObject.SetActive(true);
                   
                }
                break;
            case game_type.game:
                {
                    _Buttons_simple_game_type[(int)game_type.edit].gameObject.SetActive(false);
                    _Buttons_sliced_game_type[(int)game_type.edit].gameObject.SetActive(true);

                    _Buttons_simple_game_type[(int)game_type.game].gameObject.SetActive(true);
                    _Buttons_sliced_game_type[(int)game_type.game].gameObject.SetActive(false);
                }
                break;
        }
    }

    public void update_game_type(game_type type, message.CrashMapData mapinfo = null)
    {
        _edit_obj_btns.SetActive(false);
        _game_obj_btns.SetActive(false);
       
        gamestate_btn(type);
        //hide_game_btns();
        //hide_edit_btn();
        //hide_create_btns();
        switch(type)
        {
            case game_type.create:
                {
                    _edit_obj_btns.SetActive(true);
                    _edit_obj_create_btns.SetActive(true);
                    _edit_obj_draw_btns.SetActive(false);
                }
                break;
            case game_type.edit:
                {
                    _edit_obj_btns.SetActive(true);
                    _edit_obj_create_btns.SetActive(false);
                    _edit_obj_draw_btns.SetActive(true);
                    Camera.main.fieldOfView = 60;
                    show_edit_btn();
                }
                break;
            case game_type.game:
                {
                    _game_obj_btns.SetActive(true);
                    Vector3 vec = new Vector3(8.59f, 3.37f, -13.64f);
                    Camera.main.transform.position = vec;
                    Camera.main.fieldOfView = 34;
                    show_game_btns();
                    global_instance.Instance._crash_manager._max_x = global_instance.Instance._crash_mole_grid_manager.get_max_width();
                    global_instance.Instance._crash_manager._max_y = global_instance.Instance._crash_mole_grid_manager.get_max_height() + 20;
                    global_instance.Instance._crash_manager._max_z = 18;
                    global_instance.Instance._crash_manager.init();
                }
                break;
        }
        if (global_instance.Instance._crash_mole_grid_manager != null)
        {
            global_instance.Instance._crash_mole_grid_manager.update_game_type(type,mapinfo);
        }
        
    }

    public void check_game_type_click(Button obj)
    {
        int length = _Buttons_sliced_game_type.Length;
        for (int i = 0; i < length; i++)
        {
            if (_Buttons_sliced_game_type[i] == obj)
            {
                update_game_type((game_type)i);
                break;
            }
            
        }
    }


    public void message_on_button_click(Button obj)
    {
        int count_temp = _Buttons_sliced.Length;
        bool b_set = true;
        if (obj.GetComponent<Image>().type == Image.Type.Simple)
        {
            for (int i = 0; i < count_temp; i++)
            {
                if (_Buttons_simple[i] == obj)
                {
                    global_instance.Instance._current_color = _Buttons_simple[i].GetComponent<Image>().color;
                    global_instance.Instance._current_group = i;
                    _Buttons_simple[i].gameObject.SetActive(!b_set);
                    _Buttons_sliced[i].gameObject.SetActive(b_set);

                }
                else
                {
                    _Buttons_simple[i].gameObject.SetActive(b_set);
                    _Buttons_sliced[i].gameObject.SetActive(!b_set);
                }
            }
        }

    }

    public void on_button_click_camera_left()
    {
        global_instance.Instance._crash_manager.move_camera_left();
    }

    public void on_button_click_camera_right()
    {
        global_instance.Instance._crash_manager.move_camera_right();
    }

    public void on_buttion_click(GameObject obj)
    {
        //ngui_edit_manager entry = this.GetComponentInParent<ngui_edit_manager>();
        //entry.message_on_button_click(obj);
        
        
        //foreach (Button entry in _Buttons)
        //{
        //    Image temp = entry.GetComponent<Image>();
        //    if (entry == obj)
        //    {
                
        //        temp.type = Image.Type.Sliced;
        //    }
        //    else
        //    {
        //        temp.type = Image.Type.Simple;
        //    }
        //}
    }


}
