using System;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {
	private string _text_1;
	private string _text_2;
	private ulong _map_index;
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

	public Image _image;
}
