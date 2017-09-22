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
public enum EditType
{
    EditNo,
    EditPlayer,
    EditOfficil
}

public class ngui_edit_manager : MonoBehaviour {

    public Button[] _Buttons_sliced_game_type;
    public Button[] _Buttons_simple_game_type;
    public Color _current_color;
    //public Button[] _game_btns;
    //public Button[] _dir_btn;
    //public Button[] _catch;
    public Button _open_create_btn;
    public Button _create_ok_btn;
    //public bool[] _dir_btn_down = new bool[4];
    public GameObject _edit_scrollbar;

    public crashmolegrid _flag_grid;
    public MainPanel _main_panel;
    public SaveMapPanel _SaveMapPanel;
    public SectionEditPanel _sectionEditPanel;
    public GameObject _login_obj;
	public GameBtnsCtrl _game_btns;
    //public GameObject _game_obj_btns;
	public GameObject _edit_type_obj_btns;
	public Image _map_image;
    public Camera _camera_map;
    public Camera _camera_map_big;
    //public bool _input_keyboard;
    public Text _record_txt;
    public Text _account;
    public Text _password;

    public UIGameEnd _game_end;
    public GameObject _point_text_position_obj;
    public UISectionGiftGold _ui_section_gold;
	public TaskEditPanel _task_edit_panel;
	public UIEditMap EditMap_;
    protected EditType _EditType;

    void Awake()
    {
        _EditType = EditType.EditPlayer;
        global_instance.Instance._ngui_edit_manager = this;       
        global_instance.Instance._net_client = new u3dclient();
        //_input_keyboard = false;	
    }
    public void SaveOfficilMap()
    {
        global_instance.Instance._officilMapManager.SaveOfficilMap();
    }


	public int GetChapterID()
	{
		return _main_panel.GetChapterID();
	}
	public ulong getSection()
	{
		return _main_panel.getSection ();
	}



	public page_type getPageType()
	{
		return _main_panel.getPageType();
	}


    
    public void set_point_text(string txt)
	{
		EditMap_.set_point_text (txt);
	}

    public void setColorButtonText(int group, string txt)
    {
		EditMap_.setColorButtonText (group, txt);
    }
    public void show_main_panel()
    {
        _main_panel.gameObject.SetActive(true);
    }

    public void set_login_btns_active(bool b)
    {
        _login_obj.SetActive(b);
    }

    public void set_edit_btns_active(bool b)
    {
		EditMap_.gameObject.SetActive (b);
    }

    public void set_edit_draw_btns_active(bool b)
    {
		EditMap_.ShowEditBtns (b);
        
    }

    public void set_edit_create_btns_active(bool b)
    {
		EditMap_.ShowCreateBtns (b);
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

    public void game_win()
    {
        _game_end.clear();
        _game_end.gameObject.SetActive(true);
    }

    public void on_create_empty_map_click()
    {
		global_instance.Instance._crash_mole_grid_manager.set_max_width(EditMap_.GetWidth());
		global_instance.Instance._crash_mole_grid_manager.set_max_height(EditMap_.GetHeight());
        update_game_type(game_type.edit);
    }

    public void edit_map()
    {

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



	// Use this for initialization
	void Start () {        
        _login_obj.SetActive(true);
	}


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
					_game_btns.UpdateKeyboard();
                    global_instance.Instance._crash_manager.update();
                }
                break;
        }
        global_instance.Instance._net_client.update();
	}


    public void BackToMainPanel()
    {
		HideAllUI();
        ClearGameObj();
        _main_panel.gameObject.SetActive(true);
    }

    public void HideAllUI()
    {

		EditMap_.gameObject.SetActive (false);
		_game_btns.gameObject.SetActive (false);
        
        _edit_type_obj_btns.SetActive(false);
        _game_end.gameObject.SetActive(false);
		_sectionEditPanel.gameObject.SetActive (false);
    }


	public void HideAllUIBut(GameObject obj)
	{
		if (EditMap_.gameObject != obj) 
		{
			EditMap_.gameObject.SetActive (false);		
		}


		if (_game_btns.gameObject != obj) 
		{
			_game_btns.gameObject.SetActive (false);
		}


		if (_edit_type_obj_btns != obj) 
		{
			_edit_type_obj_btns.SetActive (false);
		}

		if (_game_end.gameObject != obj) 
		{
			_game_end.gameObject.SetActive (false);
		}

		if (_sectionEditPanel.gameObject != obj) 
		{
			_sectionEditPanel.gameObject.SetActive (false);
		}
	}

    public void ClearGameObj()
    {
        global_instance.Instance._crash_mole_grid_manager.clear_edit_crash_mole_grid();
        global_instance.Instance._crash_manager.clear();
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

 

    public void update_game_type(game_type type)
    {
        EditMap_.gameObject.SetActive(false);
		_game_btns.gameObject.SetActive (false);
        _game_end.gameObject.SetActive(false);
         gamestate_btn(type);
		EditMap_.ClearFradeText();
		_sectionEditPanel.gameObject.SetActive (false);

        switch (type)
        {
            case game_type.create:
                {
                    EditMap_.gameObject.SetActive(true);
					set_edit_create_btns_active (true);
					set_edit_draw_btns_active (false);

                }
                break;
            case game_type.edit:
                {
                    EditMap_.gameObject.SetActive(true);
					set_edit_create_btns_active (false);
					set_edit_draw_btns_active (true);

                }
                break;
            case game_type.game:
                {
					_game_btns.gameObject.SetActive (false);
                    Vector3 vec = new Vector3(7.5f, 1.73f, -2.7f);
                    Vector3 vec_rot = new Vector3(11f, 0, 0);
                    Camera.main.transform.Rotate(vec_rot);
                    Camera.main.transform.position = vec;
                    Camera.main.fieldOfView = 34;
                    _game_btns.gameObject.SetActive(true);

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
		

    public void on_buttion_click(GameObject obj)
    {

    }

}
