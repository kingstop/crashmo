using UnityEngine;
using System.Collections;

public class crash_flag : MonoBehaviour {

	// Use this for initialization
    public GameObject flag;
    public GameObject flag_pole;

    public void set_position(float x, float y, float z)
    {
        y -= 1.166f;
        Vector3 vc = new Vector3(x, y, z);
        this.transform.position = vc;
        //flag.transform.position = vc;
        //flag_pole.transform.position = vc;
        //  _creature_physics.transform.position = _current_position;
    }

    public Vector3 get_position()
    {
        Vector3 vc = new Vector3();
        vc = this.transform.position;
        vc.y += 1.166f;
        return vc;
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
