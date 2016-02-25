using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainPanelTitleButtonItem : MonoBehaviour {
	public Image _select_image;

	public void Select()
	{
		_select_image.gameObject.SetActive (true);
	}

	public void Unselect()
	{
		_select_image.gameObject.SetActive (false);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
