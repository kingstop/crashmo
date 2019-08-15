using System;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {
    public delegate void CALL_BACK(ulong index);
    private string _text_1;
	private string _text_2;
	private ulong _map_index;
    private CALL_BACK _click_call_back;
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

    public void setCallBack(CALL_BACK back)
    {
        _click_call_back = back;
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
