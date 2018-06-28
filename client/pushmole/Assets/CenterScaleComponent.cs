using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScaleComponent : MonoBehaviour {
    private WrapContent _Content;
    private Vector2 _interval;
    private Vector2 _size;
    private Vector3 _pos;
    void Awake()
    {
        _pos = this.transform.localPosition;
    }
    // Use this for initialization
    void Start () {
        _pos = this.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if(_Content != null)
        {
            Vector2 offset = _Content.GetOffset();
            float dis = offset.x * offset.x + offset.y * offset.y;
            dis = Mathf.Sqrt(dis);
            //int temp_x = (int)offset.x;
            

			Debug.LogWarning ("offset x[" + offset.x + "]");
			/*
            if(x > 0.33)
            {
                x = 1.0f - x;
            }
            float current_scale =1 + x;
            float move_x = (_size.x * current_scale - _size.x) / 2;

            float move_y = (_size.y * current_scale - _size.y) / 2;
            Vector3 vect_scale = new Vector3(current_scale, current_scale, 1);
            this.transform.localScale = vect_scale;
            Vector3 new_pos = new Vector3(_pos.x + move_x, _pos.y + move_y, 1.0f);
            this.transform.localPosition = new_pos;
            */
        }
        
    }

    void OnDisable()
    {
        Vector3 vect_scale = new Vector3(1, 1, 1);
        this.transform.localScale = vect_scale;
        this.transform.localPosition = _pos;
    }
    public void SetContent(WrapContent _content)
    {
        _Content = _content;
        _size = _content.GetGridSize();
        _interval = _content.GetInterval();


    }
}
