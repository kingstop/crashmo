using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum crash_obj_type
{
    normal,
    player,
    flag,
    target
}
public enum game_type
{
    edit,
    game,
    create,
    max
}


public enum dir_move
{
    front,
    right,
    back,
    left,    
    down,
    up,
    no
}

public struct crash_pos
{
    public int _x;
    public int _y;
    public int _z;

    public void clone(crash_pos pos)
    {
        _x = pos._x;
        _y = pos._y;
        _z = pos._z;
    }
    public void move(dir_move dir)
    {
        switch (dir)
        {
            case dir_move.back:
                {
                    _z = _z + 1;
                }
                break;
            case dir_move.front:
                {
                    _z = _z - 1;
                }
                break;
            case dir_move.down:
                {
                    _y = _y - 1;
                }
                break;
            case dir_move.up:
                {
                    _y = _y + 1;
                }
                break;
            case dir_move.left:
                {
                    _x = _x - 1;
                }
                break;
            case dir_move.right:
                {
                    _x = _x + 1;
                }
                break;

        }
    }

}

public class crash_base_obj
{
    public crash_base_obj()
    {
        _pos._x = 0;
        _pos._y = 0;
        _pos._z = 0;
        _last_pos = _pos;
    }
    public crash_base_obj(int x, int y, int z)
    {
        _pos._x = x;
        _pos._y = y;
        _pos._z = z;
        _last_pos = _pos;
    }

    public virtual void set_position(float x, float y, float z)
    {

    }

    public crash_obj_type get_obj_type()
    {
        return _type;
    }

    public crash_pos _pos = new crash_pos();
    public crash_pos _last_pos = new crash_pos();
    public crash_mole _crash_mole;
    protected crash_obj_type _type;

}

public class crash_obj_flag : crash_base_obj
{
    public crash_obj_flag()
        :base()
    {
        _type = crash_obj_type.flag;
    }

    public crash_obj_flag(int x, int y, int z)
        : base(x, y, z)
    {
        _type = crash_obj_type.flag;
    }

    public override void set_position(float x, float y, float z)
    {
        if(_flag != null)
        {
            _flag.set_position(x, y, z);
        }
    }

    public crash_flag _flag;
}

public class crash_obj_creature : crash_base_obj
{
    public crash_obj_creature()
        : base()
    {
        _type = crash_obj_type.player;
    }
    public crash_obj_creature(int x, int y, int z)
        : base(x, y, z)
    {
        _type = crash_obj_type.player;
    }

    public override void set_position(float x, float y, float z)
    {
        if(_creature != null)
        {            

			_creature.set_position(x + 0.45f, y + 0.2f, z + 0.5f);
        }
    }
    public creature _creature;
}
public class crash_obj : crash_base_obj
{
    public crash_obj() : base()
    {
        _type = crash_obj_type.normal;

    }
    public crash_obj(int x, int y, int z) :base(x, y, z)
    {
        _type = crash_obj_type.normal;
    }

    public override void set_position(float x, float y, float z)
    {
        _grid.set_position(x, y, z);
    }
    public crashmolegrid _grid;
 

}


public class crash_mole
{
    public crash_mole()
    {

    }

