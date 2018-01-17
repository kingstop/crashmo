using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class sincomponent : MonoBehaviour {
    float _pos_y;
    float _pos_x;
    public float _move_dis = 50;
    void Awake()
    {

    }
	// Use this for initialization
	void Start () {
        _move_dis = 40;
        _pos_y = this.transform.localPosition.y;
        _pos_x = this.transform.localPosition.x;
    }
	
	// Update is called once per frame
	void Update () {
        long time = System.DateTime.Now.Ticks;
        float f_temp_x = _pos_x % 360;
        //
        time = time / 1000000;
        // 10000000.0

        long current_temp = time % 360;
        float temp_dis = _move_dis * Mathf.Sin(Mathf.PI / 180 * current_temp *4 + f_temp_x);
        float temp_y = _pos_y + temp_dis;
        Vector3 vec = new Vector3(this.transform.localPosition.x, temp_y, this.transform.localPosition.z);
        
        this.transform.localPosition = vec;
    }
}
