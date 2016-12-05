using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISectionGiftGold : MonoBehaviour {
    public InputField _gold;
    protected int _count;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetGoldCount(int count)
    {
        _gold.text = count.ToString();
        _count = count;
    }

    public int GetGoldCount()
    {
        return _count;
    }

    
}
