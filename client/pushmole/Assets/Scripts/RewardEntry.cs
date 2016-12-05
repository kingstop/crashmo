using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RewardEntry : MonoBehaviour {
    public Image image_;
    public Text count_;
	// Use this for initialization
    
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setColor(Color color)
    {
        image_.color = color;
    }

    public void setCount(int count)
    {
        count_.text = count.ToString();
    }
}
