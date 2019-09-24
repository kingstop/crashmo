using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameBtnsCtrl : MonoBehaviour {
	
	public Button[] _dir_btns;
	public Button[] _catch_btns;
	public bool[] _dir_button_down;
	public bool _input_keyboard;
    public GameObject _obj_map_big;
    public GameObject _obj_map_small;
    public long _last_mouse_down_time;
    public long _last_mouse_drag_time;
    public long _drag_begin_time;
    public float _drag_offset_x;
    public Vector3 _last_mouse_down_pos;
    public bool _joystick_touching = false;
    public ETCJoystick _joystick;
    public float _joystick_max_x  = 300;
    public float _joystick_max_y =  200;
    // Use this for initialization
    void Start () {
        _joystick_max_x = 300;
        _joystick_max_y = 200;
        ButtonState(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

	protected bool game_catch_action()
	{
        if(global_instance.Instance._crash_mole_grid_manager != null)
        {
            if (global_instance.Instance._crash_mole_grid_manager.get_game_type() == game_type.game
                && global_instance.Instance._crash_manager._record._open_record == false)
            {
                return true;
            }
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
			int length = _dir_btns.Length;
			for(int i = 0; i < length; i ++)
			{
				if(_dir_btns[i] == entry)
				{
					_dir_button_down[i] = true;                
				}
				else
				{
					_dir_button_down[i] = false;                
				}       
			}
			global_instance.Instance._crash_manager.update_dir_btn();
		}

	}

	public void catch_btn_down(Button entry)
	{
		if (game_catch_action() && _input_keyboard == false)
		{
			int count = _catch_btns.Length;
			int temp_ = 0;
			for (int i = 0; i < count; i++)
			{
				if (_catch_btns[i] == entry)
				{
					temp_ = i;
				}
			}
			global_instance.Instance._crash_manager.catch_click(temp_);
		} 
	}

	public void btn_up(Button entry)
	{
		if(game_catch_action() && _input_keyboard == false)
		{
			int length = _dir_btns.Length;
			for (int i = 0; i < length; i++)
			{
				if (_dir_btns[i] == entry)
				{
					_dir_button_down[i] = false;
				}
			}
			global_instance.Instance._crash_manager.update_dir_btn();
		}
	}

	public void CatchButtonUpdate(bool freezen)
	{
		if (freezen == false) 
		{
			_catch_btns[0].gameObject.SetActive (false);
			_catch_btns[1].gameObject.SetActive (true);
		} 
		else 
		{
			_catch_btns[0].gameObject.SetActive (true);
			_catch_btns[1].gameObject.SetActive (false);
		}

	}
	private bool _jump_click = false;
    
	public void UpdateKeyboard()
	{
		if(game_catch_action() && _input_keyboard == true)
		{
			int length = _dir_button_down.Length;
			for (int i = 0; i < length; i++)
			{
				_dir_button_down[i] = false;
			}

			if (Input.GetKey(KeyCode.UpArrow))
			{
				_dir_button_down[(int)dir_move.back] = true;
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				_dir_button_down[(int)dir_move.front] = true;
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				_dir_button_down[(int)dir_move.left] = true;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				_dir_button_down[(int)dir_move.right] = true;
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

	public void OnRecordClick()
	{
		if(global_instance.Instance._global_game_type == global_game_type.global_game_type_game && global_instance.Instance._crash_manager.getGameState() == gameState.game_playing)
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

    public void OnGUI()
    {
        if(Event.current.type == EventType.MouseDown)
        {
            OnMouseDown();
        }
        else if(Event.current.type == EventType.MouseUp)
        {
            OnMouseUp();
        }

        if(Event.current.type == EventType.MouseDrag)
        {
            //if(Input.touches.Length == 1)
            {
                long cur_time = getTimeStamp();
                long diff = cur_time - _last_mouse_drag_time;

                if (diff > 3 )
                {
                    _last_mouse_drag_time = getTimeStamp();
                    _last_mouse_down_pos = Input.mousePosition;
                    _drag_begin_time = _last_mouse_drag_time;
                    _drag_offset_x = 0;

                }
                else
                {
                    Vector3 cur_pos = Input.mousePosition;
                    _drag_offset_x += (cur_pos.x - _last_mouse_down_pos.x);
                    _last_mouse_drag_time = getTimeStamp();
                }
            }
            
        }
    }

    public void OnJoystickMove(Vector2 vec)
    {
        if(_joystick_touching)
        {
            int length = _dir_button_down.Length;
            for (int i = 0; i < length; i++)
            {
                _dir_button_down[i] = false;
            }

            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                if (vec.x > 0)
                {
                    _dir_button_down[(int)dir_move.right] = true;
                }
                else
                {
                    _dir_button_down[(int)dir_move.left] = true;
                }
            }
            else
            {
                if (vec.y > 0)
                {
                    _dir_button_down[(int)dir_move.back] = true;
                }
                else
                {
                    _dir_button_down[(int)dir_move.front] = true;
                }
            }
            global_instance.Instance._crash_manager.update_dir_btn();
        }


    }

    public void OnTouchStart()
    {
        _joystick_touching = true;
    }

    public void OnTouchEnd()
    {
        _joystick_touching = false;
        int length = _dir_button_down.Length;
        for (int i = 0; i < length; i++)
        {
            _dir_button_down[i] = false;
        }
        global_instance.Instance._crash_manager.update_dir_btn();
    }

    public void OnJoystickMoveEnd()
    {
        _joystick.gameObject.SetActive(false);
        //int length = _dir_button_down.Length;
        //for (int i = 0; i < length; i++)
        //{
        //    _dir_button_down[i] = false;
        //}
        //global_instance.Instance._crash_manager.update_dir_btn();
        //_joystick.gameObject.SetActive(false);
    }

    public long getTimeStamp()
    {
        long cur_time =(System.DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        return cur_time;
    }
    public void OnMouseDown()
    {
        if(Input.mousePosition.x <= _joystick_max_x && Input.mousePosition.y <= _joystick_max_y)
        {
            _joystick.gameObject.SetActive(true);
            _joystick.gameObject.transform.position = Input.mousePosition;
        }    
        //_last_mouse_down_time = getTimeStamp();
        //_last_mouse_down_pos = Input.mousePosition;
        //Debug.Log("mouse down position[" + _last_mouse_down_pos + "] time [ " + _last_mouse_down_time + "]");
    }
    public void OnMouseUp()
    {
       // _joystick.gameObject.SetActive(false);
        
        if (Input.touches.Length == 0)
        {
            //long time_cur = getTimeStamp();
            //long offset_time = time_cur - _drag_begin_time;
            //if (offset_time < 3)
            //{
            //    if (_drag_offset_x > 100)
            //    {
            //        global_instance.Instance._crash_manager.move_camera_left();
            //    }
            //    else if (_drag_offset_x < -100)
            //    {
            //        global_instance.Instance._crash_manager.move_camera_right();
            //    }
            //}
            //_last_mouse_drag_time = 0;
            //_drag_begin_time = 0;
        }

        Debug.Log("mouse up position[" + Input.mousePosition + "] time [ " + _last_mouse_down_time + "]");
    }
    public void OnBigMapClick()
	{
        ButtonState(true);
    }

    public void ButtonState(bool big)
    {
        _joystick_touching = false;
        _obj_map_big.SetActive(big);
        _obj_map_small.SetActive(!big);
        global_instance.Instance._ngui_edit_manager._camera_map.gameObject.SetActive(!big);
        global_instance.Instance._ngui_edit_manager._camera_map_big.gameObject.SetActive(big);
        //global_instance.Instance._ngui_edit_manager._camera_map.gameObject.SetActive(false);
        //global_instance.Instance._ngui_edit_manager._camera_map_big.gameObject.SetActive(false);
        _joystick.gameObject.SetActive(false);

    }


    public void OnSmallMapClick()
	{
        ButtonState(false);
    }
    public void on_button_click_camera_left()
	{
		global_instance.Instance._crash_manager.move_camera_left();
	}

	public void on_button_click_camera_right()
	{
		global_instance.Instance._crash_manager.move_camera_right();
	}

	public void BackToMainPanel()
	{
		global_instance.Instance._ngui_edit_manager.BackToMainPanel ();
	}

}
