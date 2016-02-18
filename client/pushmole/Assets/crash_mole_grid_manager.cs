using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum MapInitType
{
    InitMapData_type,
    MsgInitMapData_Type,
    GameInitMapData_Type
}
public class MapData
{
    public int width_;
    public int height_;
    public ulong create_time_;
    public int[,] groups_;
    public string map_name_;
    public string create_name_;
    public int number_;
    public int section_;
    public ulong map_index_;
    public message.CrashMapData get_info()
    {
        message.CrashMapData Data = new message.CrashMapData();
        Data.CreaterName = create_name_;
        Data.create_time = create_time_;
        Data.number = number_;
        Data.Section = section_;
        Data.Data.height = height_;
        Data.Data.width = width_;
        Data.Data.map_index = map_index_;
        if(groups_ != null)
        {
            for(int y = 0; y < height_; y ++)
            {
                message.int32array int32temp = new message.int32array();
                for (int x = 0; x < width_; x ++)
                {
                    int item_entry = groups_[x, y];
                    int32temp.data.Add(item_entry);
                }
                Data.Data.map_data.Add(int32temp);
            }
        }
        return Data;
    }


    public void set_info(message.CrashMapData Data)
    {
        create_name_ = Data.CreaterName;
        create_time_ = Data.create_time;
        number_ = Data.number;
        section_ = Data.Section;
        height_ = Data.Data.height;
        width_ = Data.Data.width;
        map_index_ = Data.Data.map_index;
        groups_ = new int[width_, height_];
        for(int y = 0; y < Data.Data.map_data.Count; y ++)
        {
           message.int32array list_int32 =  Data.Data.map_data[y];
            for(int x = 0; x < list_int32.data.Count; x ++)
            {
                groups_[x, y] = list_int32.data[x];
            }            
        }       
    }
}



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
    public void update_game_type(game_type type)
    {        
        switch (type)
        {
            case game_type.edit:
                {
                    global_instance.Instance._crash_manager.clear();
                    create_edit_crash_mole_grid();                    
                }
                break;
            case game_type.game:
                {                                       
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
        _current_type = type;
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

	//public void create_edit_crash_mole_grid(message.CrashMapData info)
	//{
 //       if(info == null)
 //       {
 //           return;
 //       }

 //       message.CrashmoMapBaseData mapinfo = info.Data;
	//	if (mapinfo != null)
	//	{
	//		_max_width = mapinfo.width;
	//		_max_height = mapinfo.height;
	//		int date_height = mapinfo.map_data.Count;
	//		if(_max_height != date_height)
	//		{
	//			Debug.LogError("map_max_height != date_height");
	//			return;
	//		}
	//	}
 //       create_edit_crash_mole_grid();


 //       List<message.int32array> mapdata = mapinfo.map_data;
	//	for (int i = 0; i < _max_width; i++)
	//	{
	//		for (int j = 0; j < _max_height; j++)
	//		{				
	//			float x = (float)i;
	//			float y = (float)j;
	//			int group = 11;
	//			if(mapinfo != null)
	//			{
	//				message.int32array entry  = mapdata[j];
	//				group = entry.data[i];
	//			}								
	//			if(group != 11)
	//			{
 //                   _crashmolegrids[i, j].set_group(group);
 //               }
	//		}
	//	}
	//}

    public void create_edit_crash_mole_grid()
    {
        clear_edit_crash_mole_grid();
        MapData mapinfo = global_instance.Instance.GetMapData();
        if(mapinfo != null)
        {
            _max_width = mapinfo.width_;
            _max_height = mapinfo.height_;
        }
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
                if (mapinfo != null)
                {
                    int group = mapinfo.groups_[i, j];
                    if(group != 11)
                    {
                        _crashmolegrids[i, j].set_group(group);
                    }
                    
                }
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
        crash_map.CreaterName = "UNKNOW";
        crash_map.MapName = "UNKNOW";
        if (global_instance.Instance.GetMapData() != null)
        {
            crash_map.CreaterName = global_instance.Instance.GetMapData().create_name_;
            crash_map.MapName = global_instance.Instance.GetMapData().map_name_;
        }


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
                //Physics.Raycast(ray, out hitt, 100);
                //Debug.DrawLine(Camera.main.transform.position, ray.direction, Color.green);
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
    /*
   
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
    */
	
}
