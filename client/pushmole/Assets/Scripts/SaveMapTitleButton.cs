using UnityEngine;
using UnityEngine.UI;

public class SaveMapTitleButton : MonoBehaviour {
    public Image _sel_image;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void select()
    {
        _sel_image.gameObject.SetActive(true);
    }
    public void unselect()
    {
        _sel_image.gameObject.SetActive(false);
    }
}
