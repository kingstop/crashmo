using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class task_item_entry : MonoBehaviour {
    protected TaskEditPanel _parent;
    protected Image _image;
    protected int _id;
    protected string _name;
    public Text _txt;

    void Awake( )
    {
        _image = this.gameObject.GetComponent<Image>();
    }
    public void setParent(TaskEditPanel parent)
    {
        _parent = parent;
    }

    public void OnClick()
    {
		_parent.OnTaskItemClick (this);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void select()
    {
        _image.color = Color.green;
    }
    public void unselect()
    {
        _image.color = Color.white;
    }
    public void setItem(int id, string name)
    {
        _id = id;
        _name = name;
        _txt.text = id.ToString() + "-" + _name;
    }

    

    public int getID()
    {
        return _id;
    }
    public string getName()
    {
        return _name;
    }
}
