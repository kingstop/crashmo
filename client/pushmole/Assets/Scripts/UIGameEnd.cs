using UnityEngine;
using System.Collections;
public enum enResult
{
    win,
    failed
}
public class UIGameEnd : MonoBehaviour {
    public RewardEntry[] rewardEntrys_;
	public GameObject objEdit_;
	public GameObject objGame_;

    protected int _current_count = 0;
    protected enResult _en;
    // Use this for initialization
    void Start () {
			
		global_instance.Instance._ngui_edit_manager.HideAllUIBut(this.gameObject);
		switch (global_instance.Instance._global_game_type) 
		{
			case global_game_type.global_game_type_game:
			{
				objEdit_.SetActive (false);
				objGame_.SetActive (true);
			}
			break;

			case global_game_type.global_game_type_edit:
			{
				objEdit_.SetActive (true);
				objGame_.SetActive (false);
			}
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AddRewardCount(int group, int count)
    {
        rewardEntrys_[_current_count].setColor(global_instance.Instance.get_color_by_group(group));
        rewardEntrys_[_current_count].setCount(count);
        rewardEntrys_[_current_count].gameObject.SetActive(true);
        _current_count++;
    }
    public void setType(enResult en)
    {
        _en = en;
        /*
        foreach (RewardEntry entry in rewardEntrys_)
        {
            if (_en == enResult.failed)
            {
                entry.gameObject.SetActive(false);
            }
            else
            {
                entry.gameObject.SetActive(false);
            }
       }
       */
    }

    public void clear()
    {
        _current_count = 0;
        foreach (RewardEntry entry in rewardEntrys_)
        {
            entry.gameObject.SetActive(false);
        }
    }

	public void OnSaveClick()
	{
		this.gameObject.SetActive (false);
		global_instance.Instance._ngui_edit_manager.on_save_btn_click ();
	}

	public void OnModifyClick()
	{
		this.gameObject.SetActive (false);
		global_instance.Instance._ngui_edit_manager.update_game_type (game_type.edit);
	}

	public void OnGiveUpClick()
	{
		global_instance.Instance._ngui_edit_manager.BackToMainPanel ();
	}

	public void OnCompleteClick()
	{
		global_instance.Instance._ngui_edit_manager.BackToMainPanel ();
	}
}
