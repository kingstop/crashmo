using UnityEngine;
using UnityEngine.UI;

public class ChooseItemEntry : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void unselect()
    {
        Image temp = GetComponent<Image>();
        temp.color = new Color(255, 255, 255);
    }
     
    public void select()
    {
        Image temp = GetComponent<Image>();
        temp.color = new Color(74, 161, 184);
    }

    public void OnButtonClick()
    {
        global_instance.Instance._ngui_edit_manager._main_panel.ItemButtonClick(this);
    }

    public void SetTexture(Texture t)
    {
        Material material_temp = new Material(_image.material);
        if(material_temp != null)
        {
            material_temp.mainTexture = t;
            _image.material = material_temp;            
        }
    }
    public Text _txt_1;
    public Text _txt_2;
    public Text _txt_3;
    public Image _image;
    public ulong _map_index;
}
