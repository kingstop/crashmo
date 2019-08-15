using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WobbleObjState
{
    init,
    update,
    stop
}

public class WobbleObj
{
    private float _wobble;
    private GameObject _obj;
    private float _base_range;
    private float _random_range;
    private bool _left_able;
    private float _wobble_speed;
    private float _wobble_random_speed;
    private float _cur_speed;
    private float _cur_wobble;
    private float _shrink_wobble;
    private float _random_shrink_wobble;
    private float _last_woble;
    private float _target_woble;
    private WobbleObjState _state;

    public void Create(GameObject obj, float base_range, float random_range, bool left_able,
        float wobble_speed, float wobble_random_speed, float shrink_wobble, float random_shrink_wobble)
    {
        _obj = obj;
        _wobble = obj.transform.localEulerAngles.z;
        _base_range = base_range;
        _random_range = random_range;
        _left_able = left_able;
        _wobble_speed = wobble_speed;
        _wobble_random_speed = wobble_random_speed;
        _shrink_wobble = shrink_wobble;
        _random_shrink_wobble = random_shrink_wobble;
        _state = WobbleObjState.init;
        
    }

    public WobbleObjState getState()
    {
        return _state;
    }

    public void Init()
    {
        float woble = _base_range + Random.Range(0, _random_range);
        if(_left_able)
        {
            int temp = Random.Range(0, 1);
            if(temp != 0)
            {
                woble = -woble;
            }            
        }
        _cur_speed = _wobble_speed + Random.Range(0, _wobble_random_speed);
        float temp_woble = woble + _wobble;
        _target_woble = temp_woble;
        _last_woble = temp_woble;
        setWoble(temp_woble);
        AriveTarget();
        _state = WobbleObjState.update;
    }

    void Stop()
    {
        setWoble(_wobble);
        _state = WobbleObjState.stop;
    }

    void AriveTarget()
    {
        if(Mathf.Abs(_last_woble - _wobble) < 1f)
        {
            Stop();
        }
        else
        {
            float woble = Mathf.Abs(_last_woble) - _shrink_wobble - Random.Range(0, _random_shrink_wobble);   
            
            _last_woble = _target_woble;
            if (woble <= 0)
            {
                _target_woble = _wobble;
            }
            else
            {
                if (_last_woble > _wobble)
                {
                    _target_woble = _wobble - woble;
                }
                else
                {
                    _target_woble = _wobble + woble;
                }
            }
        }                      
    }

    void setWoble(float woble)
    {
        Vector3 vec = _obj.transform.localEulerAngles;
        vec.z = woble;
        _obj.transform.localEulerAngles = vec;        　　      
    }

    public void update()
    {
        if(_state == WobbleObjState.update)
        {
            Vector3 vec = _obj.transform.localEulerAngles;
            float woble = vec.z;

            if(woble > 180)
            {
                woble = woble - 360;
            }
            if (woble > _target_woble)
            {
                woble -= _cur_speed;
                if (woble < _target_woble)
                {
                    AriveTarget();
                }
                else
                {
                    setWoble(woble);
                }
            }
            else
            {
                woble += _cur_speed;
                if (woble > _target_woble)
                {
                    AriveTarget();
                }
                else
                {
                    setWoble(woble);
                }
            }
        }
        
       // vec.z += _cur_speed;
        //_obj.transform.rotation.SetEulerAngles(vec);// (vec.x, vec.y, vec.z);        
    }
}

public class UIAnimationComponent : MonoBehaviour {
    public GameObject[] UpObjs_;
    public GameObject[] LeftObjs_;
    public GameObject[] RightObjs_;
    public GameObject[] ScaleXObjs_;
    public GameObject[] ScaleYObjs_;
    public GameObject[] WobleObjs_;
    protected float _speed = 30.0f;
    protected float _scale_speed = 0.3f;
    protected Dictionary<GameObject, Vector3> _UpDicObjVec3 = new Dictionary<GameObject, Vector3>();
    protected Dictionary<GameObject, Vector3> _LeftDicObjVec3 = new Dictionary<GameObject, Vector3>();
    protected Dictionary<GameObject, Vector3> _RightDicObjVec3 = new Dictionary<GameObject, Vector3>();
    protected List<WobbleObj> _WobleObjs = new List<WobbleObj>();
    
    protected bool _move_ready;
    protected bool _scale_ready;
    protected bool _woble_ready;
    public delegate void OK_CALL_BACK();
    private OK_CALL_BACK OK_function_ = null;
    // Use this for initialization

    void Start () {
		
	}
	
    void OnMoveReady()
    {
        _move_ready = true;
    }

    void OnScaleReady()
    {
        _scale_ready = true;
        if(OK_function_ != null)
        {
            OK_function_();
        }
    }

    void OnWobleReady()
    {
        _woble_ready = true;
    }

