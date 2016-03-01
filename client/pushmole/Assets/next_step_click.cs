using UnityEngine;
public class next_step_click : MonoBehaviour {

    Rigidbody _Rigidbody = null;
	// Use this for initialization
	void Start () {
        _Rigidbody = GetComponentInParent<Rigidbody>();
    }


    // Update is called once per frame
    void Update () {
	
	}

    public void ButtonStepNextClick()
    {
        global_instance.Instance._crash_manager.need_fall_update();
    }
}
