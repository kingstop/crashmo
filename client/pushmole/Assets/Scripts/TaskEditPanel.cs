using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TaskEditPanel : MonoBehaviour {
	public InputField name_;
	public InputField id_;
	public InputField describe_;
	public TaskConditionEntry[] drop_down_condition_entrys_;
	public DropdownReward[] drop_down_rewards_;
    public InputField required_chapter_id_;
    public InputField required_section_id_;
    public InputField required_complete_count_;
    public GameObject obj_container_;
    protected List<task_item_entry> _item_entrys = new List<task_item_entry>();
    protected GameObject _source_item;
    void Awake()
    {
        _source_item = Resources.Load<GameObject>("prefab/TaskItem");
    }
    

    public void ClearAll()
    {
        foreach(task_item_entry entry in _item_entrys)
        {
            entry.setParent(null);
            entry.transform.SetParent(null);
        }
        _item_entrys.Clear();
    }

    // Use this for initialization
    void Start () {
        ClearAll();
        if(global_instance.Instance._taskManager.GetTaskManagerState() == enTaskLoadState.LOADED)
        {
            foreach(KeyValuePair<int, message.TaskInfoConfig> pair_entry in global_instance.Instance._taskManager._tasks)
            {
                GameObject obj = GameObject.Instantiate<GameObject>(_source_item);
                obj.transform.SetParent(obj_container_.transform);
                task_item_entry item_entry = obj.GetComponent<task_item_entry>();
                item_entry.setParent(this);
                item_entry.setItem(pair_entry.Key, pair_entry.Value.name);
                _item_entrys.Add(item_entry);
            }
			if (_item_entrys.Count != 0) 
			{
				OnTaskItemClick (_item_entrys [0]);
			}            
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void LoadTask()
    {

    }
    public void OnTaskItemClick(task_item_entry entry)
    {
        int click_id = 0;
        foreach(task_item_entry temp_entry in _item_entrys)
        {
            if(temp_entry == entry)
            {
                temp_entry.select();
                click_id = temp_entry.getID();
            }
            else
            {
                temp_entry.unselect();
            }
        }
        if(global_instance.Instance._taskManager._tasks.ContainsKey(click_id) == true)
        {
            message.TaskInfoConfig info_entry = global_instance.Instance._taskManager._tasks[click_id];
            if(info_entry != null)
            {
				for (int i = 0; i < 3; i++) 
				{
					drop_down_condition_entrys_ [i].SetInfo (info_entry.conditions[i]);
					drop_down_rewards_ [i].SetInfo (info_entry.rewards[i]);
				}
				name_.text = info_entry.name;
				id_.text = info_entry.task_id.ToString();
				describe_.text = info_entry.describe;
				required_chapter_id_.text = info_entry.required_pass_chapter_id.ToString ();
				required_section_id_.text = info_entry.required_pass_section_id.ToString ();
				required_complete_count_.text = info_entry.required_complete_task_count.ToString ();
            }
        }
        
    }

	public message.TaskInfoConfig GetInfo()
	{
		message.TaskInfoConfig info = null;
		int id = 0;
		int.TryParse (id_.text, out id);
		if (id != 0) 
		{
			info = new message.TaskInfoConfig ();
			info.name = name_.text;
			info.task_id = id;
			info.describe = describe_.text;
			int required_chapter_id = 0;
			int required_section_id = 0;
			int required_complete_count = 0;
			int.TryParse (required_chapter_id_.text, out required_chapter_id);
			int.TryParse (required_section_id_.text, out required_section_id);
			int.TryParse (required_complete_count_.text, out required_complete_count);
			info.required_pass_chapter_id = required_chapter_id;
			info.required_pass_section_id = required_section_id;
			info.required_complete_task_count = required_complete_count;
			for (int i = 0; i < 3; i++) 
			{
				info.conditions.Add (drop_down_condition_entrys_[i].GetInfo());
				info.rewards.Add (drop_down_rewards_ [i].GetInfo ());
			}
		}
		return info;
	}


	public void onDeleteClick()
	{
		
	}

	public void OnSaveClick()
	{
		message.TaskInfoConfig cur_info = GetInfo ();
		if (cur_info != null) 
		{
			message.MsgC2SReqModifyTaskInfo msgReq = new message.MsgC2SReqModifyTaskInfo ();
			msgReq.info = cur_info;
			bool ret = false;
			global_instance.Instance._net_client.send (msgReq);
			foreach (task_item_entry entry in _item_entrys) 
			{
				if (entry.getID () == cur_info.task_id) 
				{
					entry.setItem (cur_info.task_id, cur_info.name);
					ret = true;
					break;
				}
			}
			if (ret == false) 
			{
				GameObject obj = GameObject.Instantiate<GameObject>(_source_item);
				obj.transform.SetParent(obj_container_.transform);
				task_item_entry item_entry = obj.GetComponent<task_item_entry>();
				item_entry.setParent(this);
				item_entry.setItem(cur_info.task_id, cur_info.name);
				_item_entrys.Add(item_entry);
			}
			global_instance.Instance._taskManager._tasks [cur_info.task_id] = cur_info;
		}			
	}

}
