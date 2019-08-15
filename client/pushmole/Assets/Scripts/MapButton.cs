using System;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {
    public delegate void CALL_BACK(ulong index);
    public delegate float CALL_BACK_EXTRA_X();
    private string _text_1;
	private string _text_2;
	private ulong _map_index;
    private CALL_BACK _click_call_back;
    private CALL_BACK_EXTRA_X _extra_x;
    private float _height;
    private float _move_height;
    private GameObject _parent;
    private void Awake()
    {
        _height = this.gameObject.transform.localPosition.y;
        _move_height = 20;
        //_parent = this.GetComponentsInParent<GameObject>();
    }
    public void SetTexture(Texture t)
	{
		Material material_temp = new Material(_image.material);

		if(material_temp != null)
		{
			material_temp.mainTexture = t;
			_image.material = material_temp;            
		}
		_image.transform.SetParent (this.transform);
	}

    private void Update()
    {

        //float x = (this.GetComponentsInParent<GameObject>(). + this.gameObject.transform.localPosition.x) / 160f ;
        //if (_extra_x != null)
        //{
        //    x = x + _extra_x();
        //}
        //float height = Mathf.Asin(Mathf.Sin(x)) * _move_height + _height + 40;
        //Vector3  pos = this.gameObject.transform.localPosition;
        //pos.y = height;
        //this.gameObject.transform.localPosition = pos;
    }

    public void setCallBack(CALL_BACK back)
    {
        _click_call_back = back;
    }

    public void setExtraXCallBack(CALL_BACK_EXTRA_X call)
    {
        _extra_x = call;
    }

    public void onBtnClick()
    {
        if(_click_call_back != null)
        {
            _click_call_back(_map_index);
        }
    }

	public void setText1(string s)
	{
		_text_1 = s;
	}

	public void setText2(string s)
	{
		_text_2 = s;
	}

	public void setMapIndex(ulong index)
	{
		_map_index = index;
	}

    public ulong getMapIndex()
    {
        return _map_index;
    }

	public Image _image;
}
