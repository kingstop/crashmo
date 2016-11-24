using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class color_button : MonoBehaviour {
    public Button Sliced_;
    public Button Simple_;
    public Text Text_;
    protected bool _select = false;
    protected Color _color = new Color();
    protected int _group;
    protected ColorBoard _parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setSelect(bool b)
    {

    }
    public void setColor(Color c)
    {
        _color = c;
        Sliced_.GetComponent<Image>().color = _color;
        Simple_.GetComponent<Image>().color = _color;
    }


    public void setGroup(int group)
    {
        _group = group;
    }

    public int getGroup()
    {
        return _group;
    }

	public Color getColor()
	{
		return _color;	
	}

    public void OnButtonClick()
    {
        _parent.OnButtonClick(this);
    }
    public void setCount(int i)
    {
        Text_.text = i.ToString();
    }


}
