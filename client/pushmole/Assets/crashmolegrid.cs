using UnityEngine;
using System.Collections;

public enum side_type
{
    side_left,
    side_right,
    side_top,
    side_bottom
}
public class crashmolegrid : MonoBehaviour
{
    public MeshRenderer current_mesh_render_;
    public int _group;
    public GameObject[] _side;
    private bool _mouse_down;
    private Texture _default_texture;
    private Texture _flag_texture;
    private Material _main_material;
    private bool _is_flag;

    public crashmolegrid()
    {

    }
    void Awake()
    {
        _main_material = current_mesh_render_.materials[0];
        _default_texture = _main_material.mainTexture;
        _flag_texture = LoadImage();

    }
 
	// Use this for initialization
	void Start () {
        //Color temp = new Color(255, 255, 255, 255);
        //current_mesh_render_.materials[0].color = temp;
        _mouse_down = false;
	
	}

    public void hide_sides()
    {
        foreach(GameObject obj in _side)
        {
            obj.SetActive(false);
        }
    }

    public void show_side(side_type dir)
    {
        int temp = (int)dir;
        if(temp >= _side.Length)
        {
            return;
        }
        _side[temp].SetActive(true);
    }

    public void set_color(Color color)
    {
        _main_material.color = color;
    }

    public void set_is_flag(bool b)
    {
        _is_flag = b;
        if(_is_flag)
        {
            _main_material.mainTexture = _flag_texture;
        }
        else
        {
            _main_material.mainTexture = _default_texture;
        }
        if(_is_flag == true)
        {
            global_instance.Instance._ngui_edit_manager.set_flag_grid(this);
        }        
        
        //material.SetTexture()
    }
    public void set_color(float r, float g, float b, float a)
    {
        Color temp = new Color(r, g, b, a);
        current_mesh_render_.materials[0].color = temp;
    }

    public Color get_color()
    {
        return current_mesh_render_.materials[0].color;
    }

    public int get_group()
    {
        return _group;
    }
    public void set_group(int i)
    {
        if (i != 10)
        {
            set_color(global_instance.Instance._ngui_edit_manager.get_color_by_group(i));
            set_is_flag(false);
        }
        else
        {
            set_is_flag(true);
        }
        _group = i;
    }
    public void set_position(float x, float y)
    {
        Vector3 new_position = new Vector3(x, y, this.gameObject.transform.position.z);
        this.gameObject.transform.position = new_position;
      
    }

    public void set_position(float x, float y, float z)
    {
        Vector3 new_position = new Vector3(x, y, z);
        this.gameObject.transform.position = new_position;
    }


    void OnMouseDown()
    {
        _mouse_down = true;

    }

    void OnMouseUp()
    {
        _mouse_down = false;
    }

    private Texture2D LoadImage()
    {
        Texture2D tex = Resources.Load<Texture2D>("image/target_pos");
        return tex;
    }

	// Update is called once per frame
	void Update () {
        if(_mouse_down)
        {
            if(global_instance.Instance._crash_mole_grid_manager.get_game_type() == game_type.edit)
            {
                RaycastHit hitt = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hitt, 100);

                Debug.DrawLine(Camera.main.transform.position, ray.direction, Color.green);

                if (null != hitt.transform)
                {
                    crashmolegrid grid_temp = hitt.transform.gameObject.GetComponent<crashmolegrid>();
                    if (grid_temp != null)
                    {
                        if (grid_temp.get_group() != global_instance.Instance._current_group)
                        {
                            grid_temp.set_group(global_instance.Instance._current_group);
                            global_instance.Instance._crash_manager.add_color(global_instance.Instance._current_group, global_instance.Instance._current_color);
                          //  print("name[" + hitt.transform.gameObject.name + "] group[" + global_instance.Instance._current_group.ToString() + "]");
                        }
                    }
                }
            }
        }

	}
}
