using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScaleComponent : MonoBehaviour {
    private WrapContent _Content;
    private Vector2 _interval;
    private Vector2 _size;
    private Vector3 _pos;
	private bool _center = false;
	private bool _scale_modify = false;
	private float _scale;
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
			if (_center == true) {				
				//this.transform.gameObject
				Vector3 cur_pos = Camera.main.WorldToScreenPoint(this.transform.position);
				Debug.LogWarning ("center pos [ " + cur_pos.x + "," + cur_pos.y + "," + cur_pos.z + " ]");
				Vector2 offset = _Content.GetOffset ();
				if (cur_pos.x >- 3400f && cur_pos.x < -1714f) {
					float dis = offset.x * offset.x + offset.y * offset.y;
					dis = Mathf.Sqrt (dis);
					int offset_x = (int)(offset.x);
					float offset_entry = offset.x - offset_x;
					float offset_scale = Mathf.Abs (offset_entry - 0.4f);
					offset_scale = 1.6f / (offset_scale + 1.0f);
					_scale = offset_scale;

					if (offset_scale > 1.3f) {
						offset_scale = 1.3f;
					}
					float move_x = (_size.x - offset_scale * _size.x) / 2;
					float move_y = (_size.y - offset_scale * _size.y) / 2;
					this.transform.localScale = new Vector3 (offset_scale, offset_scale, 1);
					this.transform.localPosition = new Vector3 (_pos.x - move_x, _pos.y - move_y, 1);
					if (_scale_modify == false) {
						_scale_modify = true;
					}
				}
			}
			else 
			{

				if (_scale_modify) 
				{
					Reset ();
					_scale_modify = false;
				}
			}
        }        
    }

	public float getScale()
	{
		return _scale;
	}


	public void Reset()
	{
		Vector3 vect_scale = new Vector3(1, 1, 1);
		this.transform.localScale = vect_scale;
		this.transform.localPosition = _pos;
		_scale = 1.0f;
	}

	public void setCenter(bool center)
	{
		_center = center;
	}

    void OnDisable()
    {
		if (_scale_modify) 
		{
			Reset ();
		}
    }
    public void SetContent(WrapContent _content)
    {
        _Content = _content;
        _size = _content.GetGridSize();
        _interval = _content.GetInterval();
    }

	public void SetGameObj(GameObject obj)
	{
		_obj = obj;
	}

}
