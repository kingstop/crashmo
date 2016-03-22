using UnityEngine;
using System.Collections;

public class plant_ctrl : MonoBehaviour {
    public GameObject[] _plants;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void random_plant()
    {
        int length = _plants.Length;
        int plant = Random.Range(0, length);
        for(int i = 0; i < length; i ++)
        {
            if(plant == i)
            {
                _plants[i].SetActive(true);
            }
            else
            {
                _plants[i].SetActive(false);
            }
        }

    }
}
