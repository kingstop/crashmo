using UnityEngine;
using System.Collections.Generic;
public enum side_type
{
    side_left,
    side_right,
    side_top,
    side_bottom,
}


public enum side_qude_type
{
    side_qude_left,
    side_qude_right,
    side_qude_top,
    side_qude_bottom,
    side_qude_front,
    side_qude_back
}
    


public enum box_side_type
{
    box_side_left_front,
    box_side_right_front,
    box_side_top_front,
    box_side_bottom_front,
    box_side_left_back,
    box_side_right_back,
    box_side_top_back,
    box_side_bottom_back,
    box_line_1,
    box_line_2,
    box_line_3,
    box_line_4
}
public class crashmolegrid : MonoBehaviour
{
    public MeshRenderer current_mesh_render_;
    public int _group;
    public GameObject[] _side;
    public GameObject[] _quads;
    private bool _mouse_down;
    private Texture _default_texture;
    //private Texture _flag_texture;
    private Material _main_material;
    private bool _is_flag;
    private int[] _line_count = new  int[4];
    //private Dictionary<int, Texture> _group_Textures = new Dictionary<int, Texture>();
    private List<Material> _Materials = new List<Material>();
    

    public crashmolegrid()
    {

    }
    void Awake()
    {
        if(current_mesh_render_ != null)
        {
            _main_material = current_mesh_render_.materials[0];
            _default_texture = _main_material.mainTexture;
        }
        
        /*
        _flag_texture = LoadImage();
        for(int i = 0; i <= 11; i ++ )
        {
            string path = "texture/group_" + i.ToString();
            
            Texture2D tex = Resources.Load<Texture2D>(path);
            _group_Textures[i] = tex;
            //Resources.Load<Texture2D>("texture/")
        }
        */
        foreach( GameObject obj in _quads)
        {
            Material material = obj.GetComponent<MeshRenderer>().materials[0];
            _Materials.Add(material);
        }
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
        int length = _line_count.Length;
        for(int i = 0; i < length; i ++)
        {
            _line_count[i] = 0;
        }
        //_quads
        if(_quads.Length != 0)
        {
            foreach (GameObject obj in _quads)
            {
                obj.SetActive(false);
            }

            _quads[(int)side_qude_type.side_qude_front].SetActive(true);
            _quads[(int)side_qude_type.side_qude_back].SetActive(true);

        }
    }


    public void show_edit()
    {
        hide_sides();
        _quads[(int)side_qude_type.side_qude_front].SetActive(true);
        _side[(int)box_side_type.box_side_left_front].SetActive(true);
        _side[(int)box_side_type.box_side_right_front].SetActive(true);
        _side[(int)box_side_type.box_side_top_front].SetActive(true);
        _side[(int)box_side_type.box_side_bottom_front].SetActive(true);
    }

    public void show_side(side_type dir)
    {
        if (_quads.Length != 0)
        {
            _quads[(int)dir].SetActive(true);
        }
        switch (dir)
        {
            case side_type.side_bottom:
                {
                    _side[(int)box_side_type.box_side_bottom_front].SetActive(true);
                    _side[(int)box_side_type.box_side_bottom_back].SetActive(true);
                    _line_count[(int)box_side_type.box_line_2 - 8]++;
                    _line_count[(int)box_side_type.box_line_3 - 8]++;

                }
                break;
            case side_type.side_top:
                {
                    _side[(int)box_side_type.box_side_top_front].SetActive(true);
                    _side[(int)box_side_type.box_side_top_back].SetActive(true);
                    _line_count[(int)box_side_type.box_line_1 - 8]++;
                    _line_count[(int)box_side_type.box_line_4 - 8]++;

                }
                break;
            case side_type.side_left:
                {
                    _side[(int)box_side_type.box_side_left_front].SetActive(true);
                    _side[(int)box_side_type.box_side_left_back].SetActive(true);
                    _line_count[(int)box_side_type.box_line_3 - 8]++;
                    _line_count[(int)box_side_type.box_line_4 - 8]++;

                }
                break;
            case side_type.side_right:
                {
                    _side[(int)box_side_type.box_side_right_front].SetActive(true);
                    _side[(int)box_side_type.box_side_right_back].SetActive(true);
                    _line_count[(int)box_side_type.box_line_1 - 8]++;
                    _line_count[(int)box_side_type.box_line_2 - 8]++;
                }
                break;
        }
        int length = _line_count.Length;
        for(int i = 0; i < length; i ++)
        {
            if(_line_count[i] >= 2)
            {
                _side[i + 8].SetActive(true);
            }
        }
    }

