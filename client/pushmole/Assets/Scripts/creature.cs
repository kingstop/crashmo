using UnityEngine;
using System.Collections.Generic;

public enum creature_type
{
    creature_1,
    creature_2,
    creature_3
}

public enum creature_state
{
    attack,
    special,    
    run,
    wound,
    idle,
}
public class creature : MonoBehaviour
{
    Dictionary<creature_type, GameObject> _moles = new Dictionary<creature_type, GameObject>();
    List<GameObject> _moles_list = new List<GameObject>();
    //Rigidbody _Rigidbody = null;
    public float _move_speed = 0.005f;
    public float _jump_speed = 0;
    public float _fallspeed = -0.003f;
    public float _current_fall_speed = 0;
    public bool _is_in_falldown = false;
    public List<crashmolegrid> _alpha_grids = new List<crashmolegrid>();
    public creature()
    {
      
    }

    void Awake()
    {
        GameObject obj_1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("character/model/zippermouth_a_PF"));
        _moles.Add(creature_type.creature_1, obj_1);
        GameObject obj_2 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("character/model/zippermouth_b_PF"));
        _moles.Add(creature_type.creature_2, obj_2);
        GameObject obj_3 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("character/model/zippermouth_c_PF"));
        _moles.Add(creature_type.creature_3, obj_3);
        _moles_list.Add(obj_1);
        _moles_list.Add(obj_2);
        _moles_list.Add(obj_3);
        foreach(GameObject obj_entry in _moles_list)
        {
            obj_entry.transform.parent = gameObject.transform;
        }
        set_dir(dir_move.front);
        set_state(creature_state.idle);
        _is_in_falldown = false;
        _frame_count = 0;
    }

    void Start()
    {

    }



    public void Destroy()
    {
        foreach (var entry in _moles)
        {
            GameObject obj_entry = entry.Value;
            GameObject.Destroy(obj_entry);
        }
    }

    public void jump()
    {
        _jump_speed = 0.6f;
    }
    public void set_position(float x, float y, float z)
    {
        Vector3 vc = new Vector3(x, y, z);
		vc.y -= 0.7f;
		vc.x -= 0.5f;
		vc.z -= 0.6f;
        this.transform.position = vc;
              
        foreach (var entry in _moles)
        {
            GameObject obj_entry = entry.Value;
            obj_entry.transform.position = vc;
        }
        global_instance.Instance._crash_manager.update_camera();
    }

    public Vector3 get_position()
    {
        Vector3 vec = new Vector3();
        vec = this.transform.position;
        vec.y += 0.7f;
        vec.x += 0.5f;
        vec.z += 0.6f;
        return vec;
    }


    public void UpdateAlpha()
    {
        Vector3 vec = this.gameObject.transform.position;
        List<RaycastHit> list_hits = new List<RaycastHit>();
        Vector3 vec_temp = new Vector3();

        foreach (crashmolegrid entry in _alpha_grids)
        {
            entry.set_alpha(1);
        }
        vec_temp = vec;
        _alpha_grids.Clear();
        RaycastHit[] temp = Physics.RaycastAll(Camera.main.transform.position, vec_temp - Camera.main.transform.position);
        Vector3[] poses = new Vector3[2];
        poses[0] = Camera.main.transform.position;
        poses[1] = vec_temp;

        float dis_target = Vector3.Distance(vec, Camera.main.transform.position);
        foreach (RaycastHit entry in temp)
        {
            list_hits.Add(entry);
        }

        float alpha_width = 0.5f;
        vec_temp = vec;
        vec_temp.x += alpha_width;
        temp = Physics.RaycastAll(Camera.main.transform.position, vec_temp - Camera.main.transform.position);
        poses[0] = Camera.main.transform.position;
        poses[1] = vec_temp;

        foreach (RaycastHit entry in temp)
        {
            list_hits.Add(entry);
        }

        vec_temp = vec;
        vec_temp.x -= alpha_width;
        temp = Physics.RaycastAll(Camera.main.transform.position, vec_temp - Camera.main.transform.position);
        poses[0] = Camera.main.transform.position;
        poses[1] = vec_temp;

        foreach (RaycastHit entry in temp)
        {
            list_hits.Add(entry);
        }

        vec_temp = vec;
        vec_temp.y -= alpha_width;
        temp = Physics.RaycastAll(Camera.main.transform.position, vec_temp - Camera.main.transform.position);
        poses[0] = Camera.main.transform.position;
        poses[1] = vec_temp;

        foreach (RaycastHit entry in temp)
        {
            list_hits.Add(entry);
        }

        vec_temp = vec;
        vec_temp.y += alpha_width;
        temp = Physics.RaycastAll(Camera.main.transform.position, vec_temp - Camera.main.transform.position);
        poses[0] = Camera.main.transform.position;
        poses[1] = vec_temp;

        foreach (RaycastHit entry in temp)
        {
            list_hits.Add(entry);
        }

     
        foreach (RaycastHit entry in list_hits)
        {
            crashmolegrid grid = entry.collider.gameObject.GetComponent<crashmolegrid>();
            float temp_target = Vector3.Distance(entry.collider.gameObject.transform.position, Camera.main.transform.position);
            if(temp_target < dis_target)
            {
                if (grid != null)
                {
                    grid.set_alpha(0.5f);
                    _alpha_grids.Add(grid);
                }
            }
        }
    }

    public void frozen_history(bool b)
    {
		
        CreatureFrozenHistory his = new CreatureFrozenHistory();
        his._frozen = b;
        his._pos = get_position();
        his._dir = get_dir();
        MolMoveHistory mole_his = get_last_history();
        mole_his._Creature_acts.Add(his);
        set_last_history(mole_his);

    }

    
    public void UpdateHistory()
    {
        if(global_instance.Instance._crash_manager._record._open_record)
        {
            if(_Creature_history.Count == 0 && _current_history == null)
            {
                global_instance.Instance._crash_manager._record._creature_lock = false;
				global_instance.Instance._crash_manager.next_update ();
            }
            if(_Creature_history.Count > 0)
            {
                _current_history = _Creature_history[_Creature_history.Count - 1];
                _Creature_history.RemoveAt(_Creature_history.Count - 1);
            }
            
            if(_current_history != null)
            {
               switch(_current_history.get_hist_type())
                {
                    case CreatureHistory_type.frozen:
                        {
                            CreatureFrozenHistory hist = (CreatureFrozenHistory)_current_history;
                            set_dir(hist._dir);
                            set_position(hist._pos.x, hist._pos.y, hist._pos.z);                            
                            if(hist._frozen)
                            {
                                global_instance.Instance._crash_manager.catch_click(1);
                            }
                            else
                            {
                                global_instance.Instance._crash_manager.catch_click(0);
                            }
                            _current_history = null;
                        }
                        break;
                    case CreatureHistory_type.moveState:
                        {
                            //_state = creature_state                            
                            Vector3 vec = get_position();
                            CreatureMoveHistory hist = (CreatureMoveHistory)_current_history;
                            bool is_end = false;
                            if(hist._move)
                            {
                                switch (hist._dir)
                                {
                                    case dir_move.left:
                                        {
                                            vec.x += _move_speed;
                                            if(vec.x > hist._pos.x)
                                            {
                                                vec.x = hist._pos.x;
                                                is_end = true;
                                            }
                                        }
                                        break;
                                    case dir_move.right:
                                        {
                                            vec.x -= _move_speed;
                                            if(vec.x < hist._pos.x)
                                            {
                                                vec.x = hist._pos.x;
                                                is_end = true;
                                            }
                                        }
                                        break;
                                    case dir_move.back:
                                        {
                                            vec.z -= _move_speed;
                                            if(vec.z < hist._pos.z)
                                            {
                                                vec.z = hist._pos.z;
                                                is_end = true;
                                            }
                                        }
                                        break;
                                    case dir_move.front:
                                        {
                                            vec.z += _move_speed;
                                            if(vec.z > hist._pos.z)
                                            {
                                                vec.z = hist._pos.z;
                                                is_end = true;
                                            }
                                        }
                                        break;
                                }
                            }

                            set_position(vec.x, vec.y, vec.z);
                            set_dir(hist._dir);

                            if(is_end == true)
                            {
                                _current_history = null;
                            }                                                       
                        }
                        break;
                    case CreatureHistory_type.set_pos:
                        {
                            CreaturePosSetHistory hist = (CreaturePosSetHistory)_current_history;
                            set_dir(hist._dir);
                            set_position(hist._pos.x, hist._pos.y, hist._pos.z);
                            _current_history = null;
                        }
                        break;
                }                
            }
        }
    }


    public void NormalUpdate()
    {
        _frame_count++;
        UpdateAlpha();
        bool need_set = false;
        Vector3 vec = get_position();
        Vector3 temp_vec = new Vector3();
        temp_vec = vec;

        if (global_instance.Instance._crash_manager._freezen_creature == false)
        {
            if (_jump_speed > 0)
            {
                _current_fall_speed = _jump_speed;
                _jump_speed = 0;
            }

            _current_fall_speed += _fallspeed;
            vec.y += _current_fall_speed;
            if (global_instance.Instance._crash_manager.is_block_creature(vec.x, vec.y, vec.z) == false)
            {
                _is_in_falldown = true;
                temp_vec = vec;
                need_set = true;
            }
            else
            {
                if (_current_fall_speed > 0)
                {
                    _current_fall_speed = 0;
                }
                else
                {
                    _current_fall_speed = 0;
                    _is_in_falldown = false;
                }
            }
        }
        vec = temp_vec;
        if (_state == creature_state.run)
        {
            switch (_dir)
            {
                case dir_move.left:
                    {
                        vec.x -= _move_speed;
                    }
                    break;
                case dir_move.right:
                    {
                        vec.x += _move_speed;
                    }
                    break;
                case dir_move.back:
                    {
                        vec.z += _move_speed;
                    }
                    break;
                case dir_move.front:
                    {
                        vec.z -= _move_speed;
                    }
                    break;
            }
            need_set = true;
        }

        if (need_set == true)
        {
            if (global_instance.Instance._crash_manager.is_block_creature(vec.x, vec.y, vec.z) == false)
            {
                temp_vec = vec;
            }
            set_position(temp_vec.x, temp_vec.y, temp_vec.z);
			if (_is_in_falldown == true && isNeedRecord())
            {
                update_set_his();
            }
        }

        if (_is_in_falldown == false && _last_state != _state || _is_in_falldown != _last_fallen_state)
        {
			if (isNeedRecord ()) 
			{
				update_move_his();
			}

            
            _last_fallen_state = _is_in_falldown;
            _last_state = _state;
        }

    }
    public void Update()
    {
        if(global_instance.Instance._crash_manager._record._open_record)
        {
            UpdateHistory();
        }
        else
        {
            NormalUpdate();
        }
    }

    protected MolMoveHistory get_last_history()
    {
        MolMoveHistory history = null;
        int count_temp = global_instance.Instance._crash_manager._History._mol_history.Count;
		if (count_temp == 0) {
			history = new MolMoveHistory ();
			global_instance.Instance._crash_manager._History._mol_history.Add (history);
		} 
		else
		{
			history = global_instance.Instance._crash_manager._History._mol_history[count_temp - 1];
			if (history.move_moles.Count != 0 && global_instance.Instance._crash_manager.need_fall_update() == false 
				&& global_instance.Instance._crash_manager._need_play_animation == false) 
			{
				history = new MolMoveHistory ();
				global_instance.Instance._crash_manager._History._mol_history.Add (history);
			}

				

		}
        return history;        
    }

    protected void set_last_history(MolMoveHistory history)
    {
        int count_temp = global_instance.Instance._crash_manager._History._mol_history.Count;
        if(count_temp > 0)
        {
            global_instance.Instance._crash_manager._History._mol_history.RemoveAt(count_temp - 1);
        }
        global_instance.Instance._crash_manager._History._mol_history.Add(history);
    }
    public void set_state(creature_state state)
    {
        //global_instance.Instance._crash_manager._History._mol_history.Count

		if (global_instance.Instance._crash_manager._game_begin == false) 
		{
			return;
		}

        if (state != _state)
        {
            switch(state)
            {
                case creature_state.idle:
                    {
                        play_idle();
                    }
                    break;
                case creature_state.run:
                    {
                        play_run();
                    }
                    break;
                case creature_state.special:
                    {
                        play_special();
                    }
                    break;
                case creature_state.wound:
                    {
                        play_wound();
                    }
                    break;
            }
            _state = state;
        }
		if (isNeedRecord ()) 
		{
			MolMoveHistory his = get_last_history();
			CreatureMoveHistory creature_his = new CreatureMoveHistory();
			creature_his._dir = get_dir();
			if(_state == creature_state.idle)
			{
				creature_his._move = false;
			}
			else
			{
				creature_his._move = true;
			}
			creature_his.fram_index = _frame_count;
			creature_his._pos = get_position();
			his._Creature_acts.Add(creature_his);
			set_last_history(his);
		}

    }

    public dir_move get_dir()
    {
        return _dir;
    }
    public void set_dir(dir_move dir)
    {
        if(_dir != dir)
        {
            float y_r = 0.0f;
            switch (dir)
            {
                case dir_move.back:
                    {
                        y_r = 333;
                    }
                    break;
                case dir_move.front:
                    {
                        y_r = 160;
                    }
                    break;
                case dir_move.right:
                    {
                        y_r = 70;
                    }
                    break;
                case dir_move.left:
                    {
                        y_r = 250;
                    }
                    break;
            }

            int length = _moles_list.Count;
            for (int i = 0; i < length; i ++ )
            {
                Vector3 vc = new Vector3();
                vc = _moles_list[i].transform.rotation.eulerAngles;
                vc.y = y_r;
                Quaternion a_temp = _moles_list[i].transform.rotation;
                a_temp.eulerAngles = vc;
                _moles_list[i].transform.rotation = a_temp;                
            }
			if (isNeedRecord()) 
			{
				update_set_his();
				update_move_his();	
			}

        }
        _dir = dir;

    }

	public bool isNeedRecord()
	{
		if (global_instance.Instance._crash_manager._game_begin == true && global_instance.Instance._crash_manager._record._open_record == false) 
		{
			return true;
		}
		return false;
	}

    public void update_set_his()
    {
		if (global_instance.Instance._crash_manager._game_begin == false) 
		{
			return;
		}
			
        CreaturePosSetHistory his = new CreaturePosSetHistory();
        his._dir = _dir;
        his._pos = get_position();
        MolMoveHistory mole_his = get_last_history();
        mole_his._Creature_acts.Add(his);
        set_last_history(mole_his);
    }

    public void update_move_his()
    {
		if (isNeedRecord()) 
		{
			return;
		}

        CreatureMoveHistory hist_move_hist = new CreatureMoveHistory();
        hist_move_hist._pos = get_position();
        hist_move_hist._dir = get_dir();

        switch (_state)
        {
            case creature_state.run:
                {
                    hist_move_hist._move = true;
                }
                break;
            case creature_state.idle:
                {
                    hist_move_hist._move = false;
                }
                break;
        }
        MolMoveHistory hist = get_last_history();
        hist._Creature_acts.Add(hist_move_hist);
        set_last_history(hist);

    }

    public void set_creature_type(creature_type type)
    {
        _cureent_type = type;
        foreach(var entry in _moles)
        {
            GameObject obj_entry = entry.Value;
            if(entry.Key == _cureent_type)
            {
                obj_entry.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                obj_entry.SetActive(true);
            }
            else
            {
                obj_entry.SetActive(false);
                obj_entry.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    public void OnCollisionEnter(Collision obj)
    {
    }
    public void OnCollisionExit(Collision obj)
    {

    }

    public void play_idle()
    {
        play_animation((int)creature_state.idle);
    }

    public void play_run()
    {
        play_animation((int)creature_state.run);
    }

    public void play_attack()
    {
        play_animation((int)creature_state.attack);
    }

    public void play_special()
    {
        play_animation((int)creature_state.special);
    }

    public void play_wound()
    {
        play_animation((int)creature_state.wound);
    }

    protected void play_animation(int a)
    {
        foreach (var entry in _moles)
        {
            GameObject obj_entry = entry.Value;
            Animator temp = obj_entry.GetComponent<Animator>();            
            temp.SetInteger("State", a);            
        }
    }

    protected creature_type _cureent_type;
    protected Vector3 _current_position = new Vector3();
    protected dir_move _dir;
    protected creature_state _state;
    protected creature_state _last_state;
    protected bool _last_fallen_state;
    protected int _frame_count;
    public List<CreatureHistory> _Creature_history = new List<CreatureHistory>();
    public CreatureHistory _current_history;
}
