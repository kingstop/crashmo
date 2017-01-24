using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TaskEditPanel : MonoBehaviour {
	public InputField name_;
	public InputField id_;
	public InputField describe_;
	public TaskConditionEntry[] drop_down_condition_entrys_;
	public DropdownReward[] drop_down_rewards_;
    public InputField required_chapter_id_;
    public InputField required_section_id_;
    public InputField required_complete_count_;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void onDeleteClick()
	{
		
	}

	public void OnSaveClick()
	{

	}

}
