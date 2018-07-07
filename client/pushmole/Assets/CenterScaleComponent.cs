using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScaleComponent : MonoBehaviour {
    private WrapContent _Content;
    private Vector2 _interval;
    private Vector2 _size;
    private Vector3 _pos;
	private bool _center = false;
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
			if (_center == true) 
			{
				Vector2 offset = _Content.GetOffset();
				float dis = offset.x * offset.x + offset.y * offset.y;
				dis = Mathf.Sqrt(dis);
				int offset_x = (int)(offset.x);
				float offset_entry = offset.x - offset_x;
				float offset_scale = Mathf.Abs (offset_entry - 0.4f);
				offset_scale =  1.6f/(offset_scale + 1.0f) ;
				float move_x = (_size.x - offset_scale * _size.x) / 2;
				float move_y = (_size.y - offset_scale * _size.y) / 2;
				this.transform.localScale = new Vector3(offset_scale, offset_scale, 1);
				this.transform.localPosition = new Vector3 (_pos.x - move_x, _pos.y - move_y, 1);
				//Vector3 vc_pos = this.transform.TransformPoint (this.transform.localPosition);
				Debug.Log ("vc_pos [ " + offset.x + "," + offset.y + "]");
			}
        }
        
    }
	public void setCenter(bool center)
	{
		_center = center;
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
