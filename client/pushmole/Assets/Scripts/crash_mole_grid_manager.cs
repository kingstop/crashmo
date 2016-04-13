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
    
    public Texture2D CreateTexture()
    {
        Texture2D tex = null;
        int grid = 10;
        if(groups_ != null)
        {
            tex = new Texture2D(width_ * grid, height_ * grid);            
            for (int j = 0; j < height_; j++)
            {                
                for (int i = 0; i < width_; i++)
                {
                    int group =groups_[i, j];
                    int begin_x = i * grid;
                    int begin_y = j * grid;
                    for (int temp_x = begin_x; temp_x < begin_x + grid; temp_x++)
                    {
                        for (int temp_y = begin_y; temp_y < begin_y + grid; temp_y++)
                        {
                            Color co = global_instance.Instance._ngui_edit_manager.get_color_by_group(group);
                            tex.SetPixel(temp_x, temp_y, co);
                        }
                    }                  
                }
            }

        }
        tex.Apply();
        return tex;       
    }

    public message.CrashMapData get_info()
    {
        message.CrashMapData Data = new message.CrashMapData();
        Data.CreaterName = create_name_;
        Data.create_time = create_time_;
        Data.number = number_;
        Data.Section = section_;
        Data.Data = new message.CrashmoMapBaseData();
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
    GameObject _source_map_mole_obj;
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
    
    public game_type get_game_type()
    {
        return _current_type;
    }
    
    void Awake()
    {
        _source_crash_mole_obj = Resources.Load<GameObject>("prefab/mole_object");
        _source_flag_mole_obj = Resources.Load<GameObject>("prefab/flag");
        _source_map_mole_obj = Resources.Load<GameObject>("prefab/map");
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
                    global_instance.Instance.ClearMapData();
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
            _crashmolegrids = null;
        }

        int length = _objlist.Count;
        for (int i = 0; i < length; i++)
        {
            GameObject entry_obj = (GameObject)_objlist[i];
            Destroy(entry_obj);
        }
        _objlist.Clear();
    }


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

	public MapData save_crash_mole_grid()
	{
        MapData data = new MapData();
        data.create_name_ = "";
        data.width_ = _max_width;        
		data.height_ = _max_height;
        GameObject map_target = GameObject.Instantiate<GameObject>(_source_map_mole_obj);
        data.groups_ = new int[data.width_, data.height_];
        for (int j = 0; j < data.height_; j ++) 
		{
			message.int32array tem_array = new message.int32array();
			for(int i = 0; i < data.width_; i ++)
			{
				int group = _crashmolegrids[i, j].get_group();
                data.groups_[i, j] = group;
            }			
		}
        data.create_time_= (ulong)System.DateTime.Now.Subtract(System.DateTime.Parse("1970-1-1")).TotalSeconds + 4 * 60 * 60;
        data.map_index_ = data.create_time_;
        data.create_name_ = "unknow";
        data.map_name_ = "unknow";
        if (global_instance.Instance.GetMapData() != null)
        {
            data.create_name_ = global_instance.Instance.GetMapData().create_name_;
            data.map_name_ = global_instance.Instance.GetMapData().map_name_;
        }
        return data;
	}

	// Use this for initialization
	void Start () {        
        _mouse_down = false;
	}

    void Update()
    {

    }

    void OnMouseUp()
    {
        _mouse_down = false;
    }

    void OnMouseDown()
    {
        _mouse_down = true;

    }

	
}
