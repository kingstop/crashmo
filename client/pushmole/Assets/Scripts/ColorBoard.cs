using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorBoard : MonoBehaviour {
    public color_button[] ColorButtons_;
    public int _current_group;
	// Use this for initialization
    void Awake()
    {
        foreach(color_button entry in ColorButtons_)
        {
            entry.setParent(this);
        }
    }
	void Start () {
        _current_group = 0;
        OnButtonClick(ColorButtons_[0]);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetButtonGroupColor(int index, int group, Color color)
    {
        ColorButtons_[index].setColor(color);
        ColorButtons_[index].setGroup(group);
    }
    public void SetCount(int index, int count)
    {
        ColorButtons_[index].setCount(count);
    }

    public void SetText(int index, string s)
    {
        ColorButtons_[index].setText(s);
    }

    public void OnButtonClick(color_button entry)
    {
		int i = 0;
        foreach(color_button entry_temp in ColorButtons_)
        {
            bool b_ret = false;
            if(entry == entry_temp)
            {
                _current_group = entry_temp.getGroup();
				global_instance.Instance._current_color = entry_temp.getColor();
				global_instance.Instance._current_group = i;
                b_ret = true;
            }
            entry_temp.setSelect(b_ret);
			i++;
        }
    }
}