    public void setOK(OK_CALL_BACK ok)
    {
        OK_function_ = ok;
    }
	// Update is called once per frame
	void Update () {
        if(_move_ready == false)
        {
            bool already = true;
            foreach (KeyValuePair<GameObject, Vector3> pair_entry in _UpDicObjVec3)
            {
                GameObject obj = pair_entry.Key;
                Vector3 vec_pos = pair_entry.Value;
                Vector3 vec = obj.transform.localPosition;
                float temp_y = vec.y - _speed;
                if (temp_y < vec_pos.y)
                {
                    temp_y = vec_pos.y;
                }
                else
                {
                    already = false;
                }
                vec.y = temp_y;
                obj.transform.localPosition = vec;
            }
            foreach (KeyValuePair<GameObject, Vector3> pair_entry in _LeftDicObjVec3)
            {
                GameObject obj = pair_entry.Key;
                Vector3 vec_pos = pair_entry.Value;
                Vector3 vec = obj.transform.localPosition;
                float temp_x = vec.x + _speed;
                if (temp_x > vec_pos.x)
                {
                    temp_x = vec_pos.x;
                }
                else
                {
                    already = false;
                }
                vec.x = temp_x;
                obj.transform.localPosition = vec;
            }

            foreach (KeyValuePair<GameObject, Vector3> pair_entry in _RightDicObjVec3)
            {
                GameObject obj = pair_entry.Key;
                Vector3 vec_pos = pair_entry.Value;
                Vector3 vec = obj.transform.localPosition;
                float temp_x = vec.x - _speed;
                if (temp_x < vec_pos.x)
                {
                    temp_x = vec_pos.x;
                }
                else
                {
                    already = false;
                }
                vec.x = temp_x;
                obj.transform.localPosition = vec;
            }

            if (already)
            {
                OnMoveReady();
            }
        }

        if(_move_ready && _scale_ready == false)
        {
            bool scale_already = true;
            float scale = 0;
            foreach(GameObject obj in ScaleXObjs_)
            {
                scale = obj.transform.localScale.x + _scale_speed;
                if(scale > 1)
                {
                    scale = 1;
                }
                else
                {
                    scale_already = false;
                }
                Vector3 vex = new Vector3(scale, obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.localScale = vex;
            }

            foreach (GameObject obj in ScaleYObjs_)
            {
                scale = obj.transform.localScale.y + _scale_speed;
                if (scale > 1)
                {
                    scale = 1;
                }
                else
                {
                    scale_already = false;
                }

                Vector3 vex = new Vector3(obj.transform.localScale.x, scale, obj.transform.localScale.z);
                obj.transform.localScale = vex;               
            }

            if(scale_already)
            {
                OnScaleReady();
            }
        }
        if(_scale_ready&&_woble_ready == false)
        {
            bool woble_already = true;
            foreach (WobbleObj obj in _WobleObjs)
            {
                obj.update();

                if(obj.getState() != WobbleObjState.stop)
                {
                    woble_already = false;
                }                
            }

            if(woble_already)
            {
                OnWobleReady();
            }            
        }
    }

    void Awake()
    {
        _speed = 30f;
        _scale_speed = 0.05f;
        foreach (GameObject obj in UpObjs_)
        {
            _UpDicObjVec3[obj] = obj.transform.localPosition;
        }


        foreach (GameObject obj in LeftObjs_)
        {
            _LeftDicObjVec3[obj] = obj.transform.localPosition;
        }

        foreach (GameObject obj in RightObjs_)
        {
            _RightDicObjVec3[obj] = obj.transform.localPosition;
        }


        foreach(GameObject obj in WobleObjs_)
        {
            WobbleObj temp = new WobbleObj();
            temp.Create(obj, 40, 10, true,  10f, 0f, 4, 1);
            _WobleObjs.Add(temp);

        }
    }


    void OnEnable()
    {
        Vector3 vex = new Vector3(0, 1, 1);
        foreach (GameObject obj in ScaleXObjs_)
        {
            obj.transform.localScale = vex;
        }

        Vector3 vex1 = new Vector3(1, 0, 1);
        foreach (GameObject obj in ScaleYObjs_)
        {
            obj.transform.localScale = vex1;
        }

        foreach (KeyValuePair<GameObject, Vector3> pair_entry in _UpDicObjVec3) 
        {
            GameObject obj = pair_entry.Key;
            Vector3 vec = obj.transform.localPosition;
            vec.y = 600;
            obj.transform.localPosition = vec;
        }

        foreach(KeyValuePair<GameObject, Vector3> pair_entry in _LeftDicObjVec3)
        {
            GameObject obj = pair_entry.Key;
            Vector3 vec = obj.transform.localPosition;
            vec.x = -1200;
            obj.transform.localPosition = vec;
        }

        foreach (KeyValuePair<GameObject, Vector3> pair_entry in _RightDicObjVec3)
        {
            GameObject obj = pair_entry.Key;
            Vector3 vec = obj.transform.localPosition;
            vec.x = UnityEngine.Screen.width + 1200;
            obj.transform.localPosition = vec;
        }

        foreach(WobbleObj obj in _WobleObjs)
        {
            obj.Init();
        }
        _move_ready = false;
        _scale_ready = false;
        _woble_ready = false;
    }
}
