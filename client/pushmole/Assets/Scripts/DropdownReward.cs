using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DropdownReward : MonoBehaviour {
	public Dropdown dropdown_reward_type_;
	public Text argu_title_;
	public InputField argu_;
	// Use this for initialization
	void Awake()
	{
		dropdown_reward_type_.ClearOptions ();
		List<string> list_str = new List<string> ();
		for (int i = (int)message.ResourceType.ResourceType_NULL ; i < (int)message.ResourceType.ResourceType_Max; i++) 
		{
			message.ResourceType type_en = (message.ResourceType)i;
			list_str.Add (type_en.ToString());
		}
		dropdown_reward_type_.AddOptions (list_str);
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetInfo(message.TaskRewardConfig info)
	{
		dropdown_reward_type_.value = (int)info.resource_type;
		argu_.text = info.count.ToString ();
	}

	public message.TaskRewardConfig GetInfo()
	{
		message.TaskRewardConfig info = new message.TaskRewardConfig ();
		info.count = 0;
		info.resource_type = (message.ResourceType)dropdown_reward_type_.value;
		int count = 0;
		int.TryParse (argu_.text, out count);
		info.count = count;
		return info;			
	}

}
