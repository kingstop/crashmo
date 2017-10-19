using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIAnimationComponent : MonoBehaviour {
    public GameObject[] UpObjs_;
    public GameObject[] LeftObjs_;
    public GameObject[] ScaleXObjs_;
    public GameObject[] ScaleYObjs_;
    protected float _speed;
    protected float _scale_speed;
    protected Dictionary<GameObject, Vector3> _UpDicObjVec3 = new Dictionary<GameObject, Vector3>();
    protected Dictionary<GameObject, Vector3> _LeftDicObjVec3 = new Dictionary<GameObject, Vector3>();
    protected bool _move_ready;
    protected bool _scale_ready;
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
            if(already)
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
        _move_ready = false;
        _scale_ready = false;
    }
}
