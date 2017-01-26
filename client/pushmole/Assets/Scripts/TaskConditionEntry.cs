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
		for (int i = (int)message.ConditionType.ConditionType_NULL; i < (int)message.ConditionType.ConditionType_Max; i++) 
		{
			message.ConditionType conType = (message.ConditionType)i;
			str_list.Add (conType.ToString());
		}
		dropdown_.AddOptions (str_list);
		dropdown_.value = 0;
        
        //dropdown_.AddOptions()
    }

	public void SetInfo(message.TaskConditionTypeConfig info)
	{
		dropdown_.value = (int)info.condition;	
		argument_1_.text = info.argu_1.ToString ();
		argument_2_.text = info.argu_2.ToString ();
	}

	public message.TaskConditionTypeConfig GetInfo()
	{
		message.TaskConditionTypeConfig info = new message.TaskConditionTypeConfig ();
		info.condition = (message.ConditionType)dropdown_.value;
		int argu_1 = 0;
		int argu_2 = 0;
		int.TryParse (argument_1_.text, out argu_1);
		int.TryParse (argument_2_.text, out argu_2);
		info.argu_1 = argu_1;
		info.argu_2 = argu_2;
		return info;
	}
	// Use this for initialization
	void Start () {
	
	}	
	// Update is called once per frame
	void Update () {
	
	}
}
