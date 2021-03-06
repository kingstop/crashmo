﻿using UnityEngine;
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
    public int chapter_;
    public int section_;
    public ulong map_index_;
    public int gold_;
    public int grid_size_ = 10;
    protected Dictionary<int, int> _resorces = new Dictionary<int, int>();

    public void create(int width, int height)
    {
        width_ = width;
        height_ = height;
        groups_ = new int[width, height];
        for(int i = 0; i < width; i ++)
        {
            for(int j = 0;  j < height; j ++)
            {
                groups_[i, j] = 11;
            }
        }
    }

    public Texture2D CreateEmptyTexture()
    {
        Texture2D tex = null;
        int grid = grid_size_;
        tex = new Texture2D(width_ * grid, height_ * grid);
        Color co = global_instance.Instance.get_color_by_group(11);
        for (int j = 0; j < height_; j++)
        {
            for (int i = 0; i < width_; i++)
            {
                tex.SetPixel(i, j, co);
            }
        }
        return tex;
    }

    public void DrawGridTexture(Texture2D tex, int i, int j, int group,bool IsMapButton = true)
    {
        Color co_default = new Color(127f / 255f, 127f / 255f, 127f / 255f, 127f / 255f);
        int begin_x = i * grid_size_;
        int begin_y = j * grid_size_;
        for (int temp_x = begin_x; temp_x < begin_x + grid_size_; temp_x++)
        {
            for (int temp_y = begin_y; temp_y < begin_y + grid_size_; temp_y++)
            {
                if ((group == 11 || group == 10) && IsMapButton)
                {
                    tex.SetPixel(temp_x, temp_y, co_default);
                }
                else
                {
                    Color co = global_instance.Instance.get_color_by_group(group);
                    tex.SetPixel(temp_x, temp_y, co);
                }

            }
        }
        groups_[i, j] = group;

        tex.Apply();
    }

    public void DrawTexture(Texture2D tex, bool IsMapButton = true)
    {
        //tex.width = grid_size_ * width_;
        //tex.height = grid_size_ * height_;
        Color co_default = new Color(127f / 255f, 127f / 255f, 127f / 255f, 127f / 255f);
        for (int j = 0; j < height_; j++)
        {
            for (int i = 0; i < width_; i++)
            {
                int group = groups_[i, j];
                int begin_x = i * grid_size_;
                int begin_y = j * grid_size_;
                for (int temp_x = begin_x; temp_x < begin_x + grid_size_; temp_x++)
                {
                    for (int temp_y = begin_y; temp_y < begin_y + grid_size_; temp_y++)
                    {
                        if ((group == 11 || group == 10) && IsMapButton)
                        {
                            tex.SetPixel(temp_x, temp_y, co_default);
                        }
                        else
                        {
                            Color co = global_instance.Instance.get_color_by_group(group);
                            tex.SetPixel(temp_x, temp_y, co);
                        }

                    }
                }
            }
        }
        tex.Apply();
    }

    public Texture2D CreateTexture(bool IsMapButton = true)
    {
        Texture2D tex = null;
        int grid = grid_size_;
        if(groups_ != null)
        {
            tex = new Texture2D(width_ * grid, height_ * grid);
            DrawTexture(tex, IsMapButton);           
        }        
        return tex;       
    }

    public message.CrashMapData get_info()
    {
        message.CrashMapData Data = new message.CrashMapData();
        Data.CreaterName = create_name_;
        Data.create_time = create_time_;
        Data.Chapter = chapter_;
        Data.Section = section_;
        Data.Data = new message.CrashmoMapBaseData();
        Data.Data.height = height_;
        Data.Data.width = width_;
        Data.Data.map_index = map_index_;
        Data.MapName = map_name_;
        Data.gold = gold_;
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
        chapter_ = Data.Chapter;
        section_ = Data.Section;
        height_ = Data.Data.height;
        width_ = Data.Data.width;
        map_index_ = Data.Data.map_index;
        gold_ = Data.gold;        
        groups_ = new int[width_, height_];
        map_name_ = Data.MapName;
        for (int y = 0; y < Data.Data.map_data.Count; y ++)
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
    crashmolegrid[,] _crashmolegrids;
    ArrayList _objlist = new ArrayList();
    protected game_type _current_type;
    protected Dictionary<int, int> _resorces = new Dictionary<int, int>();
    protected Dictionary<int, int> _resources_max = new Dictionary<int, int>();
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
        _source_crash_mole_obj = Resources.Load<GameObject>("prefab/CrashmoGrid");
        //_source_crash_mole_obj = Resources.Load<GameObject>("prefab/mole_object");
        // _source_map_mole_obj = Resources.Load<GameObject>("prefab/map");
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


    public void create_edit_image()
    {
        MapData mapinfo = global_instance.Instance.GetMapData();
        if(mapinfo != null)
        {
             _max_width = mapinfo.width_;
             _max_height = mapinfo.height_;
        }
    }

    public void create_edit_crash_mole_grid()
    {
        MapData mapinfo = global_instance.Instance.GetMapData();
        if(mapinfo == null)
        {
            mapinfo = new MapData();
            mapinfo.create(_max_width, _max_height);            
        }
        else
        {

        }
        global_instance.Instance._ngui_edit_manager.EditMap_.setMapInfo(mapinfo); 
    }

    public int getResourceMaxCount(int group)
    {
        int resource_max = 0;
        if (_resources_max.ContainsKey(group) == true)
        {
            resource_max = _resources_max[group];
        }
        return resource_max;
    }

    public void updateColorButtonText(int group)
    {
        int current_resource = getResourceCount(group);
        int max_resource = getResourceMaxCount(group);
        string current_text = current_resource.ToString() + "/" + max_resource.ToString();
        global_instance.Instance._ngui_edit_manager.setColorButtonText(group, current_text);
    }

    public int getResourceRaminderCount(int group)
    {
        int resource_max = getResourceMaxCount(group);
        int current_resource_count = getResourceCount(group);
        int remainder = resource_max - current_resource_count;
        return remainder;
    }

    public void modifyResourceCount(int group, int i)
    {
        int max_count = getResourceMaxCount(group);
        int current_count = getResourceCount(group);
        int temp_count = current_count + i;
        if(temp_count < 0)
        {
            temp_count = 0;
        }
        else if(temp_count > max_count)
        {
            //temp_count = max_count;
        }
        _resorces[group] = temp_count;
		if (group != 11 && group != 10) 
		{
			string txt = temp_count.ToString () + "/" + max_count.ToString ();
			global_instance.Instance._ngui_edit_manager.setColorButtonText (group, txt);
		}
    }
    public int getResourceCount(int group)
    {
        int current_resource_count = 0;
        if (_resorces.ContainsKey(group) == true)
        {
            current_resource_count = _resorces[group];
        }
        return current_resource_count;
    }
	//public MapData save_crash_mole_grid()
	//{
 //       MapData data = new MapData();
 //       data.create_name_ = "";
 //       data.width_ = _max_width;        
	//	data.height_ = _max_height;
 //      // GameObject map_target = GameObject.Instantiate<GameObject>(_source_map_mole_obj);
 //       data.groups_ = new int[data.width_, data.height_];
 //       for (int j = 0; j < data.height_; j ++) 
	//	{
	//		//message.int32array tem_array = new message.int32array();
	//		for(int i = 0; i < data.width_; i ++)
	//		{
	//			int group = _crashmolegrids[i, j].get_group();
 //               data.groups_[i, j] = group;
 //           }			
	//	}
 //       data.create_time_= (ulong)System.DateTime.Now.Subtract(System.DateTime.Parse("1970-1-1")).TotalSeconds + 4 * 60 * 60;
 //       data.map_index_ = data.create_time_;
 //       data.create_name_ = "unknow";
 //       data.map_name_ = "unknow";
 //       if (global_instance.Instance.GetMapData() != null)
 //       {
 //           data.create_name_ = global_instance.Instance.GetMapData().create_name_;
 //           data.map_name_ = global_instance.Instance.GetMapData().map_name_;
 //       }
 //       return data;
	//}

	// Use this for initialization
	void Start () {        
       // _mouse_down = false;
	}

    void Update()
    {

    }

    void OnMouseUp()
    {
       // _mouse_down = false;
    }

    void OnMouseDown()
    {
      //  _mouse_down = true;

    }

	
}
