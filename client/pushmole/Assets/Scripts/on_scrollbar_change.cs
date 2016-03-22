using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class on_scrollbar_change : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void on_vertical_Scrollbar_change(GameObject obj)
    {
        Scrollbar bar_temp = obj.GetComponent<Scrollbar>();
        float value = bar_temp.value;
        float min_h = (float)6;
        float max_h = (float)52;
        float temp_distance = (max_h - min_h) * value;
        float target_h = temp_distance + min_h;
        Vector3 pos_new = new Vector3(Camera.main.transform.position.x, target_h, Camera.main.transform.position.z);
        Camera.main.transform.position = pos_new;

    }
}
