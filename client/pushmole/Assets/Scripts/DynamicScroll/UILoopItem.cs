using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UILoopItem : MonoBehaviour {


	public Text text;
	private int lastIndex = -1;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void UpdateItem(int index,GameObject item)
	{

	}

	public void Data(object o)
	{
		int x = (int)o;
		if (lastIndex == x)
			return;

        ResetItem(x);


    }

    protected virtual void ResetItem( int index ) 
    {
        lastIndex = index;
        print("Update " + index);
        text.text = index + 1 + "";
    }

}
