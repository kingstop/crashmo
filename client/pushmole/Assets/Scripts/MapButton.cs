using System;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {

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

	public Image _image;
}
