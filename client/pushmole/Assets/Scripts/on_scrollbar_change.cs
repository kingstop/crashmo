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
		//int width_max = global_instance.Instance._crash_mole_grid_manager.get_max_width ();
		int height_max = global_instance.Instance._crash_mole_grid_manager.get_max_height() - 20;
		int grid_temp = height_max - 13;
        Scrollbar bar_temp = obj.GetComponent<Scrollbar>();
        float value = bar_temp.value;
        float min_h = (float)6;

		float max_h = min_h;
		if (grid_temp > 0) 
		{
			max_h = max_h + ((float)grid_temp * 1.74f);
		}
        float temp_distance = (max_h - min_h) * value;
        float target_h = temp_distance + min_h;
        Vector3 pos_new = new Vector3(Camera.main.transform.position.x, target_h, Camera.main.transform.position.z);
        Camera.main.transform.position = pos_new;

    }

	public void on_horizontal_Scrollbar_change(GameObject obj)
	{
		Scrollbar bar_temp = obj.GetComponent<Scrollbar>();
		float value = bar_temp.value;

		int width_max = global_instance.Instance._crash_mole_grid_manager.get_max_width ();
		float min_x = 2.84f;
		int grid_temp = width_max - 12;

		float max_x = min_x;
		if (grid_temp > 0) 
		{
			max_x = max_x + ((float)grid_temp * 1f);
		}
		float temp_distance = (max_x - min_x) * value;
		float target_x = temp_distance + min_x;
		Vector3 pos_new = new Vector3(target_x, Camera.main.transform.position.y, Camera.main.transform.position.z);
		Camera.main.transform.position = pos_new;
	}

}