    public void set_color(Color color)
    {
        Color temp_color = new Color();
        if(_main_material != null)
        {
            temp_color = _main_material.GetColor("_Color");
            temp_color.r = color.r;
            temp_color.b = color.b;
            temp_color.g = color.g;
            _main_material.SetColor("_Color", temp_color);
        }

        if(_Materials.Count != 0)
        {
            foreach(Material mat in _Materials)
            {
                temp_color = mat.GetColor("_Color");
                temp_color.r = color.r;
                temp_color.b = color.b;
                temp_color.g = color.g;
                mat.SetColor("_Color", temp_color);
            }
        }
        
    }

    public void set_alpha(float temp)
    {
        Color temp_color = new Color();
        if(_main_material != null)
        {
            temp_color = _main_material.GetColor("_Color");
            temp_color.a = temp;
            _main_material.SetColor("_Color", temp_color);
        }

        foreach(Material mat in _Materials)
        {
            temp_color = mat.GetColor("_Color");
            temp_color.a = temp;
            mat.SetColor("_Color", temp_color);

        }
    }

    public void set_is_flag(bool b)
    {
        _is_flag = b;
        if(_main_material != null)
        {
            if (_is_flag)
            {
                _main_material.mainTexture = global_instance.Instance.GetFlagTexture();
            }
            else
            {
                _main_material.mainTexture = _default_texture;
            }
        }
        if(_Materials.Count != 0)
        {
            Texture tex = global_instance.Instance.GetGroupTexture(_group);
            if (tex)
            {
                foreach (Material mat in _Materials)
                {
                    if (_is_flag)
                    {
                        mat.mainTexture = global_instance.Instance.GetFlagTexture();
                    }
                    else
                    {
                        mat.mainTexture = tex;
                    }
                }

            }
        }

        if(_is_flag == true)
        {
            global_instance.Instance._ngui_edit_manager.set_flag_grid(this);
        }        
    }
    public void set_color(float r, float g, float b, float a)
    {
        Color temp = new Color(r, g, b, a);
        if(current_mesh_render_ != null)
        {
            current_mesh_render_.materials[0].color = temp;
        }

        if (_Materials.Count != 0)
        {
            foreach(Material mat in _Materials)
            {
                mat.color = temp;
            }
        }


    }

    public Color get_color()
    {
        Color temp = new Color();
        if(current_mesh_render_ != null)
        {
            temp = current_mesh_render_.materials[0].color;
        }
        else
        {
            if(_Materials.Count != 0)
            {
                temp = _Materials[0].color;
            }
        }
        return temp;
    }

    public int get_group()
    {
        return _group;
    }
    public void set_group(int i)
    {
        _group = i;
        Texture tex = global_instance.Instance.GetGroupTexture(_group);
        foreach (Material mat in _Materials)
        {
            mat.mainTexture = tex;
            //mat.SetTexture("_MainTex", Tex);
        }
       
        if (i != 10)
        {
            set_color(global_instance.Instance.get_color_by_group(i));
            set_is_flag(false);
        }
        else
        {
            set_is_flag(true);
        }
        
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
		Debug.Log (Input.mousePosition);
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
        if(_mouse_down&& global_instance.Instance._can_set_group == true)
        {
			int x = (int)Input.mousePosition.x;
			//if (x < 322) 
			//{
			//	return;
			//}

            if(global_instance.Instance._crash_mole_grid_manager != null)
            {
                if (global_instance.Instance._crash_mole_grid_manager.get_game_type() == game_type.edit)
                {
                    RaycastHit hitt = new RaycastHit();
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Physics.Raycast(ray, out hitt, 100);
                    if (null != hitt.transform)
                    {
                        crashmolegrid grid_temp = hitt.transform.gameObject.GetComponent<crashmolegrid>();
                        if (grid_temp != null)
                        {

                            if (grid_temp.get_group() != global_instance.Instance._current_group)
                            {
								int count_temp = 1;
								if (global_instance.Instance._current_group != 11 || global_instance.Instance._current_group != 10) 
								{
									count_temp = global_instance.Instance._crash_mole_grid_manager.getResourceRaminderCount(global_instance.Instance._current_group);
								}

								if (count_temp > 0 || global_instance.Instance._player.isadmin() == true) 
								{
									int last_group = grid_temp.get_group ();
									grid_temp.set_group (global_instance.Instance._current_group);
									global_instance.Instance._crash_mole_grid_manager.modifyResourceCount (last_group, -1);
									global_instance.Instance._crash_mole_grid_manager.modifyResourceCount (global_instance.Instance._current_group, 1);
									
								}
								else 
								{
                                    global_instance.Instance._ngui_edit_manager.set_point_text("没有足够资源");
                                }                                									                                                               
                            }
                        }
                    }
                }

            }
        }

	}
}
