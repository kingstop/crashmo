using UnityEngine;

public class crash_flag : MonoBehaviour {

	// Use this for initialization
    public GameObject flag;

    public void set_position(float x, float y, float z)
    {
        y -= 1.166f;
        Vector3 vc = new Vector3(x, y, z);
        this.transform.position = vc;
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
