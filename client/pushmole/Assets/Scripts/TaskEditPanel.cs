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

            }
        }
        
    }


	public void onDeleteClick()
	{
		
	}

	public void OnSaveClick()
	{

	}

}
