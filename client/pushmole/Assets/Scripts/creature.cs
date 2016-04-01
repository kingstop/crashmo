using UnityEngine;
using System.Collections;
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
            //Vector3 vc = new Vector3(0, 0, 0);
            //obj_entry.gameObject.transform.position = vc;

        }
                
       // _Rigidbody = GetComponent<Rigidbody>();
        set_dir(dir_move.front);
        set_state(creature_state.idle);
        _is_in_falldown = false;

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
        //if(_Rigidbody.velocity.y > 0)
        //{
        //    return;
        //}
        //else
        //{
        //    Vector3 vc = new Vector3(0, 4.8f, 0);
        //    _Rigidbody.velocity = vc;
        //}
        
        _jump_speed = 0.6f;
    }
    public void set_position(float x, float y, float z)
    {
        Vector3 vc = new Vector3(x, y, z);

        this.transform.position = vc;
        vc.y -= 0.6f;
        vc.x -= 0.28f;
        vc.z -= 0.2f;
        Vector3 vec = new Vector3(0, 0, 0);
        foreach (var entry in _moles)
        {
            GameObject obj_entry = entry.Value;

            obj_entry.transform.position = vc;
        }
        global_instance.Instance._crash_manager.update_camera();

      //  _creature_physics.transform.position = _current_position;
    }

    public Vector3 get_position()
    {
        Vector3 vec = new Vector3();
        vec = this.transform.position;
        //vec.y += 0.6f;
        //vec.x += 0.28f;
        //vec.z += 0.2f;
        // vec.y += 0.5f;
        return vec;
    }


    Vector3  FallDown()
    {
        Vector3 vec_temp = new Vector3();
        Vector3 vec = get_position();
        vec_temp = vec;
        if (global_instance.Instance._crash_manager._freezen_creature == false)
        {            
            if(_jump_speed > 0)
            {
                _current_fall_speed = _jump_speed;
                _jump_speed = 0;
            }
            
            _current_fall_speed += _fallspeed;
            vec.y += _current_fall_speed;
            if(global_instance.Instance._crash_manager.is_block_creature(vec.x, vec.y, vec.z) == false)
            {
                //set_position(vec.x, vec.y, vec.z);
                vec_temp = vec;
                _is_in_falldown = true;
            }
            else
            {
                if(_current_fall_speed > 0)
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
        return vec_temp;
    }
    public void Update()
    {
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
            if (_state == creature_state.run)
            {
                Debug.Log("RUN");
            }
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
            //if(vec.y < 0)
            //{
            //    vec.y = 0;            
            //}
            //if(vec.x < -4)
            //{
            //    vec.x = -4;
            //}
            //if(vec.x > 24)
            //{
            //    vec.x = 24;
            //}
            //if(vec.z < 3)
            //{
            //    vec.z = 3;
            //}
            //if(vec.z > 17)
            //{
            //    vec.z = 17;
            //}
            need_set = true;                        
        }
        else
        {
            Debug.Log("is idle");
        }

        if (need_set == true)
        {
            if(global_instance.Instance._crash_manager.is_block_creature(vec.x, vec.y, vec.z) == false)
            {
                temp_vec = vec;                
            }

            set_position(temp_vec.x, temp_vec.y, temp_vec.z);


        }
    }

    
    public void set_state(creature_state state)
    {
        if(state != _state)
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
            if(_state == creature_state.idle)
            {
                Debug.Log("idle");
            }
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
        }
        _dir = dir;

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
        //set_state(creature_state.idle);
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

}
