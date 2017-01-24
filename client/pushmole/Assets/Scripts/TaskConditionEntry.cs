using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class TaskConditionEntry : MonoBehaviour {
	public Dropdown dropdown_;
	public InputField argument_1_;
	public InputField argument_2_;
	public Text text_1_;
	public Text text_2_;
    void Awake()
    {
        List<string> str_list = new List<string>();
        
        dropdown_.ClearOptions();
        
        //dropdown_.AddOptions()
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
