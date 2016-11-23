using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorBoard : MonoBehaviour {
    public color_button[] ColorButtons_;
    public int _current_group;
	// Use this for initialization
	void Start () {
        _current_group = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetButtonGroupColor(int index, int group, Color color)
    {
        ColorButtons_[index].setColor(color);
        ColorButtons_[index].setGroup(group);
    }

    public void OnButtonClick(color_button entry)
    {
        foreach(color_button entry_temp in ColorButtons_)
        {
            bool b_ret = false;
            if(entry == entry_temp)
            {
                _current_group = entry_temp.getGroup();
                b_ret = true;
            }
            entry.setSelect(b_ret);
        }
    }
}
