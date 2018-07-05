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
			float offset_scale = Mathf.Abs (offset.x - 0.4f) * 0.8f;
			float use_scale = (1.0f - offset_scale);
			if (use_scale < 0.6f) 
			{
				use_scale = 0.6f;
			}
			this.transform.localScale = new Vector3(use_scale, use_scale, 1);
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