    public void clear_addr()
    {
        foreach (crash_base_obj obj_entry in _crash_objs)
        {
            _crash_manager._crash_objs[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_obj = null;
            _crash_manager._crash_moles[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_mole = null;
        }
    }

    public void reset_addr()
    {
        foreach (crash_base_obj obj_entry in _crash_objs)
        {
            _crash_manager._crash_objs[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_obj = obj_entry;
            _crash_manager._crash_moles[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_mole = this;
        }
    }

    public bool remove_crash_obj(crash_base_obj obj_entry)
    {
        if(_crash_objs.Remove(obj_entry))
        {
            _crash_manager._crash_objs[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_obj = null;
            _crash_manager._crash_moles[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_mole = null;
            return true;
        }
        return false;
    }
    public bool add_crash_obj(crash_base_obj obj_entry)
    {
        foreach (crash_base_obj enry in _crash_objs)
        {
            if(obj_entry._pos._x == enry._pos._x &&
                obj_entry._pos._y == enry._pos._y &&
                obj_entry._pos._z == enry._pos._z)
            {
                return false;
            }
        }
        crash_obj_addr obj_addr = _crash_manager.get_crash_obj_addr(obj_entry._pos);
        if(obj_addr == null)
        {
            return false;
        }

        if (_crash_manager.get_crash_obj_addr(obj_entry._pos)._crash_obj != null)
        {
            return false;
        }

       // add_crash_obj(obj_entry);
        obj_entry._crash_mole = this;
       
        _crash_objs.Add(obj_entry);
        _crash_manager._crash_objs[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_obj = obj_entry;
        _crash_manager._crash_moles[obj_entry._pos._x, obj_entry._pos._z, obj_entry._pos._y]._crash_mole = this;

        return true;
    }

    public void update_side()
    {
        foreach (crash_base_obj entry in _crash_objs)
        {
            int temp_x = entry._pos._x;
            int temp_y = entry._pos._y;
            int temp_z = entry._pos._z;
            if(entry.get_obj_type() == crash_obj_type.normal)
            {
                crash_obj temp_obj = (crash_obj)entry;
                temp_obj._grid.hide_sides();
                if (is_self(temp_x - 1, temp_z, temp_y) == false)
                {
                    temp_obj._grid.show_side(side_type.side_left);

                }
                if (is_self(temp_x + 1, temp_z, temp_y) == false)
                {
                    temp_obj._grid.show_side(side_type.side_right);
                }

                if (is_self(temp_x, temp_z, temp_y + 1) == false)
                {
                    temp_obj._grid.show_side(side_type.side_top);
                }

                if (is_self(temp_x, temp_z, temp_y - 1) == false)
                {
                    temp_obj._grid.show_side(side_type.side_bottom);
                }
            }
        }
    }
    protected bool is_self(int x, int z, int y)
    {
		bool ret = false;
		if (x >= 0 && x <= (int)map_def.map_def_max_width && y >= 0 && y <= (int)map_def.map_def_max_height) {
			if (_crash_manager._crash_moles [x, z, y]._crash_mole == this) {
				ret = true;
			}

		}
		else {
			ret = false;
		}
		return ret;
    }
    public List<crash_base_obj> _crash_objs = new List<crash_base_obj>();

    public crash_manager _crash_manager;
    public int _color_group;
}


public class crash_obj_addr
{
    public crash_base_obj _crash_obj = null;
}

public class crash_mole_addr
{
    public crash_mole _crash_mole = null;
}


enum crash_define{
    max_x = 50,
    max_z = 18,
    max_y = 80

}

public enum camera_dir
{
    front,
    right,
    back,
    left    
}

public enum want_move_dir
{
    no,
    left,
    right
}


public enum CreatureHistory_type
{
    moveState,
    set_pos,
    free,
    frozen
}
public class CreatureHistory
{
    protected CreatureHistory_type _creature_type;
    public CreatureHistory_type get_hist_type()
    {
        return _creature_type;
    }

    public int fram_index;

}


public class CreatureMoveHistory : CreatureHistory
{
    public CreatureMoveHistory()
    {
        _creature_type = CreatureHistory_type.moveState;
    }
    public dir_move _dir = new dir_move();
    public bool _move;
    public Vector3 _pos = new Vector3();
}

public class CreatureFrozenHistory : CreatureHistory
{
    public CreatureFrozenHistory()
    {
        _creature_type = CreatureHistory_type.frozen;
    }
    public dir_move _dir = new dir_move();

    public Vector3 _pos = new Vector3();

    public bool _frozen;
}
public class CreaturePosSetHistory : CreatureHistory
{
    public CreaturePosSetHistory()
    {
        _creature_type = CreatureHistory_type.set_pos;
    }
    public dir_move _dir = new dir_move();
    public Vector3 _pos = new Vector3();
}

public class MolMoveHistory
{
	public MolMoveHistory()
	{
		_dir = dir_move.no;
	}
    public dir_move _dir = new dir_move();
    public List<crash_mole> move_moles = new List<crash_mole>();
    public List<CreatureHistory> _Creature_acts = new List<CreatureHistory>();
    public int _mole_move_count;
}


public class CrashMoveHistory
{
    public List<MolMoveHistory> _mol_history = new List<MolMoveHistory>();
    public void reset()
    {
        _mol_history.Clear();
    }

}


public enum crashmo_record_type
{
	record_closed,
	record_ready_for_open,
	record_open,
	record_ready_for_closed,

}

public class CrashMoRecord
{
    public CrashMoRecord()
    {
        _frame_begin = 0;
        _frame_index = 0;
        //_open_record = false;
		_open_record_state = false;
		//set_open_type(crashmo_record_type.record_closed);
    }

	public bool _open_record
	{
		get {
			return _open_record_state;
		}

	}
    public int _frame_begin;
    public int _frame_index;
    public bool _my_open_record;
	private bool _open_record_state;
    public bool _creature_lock;
	private crashmo_record_type _open_type = new crashmo_record_type();
	public crashmo_record_type get_open_type()
	{
		return _open_type;
	}
	private void ModifyButton (string txt)
	{
		global_instance.Instance._ngui_edit_manager._record_txt.text = txt;
	}
	private void set_open_type(crashmo_record_type tp)
	{
		switch (tp) {
		case crashmo_record_type.record_closed:
			{
				ModifyButton("record");
			}
			break;
		case crashmo_record_type.record_open:
			{
				ModifyButton("close record");
			}
			break;
		case crashmo_record_type.record_ready_for_closed:
			{
				ModifyButton("prepare closed");
			}
			break;

		case crashmo_record_type.record_ready_for_open:
			{
				ModifyButton("prepare open");
			}
			break;
		}
		_open_type = tp;
		
	}
	public void ready_for_record_closed()
	{
		set_open_type (crashmo_record_type.record_ready_for_closed);

	}

	public void ready_for_record_open()
	{		
		set_open_type (crashmo_record_type.record_ready_for_open);
	}

	private bool is_in_ready_type()
	{
		if (_open_type == crashmo_record_type.record_closed || _open_type == crashmo_record_type.record_open) 
		{
			return true;
		}

		return false;
	}

	public void try_to_next_state()
	{
        bool need_next_update = false;
		if (_open_type == crashmo_record_type.record_ready_for_closed) 
		{
			set_open_type (crashmo_record_type.record_closed);
			_open_record_state = false;
            need_next_update = true;

        }

		if (_open_type == crashmo_record_type.record_ready_for_open) 
		{
			set_open_type (crashmo_record_type.record_open);
			_open_record_state = true;
            need_next_update = true;
        }

        if(need_next_update)
        {
            global_instance.Instance._crash_manager.next_update();
        }
	}
}

public enum gameState
{
    game_prepare,
    game_playing,
    game_end
}
public class crash_manager
{
    public int _max_x = 0;
    public int _max_y = 0;
    public int _max_z = 0;
    public Dictionary<int, Color> _group_colors = new Dictionary<int, Color>();
    bool[, ,] _can_move_locks;
    public crash_obj_addr[, ,] _crash_objs;
    public crash_mole_addr[, ,] _crash_moles;
    public List<crash_mole> _crash_moles_list = new List<crash_mole>();
    public List<crash_mole> _move_mole_list = new List<crash_mole>();
    public List<crash_mole> _can_not_move_list = new List<crash_mole>();
    public List<crashmolegrid> _alpha_grids = new List<crashmolegrid>();
    public float _grid_distance;
    public float _current_move_distance;
    public float _move_animation_distance;
    public bool _need_play_animation;
    dir_move _last_move_dir;
    GameObject _source_crash_mole_obj;
    GameObject _source_flag_mole_obj;
    protected ArrayList _Game_objs = new ArrayList();
    public int _use_count = 0;
    public bool[] _dir_btn_down = new bool[4];
    public creature _creature = null;
    public GameObject _creaatuee_obj = null;
    public bool _cash_click = false;
    public bool _freezen_creature = false;
    public List<crash_mole> _lock_mole = new List<crash_mole>();
    public List<GameObject> _enclosure_objs = new List<GameObject>();
    public crash_obj_creature _obj_creature = null;
    camera_dir _camera_dir = camera_dir.front;
    want_move_dir _want_camera_dir = want_move_dir.no;
    int _move_count = 0;
    public CrashMoveHistory _History = new CrashMoveHistory();
    public CrashMoRecord _record = new CrashMoRecord();
    public int _current_frame_count;
    gameState _game_state = gameState.game_prepare;
    //public bool _game_begin = false;
    protected Queue<KeyValuePair<int, bool>> _catch_click_list = new Queue<KeyValuePair<int, bool>>();
    public void init()
    {
        clear();
        _max_x = (int)crash_define.max_x;
        _max_z = (int)crash_define.max_z;
        _max_y = (int)crash_define.max_y; 
        _can_move_locks = new bool[_max_x, _max_z, _max_y];
        _crash_objs = new crash_obj_addr[_max_x, _max_z, _max_y];
        _crash_moles = new crash_mole_addr[_max_x, _max_z, _max_y];
        _grid_distance = (float)1;
        for (int x = 0; x < (int)_max_x; x++)
        {
            for (int y = 0; y < (int)_max_y; y++)
            {
                for (int z = 0; z < (int)_max_z; z++)
                {
                    _crash_objs[x, z, y] = new crash_obj_addr();
                    _crash_moles[x, z, y] = new crash_mole_addr();
                }
            }
        }
        next_update();
    }

    public gameState getGameState()
    {
        return _game_state;
    }
    public void move_camera_left()
    {
        if(_want_camera_dir == want_move_dir.no)
        {
            _want_camera_dir = want_move_dir.left;
            _move_count = 0;
        }
    }
    public void move_camera_right()
    {
        if (_want_camera_dir == want_move_dir.no)
        {
            _want_camera_dir = want_move_dir.right;
            _move_count = 0;
        }
    }    
    void camera_move_update()
    {        
        if(_want_camera_dir != want_move_dir.no)
        {
            int current = (int)_camera_dir;
            switch(_want_camera_dir)
            {
                case want_move_dir.left:
                    {
                        current -= 1;
                    }
                    break;
                case want_move_dir.right:
                    {
                        current += 1;
                    }
                    break;
            }

            int special_temp = 0;
            if (current < 0)
            {
                current = 3;
                special_temp = 1;
            }
            else if (current >= 4)
            {
                current = 0;
                special_temp = 2;
            }
            special_temp = 0;
            Vector3 current_pos = get_camera_pos(_camera_dir);
            Vector3 map_current_pos = get_map_camera_pos(_camera_dir);
            Vector3 current_rotation = get_camera_rotation(_camera_dir);
            Vector3 target_pos = get_camera_pos((camera_dir)current);
            Vector3 map_target_pos = get_map_camera_pos((camera_dir)current);
            Vector3 target_rotetion = get_camera_rotation((camera_dir)current);
            Vector3 current_camera_pos = new Vector3();
            Vector3 map_current_camera_pos = new Vector3();
            Vector3 current_camera_roation = new Vector3();
            int need_count = 20;
            _move_count += 1;
            current_camera_pos.x = current_pos.x + ((target_pos.x - current_pos.x) / need_count * _move_count);
            current_camera_pos.y = current_pos.y + ((target_pos.y - current_pos.y) / need_count * _move_count);
            current_camera_pos.z = current_pos.z + ((target_pos.z - current_pos.z) / need_count * _move_count);

            map_current_camera_pos.x = map_current_pos.x + ((map_target_pos.x - map_current_pos.x) / need_count * _move_count);
            map_current_camera_pos.y = map_current_pos.y + ((map_target_pos.y - map_current_pos.y) / need_count * _move_count);
            map_current_camera_pos.z = map_current_pos.z + ((map_target_pos.z - map_current_pos.z) / need_count * _move_count);

            current_camera_roation.x = current_rotation.x + ((target_rotetion.x - current_rotation.x) / need_count * _move_count);
            if (special_temp == 0)
            {
                float temp_f = target_rotetion.y - current_rotation.y;
                if(temp_f > 93)
                {
                    temp_f = -90;
                }
                else if(temp_f < -93)
                {
                    temp_f = 90;
                }
                current_camera_roation.y = current_rotation.y + ((temp_f) / need_count * _move_count);
            }                        
            current_camera_roation.z = current_rotation.z + ((target_rotetion.z - current_rotation.z) / need_count * _move_count);
            Camera.main.transform.position = current_camera_pos;
            Camera.main.transform.eulerAngles = current_camera_roation;
            global_instance.Instance._ngui_edit_manager._camera_map.transform.position = map_current_camera_pos;
            global_instance.Instance._ngui_edit_manager._camera_map.transform.eulerAngles = current_camera_roation;
            global_instance.Instance._ngui_edit_manager._camera_map_big.transform.position = map_current_camera_pos;
            global_instance.Instance._ngui_edit_manager._camera_map_big.transform.eulerAngles = current_camera_roation;
           
            if (_move_count == need_count)
            {
                _want_camera_dir = want_move_dir.no;
                _move_count = 0;
                _camera_dir = (camera_dir)current;
            }
        }

    }
    public Vector3 get_map_camera_pos(camera_dir dir)
    {
        Vector3 vc = _creature.get_position();
        vc.y += 7.21f;
        switch (dir)
        {
            case camera_dir.front:
                {
                    vc.z -= (10 + 4.48f);
                    vc.x -= 0.5f;
                }
                break;
            case camera_dir.back:
                {
                    vc.z += (10 + 4.48f);
                    vc.x += 0.5f;
                }
                break;
            case camera_dir.right:
                {
                    vc.x += (10 + 4.48f);
                    vc.z -= 0.5f;
                }
                break;
            case camera_dir.left:
                {
                    vc.x -= (10 + 4.48f);
                }
                break;
        }
        return vc;
    }
    public Vector3 get_camera_pos(camera_dir dir)
    {       
        Vector3 vc = _creature.get_position();

        vc.y += 1.21f;
        switch (dir)
        {
            case camera_dir.front:
                {
                    vc.z -= 4.48f;
                    vc.x -= 0.5f;
                }
                break;
            case camera_dir.back:
                {
                    vc.z += 4.48f;
                    vc.x += 0.5f;
                }
                break;
            case camera_dir.right:
                {
                    vc.x += 4.48f;
                    vc.z -= 0.5f;
                }
                break;
            case camera_dir.left:
                {
                    vc.x -= 4.48f;
                }
                break;
        }
        return vc;
    }
    public Vector3 get_camera_rotation(camera_dir dir)
    {
        Vector3 vc = new Vector3(12, 0,0);
        switch (dir)
        {
            case camera_dir.front:
                {

                }
                break;
            case camera_dir.back:
                {
                    vc.y = 180;
                }
                break;
            case camera_dir.right:
                {
                    vc.y = 270;
                }
                break;
            case camera_dir.left:
                {
                    vc.y = 90;
                }
                break;
        }
        return vc;

    }
    public void update_camera()
    {
        Vector3 vc_target = get_camera_pos(_camera_dir);
        
        float offset_x = Camera.main.transform.position.x - vc_target.x;
        float offset_y = Camera.main.transform.position.y - vc_target.y;
        float offset_z = Camera.main.transform.position.z - vc_target.z;
        Vector3 vc_target_temp = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        float offset_temp = 0.2f;
        if(offset_x < -offset_temp)
        {
            vc_target_temp.x = vc_target.x - offset_temp;
        }
        else if(offset_x > offset_temp)
        {
            vc_target_temp.x = vc_target.x + offset_temp;
        }
        if(offset_y < -offset_temp)
        {
            vc_target_temp.y = vc_target.y - offset_temp;
        }
        else if(offset_y > offset_temp)
        {
            vc_target_temp.y = vc_target.y + offset_temp;
        }
        
        if(offset_z < -offset_temp)
        {
            vc_target_temp.z = vc_target.z - offset_temp;
        }
        else if(offset_z > offset_temp)
        {
            vc_target_temp.z = vc_target.z + offset_temp;
        }
 
        Camera.main.transform.position = vc_target_temp;
		global_instance.Instance._ngui_edit_manager._camera_map.transform.position = get_map_camera_pos (_camera_dir);
        global_instance.Instance._ngui_edit_manager._camera_map_big.transform.position = get_map_camera_pos(_camera_dir);
    }
    public void add_color(int group, Color temp_color)
    {
        if (_group_colors.ContainsKey(group) == false)
        {
            _group_colors.Add(group, temp_color);
        }        
    }
    public void creature_jump()
    {
        if(_creature != null)
        {
            if(_freezen_creature == false)
            {
                _creature.jump();
            }            
        }
    }
    public void update_dir_btn()
    {
        bool have_update = false;
        bool no_key_down = true;
        for (int i = 0; i < 4; i ++ )
        {
			if(global_instance.Instance._ngui_edit_manager._game_btns._dir_button_down[i])
            {
                no_key_down = false;
                if(_dir_btn_down[i] == false)
                {
                    int temp = i + (int)_camera_dir;
                    if(temp > 3)
                    {
                        temp = temp - 4; 
                    }

                    have_update = true;
                    if (_freezen_creature)
                    {
                        move_lock_mole((dir_move)temp);
                    }
                    else
                    {
                        _dir_btn_down[i] = true;
                        _creature.set_dir((dir_move)temp);
                        _creature.set_state(creature_state.run);
                    }                                     
                }                
            }
            else
            {
                if (_dir_btn_down[i] == true)
                {
                    have_update = true;
                    _dir_btn_down[i] = false;
                }
                
            }
        }
        if(have_update && no_key_down)
        {
            if(_freezen_creature)
            {

            }
            else
            {
                _creature.set_state(creature_state.idle);
            }
            
        }
    }
    public void move_lock_mole(dir_move dir)
    {
        move_list(_lock_mole, dir);
    }    
    protected void _catch_click(int i, bool his)
    {
        if (_creature._is_in_falldown == false)
        {
            if (i == 0)
            {
                _cash_click = false;
            }
            else
            {
                _cash_click = true;
            }
            Vector3 vc = _creature.get_position();
            dir_move dir = _creature.get_dir();
            crash_pos pos = new crash_pos();
            pos._x = transform_to_map(vc.x);
            pos._y = transform_to_map(vc.y);
            pos._z = transform_to_map(vc.z);

            if (_cash_click)
            {
                if (_freezen_creature == false)
                {
                    crash_pos pos_target = new crash_pos();
                    pos_target.clone(pos);
                    pos_target.move(dir);
                    if (check_pos_valid(pos_target) == true)
                    {
                        crash_mole target_mole = get_crash_mole_addr(pos_target)._crash_mole;
                        if (target_mole != null)
                        {
                            _freezen_creature = true;
                            if (his == false)
                            {
                                _creature.frozen_history(_freezen_creature);
                            }
                            _obj_creature = new crash_obj_creature(pos._x, pos._y, pos._z);
                            _obj_creature._creature = _creature;
                            _creature.set_position(pos._x + 0.45f, pos._y + 0.2f, pos._z + 0.5f);
                            target_mole.add_crash_obj(_obj_creature);
                            _lock_mole.Add(target_mole);
							global_instance.Instance._ngui_edit_manager._game_btns.CatchButtonUpdate (true);
                        }
                    }
                }
            }
            else
            {
                if (_obj_creature != null)
                {
                    _lock_mole.Clear();
                    if (_freezen_creature == true)
                    {
                        pos.move(dir_move.up);
                        if (check_pos_valid(pos))
                        {
                            if (_obj_creature._crash_mole != null)
                            {
                                crash_mole mole_temp = _obj_creature._crash_mole;
                                mole_temp.remove_crash_obj(_obj_creature);
                                _obj_creature = null;
                            }

                        }
						global_instance.Instance._ngui_edit_manager._game_btns.CatchButtonUpdate (false);
                        _freezen_creature = false;
                        if (his == false)
                        {
                            _creature.frozen_history(_freezen_creature);
                        }
                    }
                }
            }
        }
    }
    public void catch_click(int i, bool his = false)
	{
        if(global_instance.Instance._crash_mole_grid_manager.get_game_type() == game_type.game 
            && _game_state == gameState.game_playing)
        {
            KeyValuePair<int, bool> entry = new KeyValuePair<int, bool>(i, his);
            _catch_click_list.Enqueue(entry);
        }
        
    }

    protected void GameEnd()
    {
		_creature.set_state (creature_state.idle);
		_game_state = gameState.game_end;
        global_instance.Instance._ngui_edit_manager.game_win();        
    }
    public void update()
    {
        camera_move_update();        
        if(global_instance.Instance._crash_mole_grid_manager.get_game_type() == game_type.game)
        {
            if (_game_state == gameState.game_playing && _need_play_animation == false)
            {
                Vector3 vc_position = _creature.get_position();
                crash_pos pos = new crash_pos();
                pos._x = transform_to_map(vc_position.x);
                pos._y = transform_to_map(vc_position.y);
                pos._z = transform_to_map(vc_position.z);
                if (get_crash_obj_addr(pos) != null)
                {
                    if (get_crash_obj_addr(pos)._crash_obj != null)
                    {
                        if (get_crash_obj_addr(pos)._crash_obj.get_obj_type() == crash_obj_type.flag)
                        {                            
                            GameEnd();
                        }
                    }
                }
                while (_catch_click_list.Count != 0)
                {
                    KeyValuePair<int, bool> entry = _catch_click_list.Dequeue();
                    _catch_click(entry.Key, entry.Value);
                }
            }
            update_move_animation();
        }        
    }
    public int transform_to_map(float temp_number)
    {
        if(temp_number < 0)
        {
            return -1;
        }
        int temp = (int)temp_number;
        int grid = (int)(temp / _grid_distance);
        return grid;
    }

	public crash_pos transform_to_map(Vector3 vec)
	{
		crash_pos entry_pos = new crash_pos ();
		entry_pos._x = transform_to_map (vec.x);
		entry_pos._y = transform_to_map (vec.y);
		entry_pos._z = transform_to_map (vec.z);
		return entry_pos;
	}


	public bool is_block(Vector3 vec)
	{
		return is_block (vec.x, vec.y, vec.z);
	}

    public bool is_block(float tempx, float tempy, float tempz)
    {
        crash_pos pos = new crash_pos();
        pos._x = transform_to_map(tempx);
        pos._y = transform_to_map(tempy);
        pos._z = transform_to_map(tempz);
        if(get_crash_mole_addr(pos) == null)            
        {
            return true;
        }
        else if (get_crash_mole_addr(pos) != null && get_crash_mole_addr(pos)._crash_mole != null)
        {
            if(get_crash_obj_addr(pos) != null)
            {
                if(get_crash_obj_addr(pos)._crash_obj.get_obj_type() != crash_obj_type.flag)
                {
                    return true;
                }
            }   
        }

        return false;
    }
    bool check_block(float tempx, float tempy, float tempz, float block_width)
    {
        if (is_block(tempx + block_width, tempy, tempz))
        {
            return true;
        }

        if (is_block(tempx, tempy + block_width, tempz))
        {
            return true;
        }

        if (is_block(tempx, tempy, tempz + block_width))
        {
            return true;
        }

        if (is_block(tempx + block_width, tempy + block_width, tempz))
        {
            return true;
        }

        if (is_block(tempx, tempy + block_width, tempz + block_width))
        {
            return true;
        }

        if (is_block(tempx + block_width, tempy, tempz + block_width))
        {
            return true;
        }
        return false;
    }
     public bool is_block_creature(float tempx, float tempy, float tempz)
    {
        float block_width = 0.18f;
        crash_pos pos = new crash_pos();
        pos._x = transform_to_map(tempx);
        pos._y = transform_to_map(tempy);
        pos._z = transform_to_map(tempz);
        if (isvalid(pos) == false)
        {
            return true;
        }
        if (is_block(tempx, tempy, tempz) == true)
        {
            return true;
        }

        if(check_block(tempx, tempy, tempz, block_width))
        {
            return true;
        }

        if (check_block(tempx, tempy, tempz, -block_width))
        {
            return true;
        }
        return false;

    }
    public float transform_to_position(int number)
    {
        float position = _grid_distance * ((float)number + 0.5f);
        return position;
    }
    public crash_manager()
    {
        _source_crash_mole_obj = Resources.Load<GameObject>("prefab/mole_object");
        _source_flag_mole_obj = Resources.Load<GameObject>("prefab/flag");        
    }
    public void clear()
    {
        clear_enclosure();
        if (_crash_objs != null && _crash_moles != null)
        {
            for (int x = 0; x < (int)_max_x; x++)
            {
                for (int y = 0; y < (int)_max_y; y++)
                {
                    for (int z = 0; z < (int)_max_z; z++)
                    {
                        _crash_objs[x, z, y]._crash_obj = null;
                        _crash_moles[x, z, y]._crash_mole = null;
                    }
                }
            }
        }
        _grid_distance = (float)1;        
        _need_play_animation = false;
        _move_animation_distance = (float)0.1;
        int length = _Game_objs.Count;
        for(int i = 0; i < length; i ++)
        {
            GameObject obj = (GameObject)_Game_objs[i];
            GameObject.Destroy(obj);            
        }
        _crash_moles_list.Clear();
        _move_mole_list.Clear();
        _Game_objs.Clear();
		_obj_creature = null;
		_freezen_creature = false;
		_lock_mole.Clear ();
        if(_creature != null)
        {
            _creature.gameObject.SetActive(false);
        }
        _catch_click_list.Clear();
        _game_state = gameState.game_prepare;
       _History.reset();
        _current_frame_count = 0;
        _move_count = 0;
    }
    public void update_move_animation()
    {
        if (_need_play_animation)
        {
            bool last_move = false;
            float move_distance = _current_move_distance + _move_animation_distance;
            if (move_distance >= _grid_distance)
            {
                last_move = true;
                _current_move_distance = _grid_distance;
            }
            else
            {
                _current_move_distance = move_distance;
            }
            int temp_count = _move_mole_list.Count;
            for (int j = 0; j < temp_count; j++)
            {
                crash_mole mole = (crash_mole)_move_mole_list[j];
                int crash_obj_count = mole._crash_objs.Count;
                for (int i = 0; i < crash_obj_count; i++)
                {
                    crash_base_obj obj = mole._crash_objs[i];
                    float x_temp = obj._last_pos._x * _grid_distance;
                    float z_temp = obj._last_pos._z * _grid_distance ;
                    float y_temp = obj._last_pos._y * _grid_distance;
                    
                    switch (_last_move_dir)
                    {
                        case dir_move.back:
                            {
                                z_temp += _current_move_distance;
                            }
                            break;
                        case dir_move.front:
                            {
                                z_temp -= _current_move_distance;
                            }
                            break;
                        case dir_move.left:
                            {
                                x_temp -= _current_move_distance;
                            }
                            break;
                        case dir_move.right:
                            {
                                x_temp += _current_move_distance;
                            }
                            break;
                        case dir_move.down:
                            {
                                y_temp -= _current_move_distance;
                            }
                            break;
                        case dir_move.up:
                            {
                                y_temp += _current_move_distance;
                            }
                            break;
                    }
                    obj.set_position(x_temp, y_temp, z_temp);
                }
            }

            if (last_move)
            {
                temp_count = _move_mole_list.Count;
                for (int j = 0; j < temp_count; j++)
                {
                    crash_mole mole = (crash_mole)_move_mole_list[j];
                    int crash_obj_count = mole._crash_objs.Count;
                    for (int i = 0; i < crash_obj_count; i++)
                    {
                        crash_base_obj obj = mole._crash_objs[i];
                        obj._last_pos = obj._pos;
                    }
                }
                _current_move_distance = 0;
                _need_play_animation = false;
                next_update();
            }
        }

    }
    void seek_create_mole(int temp_x, int temp_y, crash_mole mole, bool dir_up = false)
    {
        MapData map_data = global_instance.Instance.GetMapData();
        if(map_data == null)
        {
            return;
        }

        if (temp_x >= 0 && temp_x < map_data.width_ && temp_y >= 0 && temp_y < map_data.height_)
        {
            crash_mole mole_entry = null;
            int group = 11;
            group = map_data.groups_[temp_x, temp_y];
            if (group != 11)
            {
                int x_offset = (_max_x - map_data.width_) / 2;
                mole_entry = global_instance.Instance._crash_manager.get_crash_mole_addr(temp_x + x_offset, 9, temp_y + 20)._crash_mole;
                if (mole_entry == null)
                {
                    if (group == 10)
                    {
                        if (dir_up == true)
                        {
                            crash_base_obj obj = global_instance.Instance._crash_manager.create_flag_obj(temp_x + x_offset, temp_y + 20);
                            mole.add_crash_obj(obj);
                            create_mole(temp_x, temp_y, mole);
                        }
                    }
                    else if (group == mole._color_group || dir_up == true && group == 10)
                    {
                        crash_obj obj = global_instance.Instance._crash_manager.create_crash_obj(temp_x + x_offset, temp_y + 20);
                        mole.add_crash_obj(obj);
                        create_mole(temp_x, temp_y, mole);
                    }
                }

            }

        }
    }
    public void create_mole(int x, int y, crash_mole mole)
    {
        seek_create_mole(x - 1, y, mole);
        seek_create_mole(x + 1, y, mole);
        seek_create_mole(x, y - 1, mole);
        seek_create_mole(x, y + 1, mole, true);
    }
    public void clear_enclosure()
    {
        foreach(GameObject obj in _enclosure_objs)
        {
            GameObject.Destroy(obj);

        }
    }
    public void create_enclosure()
    {
        GameObject obj_temp = null;
        crashmolegrid grid = null;
        for (int i = 0; i < _max_x; i ++)
        {
            obj_temp = Object.Instantiate<GameObject>(_source_crash_mole_obj);
            grid  = obj_temp.GetComponent<crashmolegrid>();
            grid.set_position(i, 0, -1);
            grid.set_alpha(12);
            _enclosure_objs.Add(obj_temp);
            obj_temp = Object.Instantiate<GameObject>(_source_crash_mole_obj);
            grid = obj_temp.GetComponent<crashmolegrid>();
            grid.set_position(i, 0, _max_z);
            grid.set_alpha(12);
            _enclosure_objs.Add(obj_temp);
        }

        for(int i = 0; i < _max_z; i ++)
        {
            obj_temp = Object.Instantiate<GameObject>(_source_crash_mole_obj);
            grid = obj_temp.GetComponent<crashmolegrid>();
            grid.set_position(-1, 0, i);
            grid.set_alpha(12);
            _enclosure_objs.Add(obj_temp);
            obj_temp = Object.Instantiate<GameObject>(_source_crash_mole_obj);
            grid = obj_temp.GetComponent<crashmolegrid>();
            grid.set_position(_max_x, 0, i);
            grid.set_alpha(12);
            _enclosure_objs.Add(obj_temp);
        }
    }
    public void create_map()
    {
        create_enclosure();
        MapData map_data = global_instance.Instance.GetMapData();
        if(map_data != null)
        {
            for (int i = 0; i < map_data.width_; i++)
            {
                for (int j = 0; j < map_data.height_; j++)
                {
                    int group = map_data.groups_[i, j];
                    if (group != 11)
                    {
                        int temp_x = (_max_x - map_data.width_) / 2 + i;
                        int temp_y = /*(_max_y - map_data.height_)+ */j + 20;
                        crash_mole mole_entry = global_instance.Instance._crash_manager.get_crash_mole_addr(temp_x, 9, temp_y)._crash_mole;
                        if (mole_entry == null)
                        {
                            crash_base_obj obj = null;
                            if (group != 10)
                            {
                                obj = global_instance.Instance._crash_manager.create_crash_obj(temp_x, temp_y);
                            }
                            else
                            {
                                obj = global_instance.Instance._crash_manager.create_flag_obj(temp_x, temp_y);
                            }

                            mole_entry = global_instance.Instance._crash_manager.create_crash_mole();
                            mole_entry.add_crash_obj(obj);
                            mole_entry._color_group = group;
                            global_instance.Instance._crash_manager.add_crash_mole(mole_entry);
                            create_mole(i, j, mole_entry);
                        }
                    }
                }
            }
        }

        for (int x = 0; x < (int)_max_x; x++)
        {
            for (int z = 0; z < (int)_max_z; z++)
            {
                for (int y = 0; y < (int)_max_y; y++)
                {
                    if (_crash_objs[x, z, y]._crash_obj != null)
                    {
                        crash_base_obj entry = _crash_objs[x, z, y]._crash_obj;
                        switch(entry.get_obj_type())
                        {
                            case crash_obj_type.normal:
                                {
                                    GameObject obj_temp = Object.Instantiate<GameObject>(_source_crash_mole_obj);
                                    _Game_objs.Add(obj_temp);
                                    crash_obj crash_obj_temp = (crash_obj)entry;                                  
                                    crash_obj_temp._grid = obj_temp.GetComponent<crashmolegrid>();
                                    crash_obj_temp._grid.set_group(_crash_objs[x, z, y]._crash_obj._crash_mole._color_group);    
                                }
                                break;
                            case crash_obj_type.flag:
                                {
                                    GameObject obj_temp = Object.Instantiate<GameObject>(_source_flag_mole_obj);
                                    _Game_objs.Add(obj_temp);
                                    crash_obj_flag crash_temp = (crash_obj_flag)entry;
                                    crash_temp._flag = obj_temp.GetComponent<crash_flag>();                                
                                }
                                break;
                        }
                        float x_temp = x * _grid_distance;
                        float z_temp = z * _grid_distance;
                        float y_temp = y * _grid_distance;
                        entry.set_position(x_temp, y_temp, z_temp);
                    }                    
                }
            }
        }

        foreach (crash_mole entry in _crash_moles_list)
        {
            entry.update_side();
        }

        if(_creature == null)
        {
            GameObject obj_temp = Resources.Load<GameObject>("prefab/creature");
            GameObject obj_creature = Object.Instantiate<GameObject>(obj_temp);
            _creature = obj_creature.GetComponent<creature>();            
            _creature.set_creature_type(creature_type.creature_2);              
        }
        _camera_dir = camera_dir.front;
        _want_camera_dir = want_move_dir.no;
        _creature.gameObject.SetActive(true);
        _creature.set_position(22, 2, 3);

        _camera_dir = camera_dir.front;
        _want_camera_dir = want_move_dir.no;
        Camera.main.transform.position = get_camera_pos(_camera_dir);
        Camera.main.transform.eulerAngles = get_camera_rotation(_camera_dir);

        global_instance.Instance._ngui_edit_manager._camera_map.transform.position = get_map_camera_pos(_camera_dir);
        global_instance.Instance._ngui_edit_manager._camera_map.transform.eulerAngles = get_camera_rotation(_camera_dir);
        global_instance.Instance._ngui_edit_manager._camera_map_big.transform.position = get_map_camera_pos(_camera_dir);
        global_instance.Instance._ngui_edit_manager._camera_map_big.transform.eulerAngles = get_camera_rotation(_camera_dir);

        for (int i = 0 ;i < 4; i ++)
        {
            _dir_btn_down[i] = false;
        }

        next_update();
    }
    public crash_obj create_crash_obj(int x, int y)
    {
        crash_obj obj = new crash_obj();
        obj._pos._x = x;
        obj._pos._y = y;
        obj._pos._z = 9;
        //add_crash_obj(obj);
        return obj;
    }

    public crash_base_obj create_flag_obj(int x, int y)
    {
        crash_obj_flag obj = new crash_obj_flag();
        obj._pos._x = x;
        obj._pos._y = y;
        obj._pos._z = 9;
        return obj;
    }

    public crash_mole create_crash_mole()
    {
        crash_mole mole_entry = new crash_mole();
        mole_entry._crash_manager = this;
        return mole_entry;
    }

    public bool check_pos_valid(crash_pos pos)
    {
        if (pos._x < 0 || pos._y < 0 || pos._z < 0)
        {
            return false;
        }

        if (pos._x >= (int)_max_x || pos._y >= (int)_max_y || pos._z >= (int)_max_z)
        {
            return false;
        }
        return true;
    }

    public void clear_block()
    {
        for (int x = 0; x < (int)_max_x; x++)
        {
            for (int z = 0; z < (int)_max_z; z++)
            {
                for (int y = 0; y < (int)_max_z; y++)
                {
                    _can_move_locks[x, z, y] = false;
                }
            }
        }
    }
    public void set_block(int x, int z, int y)
    {
        _can_move_locks[x, z, y] = true;
    }
    bool is_in_can_not_move_list(crash_mole entry)
    {
        foreach(crash_mole temp in _can_not_move_list)
        {
            if(temp == entry)
            {
                return true;
            }
        }
        return false;
    }
    public bool move_list(List<crash_mole> list, dir_move dir)
    {
        if (_need_play_animation == true)
        {
            return false;
        }
        _move_mole_list.Clear();
        List<crash_mole> enry_list = new List<crash_mole>();
        int current_count = list.Count;
        bool can_fall_temp = false;
        for (int i = 0; i < current_count; i++)
        {
            bool need_continue = false;
            int move_count = enry_list.Count;
            crash_mole cur_entry = list[i];
            if (is_in_can_not_move_list(cur_entry) == true)
            {
                continue;
            }
            for (int j = 0; j < move_count; j++)
            {
                crash_mole temp_entry = enry_list[j];
                if (temp_entry == cur_entry)
                {
                    need_continue = true;
                    break;
                }
            }
            if (need_continue == true)
            {
                continue;
            }

            _move_mole_list.Add(cur_entry);
            if (move(cur_entry, dir) == true)
            {
                move_count = _move_mole_list.Count;
                for (int j = 0; j < move_count; j++)
                {
                    enry_list.Add(_move_mole_list[j]);
                }
                update_move_list(_move_mole_list, dir);
            }
            else
            {
                _can_not_move_list.Add(cur_entry);
            }
            _move_mole_list.Clear();
        }

        _can_not_move_list.Clear();
        int cur_count = enry_list.Count;
        if (cur_count > 0)
        {
            can_fall_temp = true;
            _need_play_animation = true;
            _last_move_dir = dir;
            _move_mole_list.Clear();
            for (int i = 0; i < cur_count; i++)
            {
                _move_mole_list.Add(enry_list[i]);                
            }

			if(_record._open_record == false&& _game_state == gameState.game_playing)
            {
                MolMoveHistory history = new MolMoveHistory();
                history._dir = dir;
                foreach (crash_mole entry in _move_mole_list)
                {
                    history.move_moles.Add(entry);
                }
                _current_frame_count++;
                history._mole_move_count = _current_frame_count;
                _History._mol_history.Add(history);
            }
        }

        return can_fall_temp;
    }
    public bool need_fall_update()
    {
        return move_list(_crash_moles_list, dir_move.down);
    }
    public void next_update()
    {
        if(_record._open_record == true)
        {
			if(_creature._Creature_history.Count == 0&&_record._creature_lock != true&& _need_play_animation == false)
            {									
                int count = _History._mol_history.Count;
                if (count > 0)
                {
                    _record._frame_begin = _current_frame_count;
                    MolMoveHistory history = _History._mol_history[count - 1];
                    _History._mol_history.RemoveAt(count - 1);
                    if (history._Creature_acts.Count != 0)
                    {
                        _record._creature_lock = true;
                        foreach(CreatureHistory entry in history._Creature_acts)
                        {
                            _creature._Creature_history.Add(entry);
                        }
                    }
                    int mole_count = history.move_moles.Count;
					List<crash_mole> temp_list = new List<crash_mole> (); 
                    foreach(crash_mole entry in history.move_moles)
                    {
						temp_list.Add(entry);
                    }
                   switch(history._dir)
                    {
                        case dir_move.back:
                            {
                                _last_move_dir = dir_move.front;
                            }
                            break;
                        case dir_move.front:
                            {
                                _last_move_dir = dir_move.back;
                            }
                            break;
                        case dir_move.left:
                            {
                                _last_move_dir = dir_move.right;
                            }
                            break;
                        case dir_move.right:
                            {
                                _last_move_dir = dir_move.left;
                            }
                            break;
                        case dir_move.down:
                            {
                                _last_move_dir = dir_move.up;
                            }
                            break;
                        case dir_move.up:
                            {
                                _last_move_dir = dir_move.down;
                            }
                            break;
                    }
					if(temp_list.Count != 0)
                    {
						move_list(temp_list, _last_move_dir);
                    }                    
                }
                else
                {
					_record.ready_for_record_closed();
					_record.try_to_next_state ();

                }
            }
        }
        else
        {
            bool temp = need_fall_update();
			if (temp == false && _need_play_animation == false && _crash_moles_list.Count != 0) 
			{
				_game_state = gameState.game_playing;
			}
			if (temp) 
			{
				_record.try_to_next_state ();
			}
        }
    }
    public void update_move_list(List<crash_mole> temp_list, dir_move dir)
    {
        List<crash_mole> list = new List<crash_mole>();
        foreach(crash_mole entry in temp_list)
        {
            bool find = false;
            foreach(crash_mole entry_find in list)
            {
                if(entry_find == entry)
                {
                    find = true;
                    break;
                }
            }
            if(find == false)
            {
                list.Add(entry);
            }
        }

        temp_list = list;
        int count_temp = temp_list.Count;
        for (int i = 0; i < count_temp; i ++ )
        {
            crash_mole obj_mole = (crash_mole)temp_list[i];
            int crash_objs_count = obj_mole._crash_objs.Count;
            for(int j = 0; j < crash_objs_count; j ++)
            {
                crash_base_obj obj_obj = obj_mole._crash_objs[j];
                _crash_moles[obj_obj._pos._x, obj_obj._pos._z, obj_obj._pos._y]._crash_mole = null;
                _crash_objs[obj_obj._pos._x, obj_obj._pos._z, obj_obj._pos._y]._crash_obj = null;                
            }
        }

        
        for (int i = 0; i < count_temp; i++)
        {
            crash_mole obj_mole = (crash_mole)temp_list[i];
            int crash_objs_count = obj_mole._crash_objs.Count;
            for (int j = 0; j < crash_objs_count; j++)
            {
                crash_base_obj obj_obj = (crash_base_obj)obj_mole._crash_objs[j];
                obj_obj._last_pos = obj_obj._pos;
                obj_obj._pos.move(dir);
                _crash_moles[obj_obj._pos._x, obj_obj._pos._z, obj_obj._pos._y]._crash_mole = obj_obj._crash_mole;
                _crash_objs[obj_obj._pos._x, obj_obj._pos._z, obj_obj._pos._y]._crash_obj = obj_obj;
            }
        }
    }
    protected bool move(crash_mole mole, dir_move dir)
    {
        if(mole == null)
        {
            return false;
        }
        _use_count++;
        int min_move_y = (int)crash_define.max_y;
        foreach (crash_base_obj entry in mole._crash_objs)
        {
            crash_pos pos_temp = new crash_pos();
            pos_temp = entry._pos;
            if (min_move_y > pos_temp._y)
            {
                min_move_y = pos_temp._y;
            }
            pos_temp.move(dir);
            Vector3 vec = _creature.get_position();
            int pos_x = transform_to_map(vec.x);
            int pos_y = transform_to_map(vec.y);
            int pos_z = transform_to_map(vec.z);
            if(_freezen_creature == false)
            {
                if (pos_temp._x == pos_x && pos_temp._y == pos_y && pos_temp._z == pos_z)
                {
                    return false;
                }
            }

            if (check_pos_valid(pos_temp))
            {
                crash_mole mole_entry = get_crash_mole_addr(pos_temp)._crash_mole;
                if (is_in_can_not_move_list(mole_entry) == true)
                {
                    return false;
                }
                
                if(mole_entry == null)
                {
                    continue;
                }
                else
                {
                    if(mole_entry == mole)
                    {
                        continue;
                    }
                    int length = _move_mole_list.Count;
                    bool have_mole = false;
                    for (int i = 0; i < length; i++)
                    {
                        if (mole_entry == (crash_mole)_move_mole_list[i])
                        {
                            have_mole = true;
                        }
                    }
                    if (have_mole == true)
                    {
                        continue;
                    }
                    else
                    {
                        _move_mole_list.Add(mole_entry);
                    }                    
                    if (move(mole_entry, dir) == false)
                    {
                        foreach(crash_mole entry_temp in _move_mole_list)
                        {
                            add_can_not_move_list(entry_temp);
                        }
                        _move_mole_list.Clear();
                        return false;
                    }                                        
                }                
            }
            else
            {
                _move_mole_list.Clear();
                return false;
            }            
        }
        return true;
    }
    void add_can_not_move_list(crash_mole mole)
    {
        foreach(crash_mole entry in _can_not_move_list)
        {
            if(entry == mole)
            {
                return;
            }
        }
        _can_not_move_list.Add(mole);
    }
    public bool isvalid(crash_pos temp)
    {
        return isvalid(temp._x, temp._y, temp._z);
    }
    public bool isvalid(int x, int y, int z)
    {
        if (x < _max_x && x >= 0 && z >= 0 && z < _max_z && y >= 0 && y < _max_y)
        {
            return true;
        }
        return false;
    }
    public crash_obj_addr get_crash_obj_addr(int x, int z, int y)
    {
        crash_obj_addr temp = null;
        if (isvalid(x, y, z))
        {
            temp = _crash_objs[x, y, z];
        }
        return temp;
    }    
    public crash_mole_addr get_crash_mole_addr(int x, int z, int y)
    {
        crash_mole_addr temp = null;
        if(isvalid(x, y, z))
        {
            temp = _crash_moles[x, z, y];
        }
		if (temp == null) {
			int n = 0;
			n++;
		}
        return temp;
    }
    public crash_obj_addr get_crash_obj_addr(crash_pos pos)
    {
        crash_obj_addr temp = null;
        if(isvalid(pos))
        {
            temp = _crash_objs[pos._x, pos._z, pos._y];
        }
        return temp;
    }
    public crash_mole_addr get_crash_mole_addr(crash_pos pos)
    {
        crash_mole_addr temp = null;
        if(isvalid(pos))
        {
            temp = _crash_moles[pos._x, pos._z, pos._y];
        }
        return temp;
    }
    public bool add_crash_obj(crash_obj obj_temp)
    {
        if (isEmpty(obj_temp))
        {
            _crash_objs[obj_temp._pos._x, obj_temp._pos._z, obj_temp._pos._y]._crash_obj = obj_temp;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool add_crash_mole(crash_mole obj_temp)
    {
        foreach (crash_mole entry in _crash_moles_list)
        { 
            if(obj_temp == entry)
            {
                return false;
            }
        
        }
        _crash_moles_list.Add(obj_temp);
        return true;
       
    }
    private bool isEmpty(crash_mole obj_temp)
    {
        int count_temp = obj_temp._crash_objs.Count;
        for (int i = 0; i < count_temp; i++)
        {
            if (isEmpty(((crash_obj)obj_temp._crash_objs[i])) == false)
            {
                return false;
            }                        
        }
        return true;        
    }
    private bool isEmpty(crash_obj obj_temp)
    {
        if (_crash_objs[obj_temp._pos._x, obj_temp._pos._z, obj_temp._pos._y]._crash_obj == null)
        {
            return true;
        }
        return false;
    }

}