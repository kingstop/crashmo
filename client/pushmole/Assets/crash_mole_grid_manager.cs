using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class crash_mole_grid_manager : MonoBehaviour {
    GameObject _source_crash_mole_obj;
    GameObject _source_flag_mole_obj;
    crashmolegrid[,] _crashmolegrids;
    ArrayList _objlist = new ArrayList();
    crashmolegrid _flag_grid;
    bool _mouse_down = false;
    protected game_type _current_type;

    private int _max_width;
    private int _max_height;

    
    public int get_max_width()
    {
        return _max_width;
    }

    public int get_max_height()
    {
        return _max_height;
    }
    public void set_max_width(int maxwidth)
    {
        _max_width = maxwidth;
    }
    public void set_max_height(int maxheight)
    {
        _max_height = maxheight;
    }
    
    //prush_mo
    public game_type get_game_type()
    {
        return _current_type;
    }
    
    void Awake()
    {
        _source_crash_mole_obj = Resources.Load<GameObject>("prefab/mole_object");
        _source_flag_mole_obj = Resources.Load<GameObject>("prefab/flag");
        global_instance.Instance._crash_mole_grid_manager = this;
    }
    public void update_game_type(game_type type, message.CrashMapData mapinfo)
    {
        _current_type = type;
        switch (_current_type)
        {
            case game_type.edit:
                {
                    global_instance.Instance._crash_manager.clear();
                    if(mapinfo == null)
                    {
                        create_edit_crash_mole_grid();
                    }
                    else
                    {
                        create_edit_crash_mole_grid(mapinfo);
                    }
                    

                }
                break;
            case game_type.game:
                {
                   
                    create_map();
                    clear_edit_crash_mole_grid();
                    global_instance.Instance._crash_manager.create_map();
                }
                break;
            case game_type.create:
                {
                    clear_edit_crash_mole_grid();
                }
                break;
        }
    }
    public void clear_edit_crash_mole_grid()
    {
        if(_crashmolegrids != null)
        {
            for (int i = 0; i < _max_width; i++)
            {
                for (int j = 0; j < _max_height; j++)
                {
                    _crashmolegrids[i, j] = null;
                }
            }
        }

        int length = _objlist.Count;
        for (int i = 0; i < length; i++)
        {
            GameObject entry_obj = (GameObject)_objlist[i];
            Destroy(entry_obj);
        }
        _objlist.Clear();
    }

	public void create_edit_crash_mole_grid(message.CrashMapData info)
	{
        if(info == null)
        {
            return;
        }

        message.CrashmoMapBaseData mapinfo = info.Data;
		if (mapinfo != null)
		{
			_max_width = mapinfo.width;
			_max_height = mapinfo.height;
			int date_height = mapinfo.map_data.Count;
			if(_max_height != date_height)
			{
				Debug.LogError("map_max_height != date_height");
				return;
			}
		}
        create_edit_crash_mole_grid();


        List<message.int32array> mapdata = mapinfo.map_data;
		for (int i = 0; i < _max_width; i++)
		{
			for (int j = 0; j < _max_height; j++)
			{				
				float x = (float)i;
				float y = (float)j;
				int group = 11;
				if(mapinfo != null)
				{
					message.int32array entry  = mapdata[j];
					group = entry.data[i];
				}								
				if(group != 11)
				{
                    _crashmolegrids[i, j].set_group(group);
                }
			}
		}
	}

    public void create_edit_crash_mole_grid()
    {
        clear_edit_crash_mole_grid();
        _crashmolegrids = new crashmolegrid[_max_width, _max_height];
		Vector3 new_position = new Vector3((float)7.7, (float)4.7, (float)-10.6);
		Camera.main.transform.position = new_position;
		for (int i = 0; i < _max_width; i++)
		{
			for (int j = 0; j < _max_height; j++)
			{
				
				GameObject obj_temp = Instantiate<GameObject>(_source_crash_mole_obj);
				_objlist.Add(obj_temp);
				
				obj_temp.name = i.ToString() + "-" + j.ToString();
				_crashmolegrids[i, j] = obj_temp.GetComponent<crashmolegrid>();
				float x = (float)i;
				float y = (float)j;
				
				_crashmolegrids[i, j].set_position(x, y);
				_crashmolegrids[i, j].set_color(1, 1, 1, 1);
				_crashmolegrids[i, j].set_group(11);
			}
		}
	}

	public message.CrashMapData save_crash_mole_grid()
	{
		message.CrashMapData crash_map = new message.CrashMapData();
		crash_map.CreaterName = "";
		message.CrashmoMapBaseData map_data = new message.CrashmoMapBaseData ();
		map_data.width = _max_width;
		map_data.height = _max_height;
        

        for (int j = 0; j < _max_height; j ++) 
		{
			message.int32array tem_array = new message.int32array();
			for(int i = 0; i < _max_width; i ++)
			{
				int group = _crashmolegrids[i, j].get_group();
				tem_array.data.Add(group);
			}
			map_data.map_data.Add(tem_array);
		}
        crash_map.create_time = (ulong)System.DateTime.Now.Subtract(System.DateTime.Parse("1970-1-1")).TotalSeconds + 4 * 60 * 60;
        map_data.map_index = crash_map.create_time;
        crash_map.Data = map_data;
        crash_map.CreaterName = "unkonw";
        crash_map.MapName = "unknow";
        

        return crash_map;

	}

	// Use this for initialization
	void Start () {
        
      //  create_edit_crash_mole_grid();

        _mouse_down = false;
	}

    void Update()
    {
        if (get_game_type() == game_type.edit)
        {
            if(_mouse_down)
            {
                RaycastHit hitt = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hitt, 100);
                Debug.DrawLine(Camera.main.transform.position, ray.direction, Color.green);
            }
        }
        else
        {
            global_instance.Instance._crash_manager.update_move_animation();
        }
        
    }

    void OnMouseUp()
    {
        _mouse_down = false;
    }

    void OnMouseDown()
    {
        _mouse_down = true;

    }

    void seek_create_mole(int temp_x, int temp_y, crash_mole mole, bool dir_up = false)
    {
        if(temp_x >= 0 && temp_x < _max_width && temp_y >= 0 && temp_y < _max_height)
        {
            crash_mole mole_entry = null;
            int group = 11;

            group = _crashmolegrids[temp_x, temp_y]._group;
            if (group != 11)
            {
                mole_entry = global_instance.Instance._crash_manager.get_crash_mole_addr(temp_x, 9, temp_y + 20)._crash_mole;
                if (mole_entry == null)
                {
                    if (group == 10)
                    {
                        if (dir_up == true)
                        {
                            crash_base_obj obj = global_instance.Instance._crash_manager.create_flag_obj(temp_x, temp_y + 20);
                            mole.add_crash_obj(obj);
                            create_mole(temp_x, temp_y, mole);
                        }
                    }
                    else if (group == mole._color_group || dir_up == true && group == 10)
                    {
                        crash_obj obj = global_instance.Instance._crash_manager.create_crash_obj(temp_x, temp_y + 20);
                        mole.add_crash_obj(obj);
                        create_mole(temp_x, temp_y, mole);
                    }
                }

            }
        
        }
    }

    public void create_mole(int x, int y, crash_mole mole)
    {
        seek_create_mole(x - 1, y, mole);
        seek_create_mole(x + 1, y, mole);
        seek_create_mole(x, y - 1, mole);
        seek_create_mole(x, y + 1, mole, true);
    }

    public void create_map( )
    {
        for (int i = 0; i < _max_width; i++)
        {
            for (int j = 0; j < _max_height; j++)
            {

                int group = _crashmolegrids[i, j]._group;
                if (group != 11)
                {
                    crash_mole mole_entry = global_instance.Instance._crash_manager.get_crash_mole_addr(i, 9, j + 20)._crash_mole;
                    if (mole_entry == null)
                    {
                        crash_base_obj obj = null;
                        if (group != 10)
                        {
                            obj = global_instance.Instance._crash_manager.create_crash_obj(i, j + 20);
                        }
                        else
                        {
                            obj = global_instance.Instance._crash_manager.create_flag_obj(i, j + 20);
                        }

                        mole_entry = global_instance.Instance._crash_manager.create_crash_mole();
                        mole_entry.add_crash_obj(obj);
                        mole_entry._color_group = group;
                        global_instance.Instance._crash_manager.add_crash_mole(mole_entry);
                        create_mole(i, j, mole_entry);
                    }
                }

            

            }
        }
    }
	
}
