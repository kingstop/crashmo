using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskRewards : MonoBehaviour {

    public RewardEntry[] _rewards;
    public Text _task_name;
    public Text _gold;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTaskName(string name)
    {
        _task_name.text = name;
    }

    public void SetGold(int gold)
    {
        _gold.text = gold.ToString();
    }

    public void SetResourceCount(int i, int type, int count)
    {
        if(i < 4)
        {
            Color col = global_instance.Instance.get_color_by_group(type);

            _rewards[i].setColor(col);
            _rewards[i].setCount(count);
        }
    }
}
