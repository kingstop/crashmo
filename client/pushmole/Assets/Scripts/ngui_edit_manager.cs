﻿using UnityEngine;
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
    //public Button[] _Buttons_sliced;
    //public Button[] _Buttons_simple;
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
    public MainPanel _main_panel;
    public SaveMapPanel _SaveMapPanel;
    public SectionEditPanel _sectionEditPanel;
    public GameObject _login_obj;
    public GameObject _edit_obj_btns;
    public GameObject _edit_obj_draw_btns;
    public GameObject _edit_obj_create_btns;
    public GameObject _game_obj_btns;
	public GameObject _edit_type_obj_btns;
	public Image _map_image;
    public Camera _camera_map;
    public bool _input_keyboard;
    public Text _record_txt;
    public Text _account;
    public Text _password;
    public List<FradeText> _frade_texts = new List<FradeText>();
	//public FradeText _point_out;
    public UIGameEnd _game_end;
    public ColorBoard _color_board;
    public GameObject _point_text_position_obj;
    protected GameObject _source_frade;

	/// <summary>

	/// </summary>
    void Awake()
    {
        _source_frade = Resources.Load<GameObject>("prefab/point_out_txt");
        //_show_point_out = false;
        //_point_out.gameObject.SetActive (false);
        // Dictionary<int, Color> temp_dic = new Dictionary<int, Color>();
        _group_color.Add(0, new Color((float)120/255, (float)56/255, (float)56/255));

        _group_color.Add(1, new Color((float)170/255, (float)170/255, (float)255/255));
        _group_color.Add(2, new Color((float)246/255, (float)152/255, (float)152/255));
        _group_color.Add(3, new Color((float)154/255, (float)61/255, (float)154/255));

        _group_color.Add(4, new Color((float)90/255, (float)174/255, (float)174/255));
        _group_color.Add(5, new Color((float)255/255, (float)245/255, (float)71/255));
        _group_color.Add(6, new Color((float)180/255, (float)116/255, (float)116/255));

        _group_color.Add(7, new Color((float)219/255, (float)0/255, (float)0/255));
        _group_color.Add(8, new Color((float)5/255, (float)72/255, (float)246/255));
        _group_color.Add(9, new Color((float)0 / 255, (float)0 / 255, (float)0 / 255));
        _group_color.Add(10, new Color((float)255 / 255, (float)255 / 255, (float)255 / 255));
        _group_color.Add(11, new Color((float)255 / 255, (float)255 / 255, (float)255 / 255));

        int color_index = 0;
        foreach(KeyValuePair<int, Color> entry_pair in _group_color)
        {
            _color_board.SetButtonGroupColor(color_index,entry_pair.Key, entry_pair.Value);
            _color_board.SetCount(color_index, 0);
            color_index++;
        }
        _color_board.SetText(10, "目标");
        _color_board.SetText(11, "删除");
        global_instance.Instance._ngui_edit_manager = this;
        hide_edit_btn();
        hide_game_btns();
        show_create_btns();
        global_instance.Instance._net_client = new u3dclient();
        _input_keyboard = false;


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

	public void set_point_text(string txt)
	{
        FradeText frab = GameObject.Instantiate(_source_frade).GetComponent<FradeText>();
        frab.gameObject.transform.position = _point_text_position_obj.transform.position;
        frab.setText(txt);
        frab.setParent(this);
        frab.gameObject.transform.parent = _edit_obj_btns.gameObject.transform;
        _frade_texts.Add(frab);
	}

    public void setColorButtonText(int group, string txt)
    {
        _color_board.SetText(group, txt);
    }
    public void show_main_panel()
    {
        // _login_obj.SetActive(false);
        _main_panel.gameObject.SetActive(true);
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
		_color_board.gameObject.SetActive(show);
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
        string account = _account.text;
        string password = _account.text;
        global_instance.Instance._client_session.login(account, password);
    }

    public void on_register_btn_click()
    {
        string account = _account.text;
        string password = _account.text;
        //global_instance.Instance._net_client.register_account(account, password);
    }

    public void on_create_btn_click()
    {
        update_game_type(game_type.create);
    }

	public void set_save_map_panel_active(bool active)
	{
		_SaveMapPanel.gameObject.SetActive (active);
	}

	public void on_save_btn_click ()
	{
        set_edit_btns_active(false);
		set_edit_type_btns_active (false);
		set_save_map_panel_active (true);
	}

	public void set_edit_type_btns_active(bool active)
	{
		_edit_type_obj_btns.gameObject.SetActive (active);
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
    protected bool game_catch_action()
    {
        if (global_instance.Instance._crash_mole_grid_manager.get_game_type() == game_type.game
            && global_instance.Instance._crash_manager._record._open_record == false)
        {
            return true;
        }

        return false;
    }
    public void click_jump()
    {
        if (game_catch_action() && _input_keyboard == false)
        {
            global_instance.Instance._crash_manager.creature_jump();
        }
            
    }
    public void btn_down(Button entry)
    {
        if (game_catch_action() && _input_keyboard == false)
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

    }

    public void catch_btn_down(Button entry)
    {
        if (game_catch_action() && _input_keyboard == false)
        {
            int count = _catch.Length;
            int temp_ = 0;
            for (int i = 0; i < count; i++)
            {
                if (_catch[i] == entry)
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
    }
    public void btn_up(Button entry)
    {
        if(game_catch_action() && _input_keyboard == false)
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
		//message_on_button_click (_Buttons_simple[0]);			
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
                    UpdateKeyboard();
                    global_instance.Instance._crash_manager.update();
                }
                break;
        }
        global_instance.Instance._net_client.update();
	}

    void UpdateKeyboard()
    {
		if(game_catch_action() && _input_keyboard == true)
        {
            int length = _dir_btn_down.Length;
            for (int i = 0; i < length; i++)
            {
                _dir_btn_down[i] = false;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _dir_btn_down[(int)dir_move.back] = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _dir_btn_down[(int)dir_move.front] = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _dir_btn_down[(int)dir_move.left] = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _dir_btn_down[(int)dir_move.right] = true;
            }
            global_instance.Instance._crash_manager.update_dir_btn();
            bool jump_down = false;
            if (Input.GetKey(KeyCode.Space))
            {
                jump_down = true;
            }

            if (_jump_click == true && jump_down == false)
            {
                _jump_click = false;
            }
            else if (_jump_click == false && jump_down == true)
            {
                _jump_click = true;
                global_instance.Instance._crash_manager.creature_jump();
            }

            int b_ret = 0;
            if(Input.GetKey(KeyCode.H))
            {
                b_ret = 1;
            }
            global_instance.Instance._crash_manager.catch_click(b_ret);
        }
    }

    private bool _jump_click = false;
    public void BackToMainPanel()
    {
        HideAlluis();
        ClearGameObj();
        _main_panel.gameObject.SetActive(true);
    }

    public void HideAlluis()
    {
        _edit_obj_btns.SetActive(false);
        _edit_obj_create_btns.SetActive(false);
        _edit_obj_draw_btns.SetActive(false);
        _game_obj_btns.SetActive(false);
        _edit_type_obj_btns.SetActive(false);
        _game_end.gameObject.SetActive(false);
    }

    public void ClearGameObj()
    {
        global_instance.Instance._crash_mole_grid_manager.clear_edit_crash_mole_grid();
        global_instance.Instance._crash_manager.clear();
    }

    public int get_scrollbar_value(GameObject obj, int min, int max)
    {
        Scrollbar bar_temp = obj.GetComponent<Scrollbar>();
        float value = bar_temp.value;
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

    public void HideStateButton()
    {
        _Buttons_simple_game_type[(int)game_type.edit].gameObject.SetActive(false);
        _Buttons_sliced_game_type[(int)game_type.edit].gameObject.SetActive(false);
        _Buttons_simple_game_type[(int)game_type.game].gameObject.SetActive(false);
        _Buttons_sliced_game_type[(int)game_type.game].gameObject.SetActive(false);
    }
    public void gamestate_btn(game_type type)
    {
        HideStateButton();
        if (type != game_type.create) 
		{
			set_edit_type_btns_active(true);
		}

        if(global_instance.Instance._global_game_type == global_game_type.global_game_type_edit)
        {
            switch (type)
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
    }

    public void OnRecordClick()
    {
        if(global_instance.Instance._global_game_type == global_game_type.global_game_type_game && global_instance.Instance._crash_manager._game_begin == true)
        {
			crashmo_record_type current_type = global_instance.Instance._crash_manager._record.get_open_type ();
			if (current_type == crashmo_record_type.record_open || current_type == crashmo_record_type.record_ready_for_open) {
				global_instance.Instance._crash_manager._record.ready_for_record_closed ();
			} else {
				global_instance.Instance._crash_manager._record.ready_for_record_open ();	
			}

			if (global_instance.Instance._crash_manager._need_play_animation == false) 
			{
				if (global_instance.Instance._crash_manager._creature._is_in_falldown == false) 
				{
					if (global_instance.Instance._crash_manager.need_fall_update () == false) {
						global_instance.Instance._crash_manager._record.try_to_next_state ();
                        
					}

				}
			}

        }

    }

    public void update_game_type(game_type type)
    {
        _edit_obj_btns.SetActive(false);
        _game_obj_btns.SetActive(false);
        _game_end.gameObject.SetActive(false);
         gamestate_btn(type);
        ClearFradeText();
        //hide_game_btns();
        //hide_edit_btn();
        //hide_create_btns();
        switch (type)
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

                    show_edit_btn();
                }
                break;
            case game_type.game:
                {
                    _game_obj_btns.SetActive(true);
                    Vector3 vec = new Vector3(7.5f, 1.73f, -2.7f);
                    Vector3 vec_rot = new Vector3(11f, 0, 0);
                    Camera.main.transform.Rotate(vec_rot);
                    Camera.main.transform.position = vec;
                    Camera.main.fieldOfView = 34;
                    show_game_btns();

                    global_instance.Instance._crash_manager.init();
                }
                break;
        }
        if (global_instance.Instance._crash_mole_grid_manager != null)
        {
            global_instance.Instance._crash_mole_grid_manager.update_game_type(type);
        }
        
    }

    public void check_game_type_click(Button obj)
    {
        int length = _Buttons_sliced_game_type.Length;
        for (int i = 0; i < length; i++)
        {
            if (_Buttons_sliced_game_type[i] == obj)
            {
                game_type type = (game_type)i;
                if(type == game_type.game)
                {
                    global_instance.Instance.SetMapData(global_instance.Instance._crash_mole_grid_manager.save_crash_mole_grid());                    
                }
                update_game_type(type);
                break;
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

    }

    public void DestroyFrade(FradeText entry)
    {
        _frade_texts.Remove(entry);
        DestroyObject(entry.gameObject);
    }

    protected void ClearFradeText()
    {
        foreach(FradeText entry in _frade_texts)
        {
            DestroyObject(entry.gameObject);
        }
        _frade_texts.Clear();
    }

}
