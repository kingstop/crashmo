using UnityEngine;
using System.Collections;
public enum enResult
{
    win,
    failed
}
public class UIGameEnd : MonoBehaviour {
    public RewardEntry[] rewardEntrys_;
    protected enResult _en;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setType(enResult en)
    {
        _en = en;
        foreach (RewardEntry entry in rewardEntrys_)
        {
            if (_en == enResult.failed)
            {
                entry.gameObject.SetActive(false);
            }
            else
            {
                entry.gameObject.SetActive(true);
            }
       }
    }
}
