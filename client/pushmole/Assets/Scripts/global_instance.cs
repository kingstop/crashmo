using UnityEngine;
using System;
using System.Collections.Generic; 
public enum global_game_type
{
	global_game_type_no,
    global_game_type_edit,
    global_game_type_game
}

public class global_instance
{
    private static global_instance instance;
	private Dictionary<int, Color> _group_color = new Dictionary<int,Color>();
    private Dictionary<int, Texture> _group_Textures = new Dictionary<int, Texture>();
    private Texture _flag_texture;
    private string _language_path;

    private global_instance() {
         _crash_manager = new crash_manager();
         _player = new CrashPlayer();
         _client_session = new client_session();
		_group_color.Add(0, new Color((float)120/255, (float)56/255, (float)56/255));
		_group_color.Add(1, new Color((float)170/255, (float)170/255, (float)255/255));
		_group_color.Add(2, new Color((float)246/255, (float)152/255, (float)152/255));
		_group_color.Add(3, new Color((float)154/255, (float)61/255, (float)154/255));
		_group_color.Add(4, new Color((float)90/255, (float)174/255, (float)174/255));
		_group_color.Add(5, new Color((float)255/255, (float)245/255, (float)71/255));
		_group_color.Add(6, new Color((float)180/255, (float)116/255, (float)116/255));
		_group_color.Add(7, new Color((float)219/255, (float)0/255, (float)0/255));
		_group_color.Add(8, new Color((float)5/255, (float)72/255, (float)246/255));
		_group_color.Add(9, new Color((float)0 / 255, (float)0 / 255, (float)0 / 255));
		_group_color.Add(10, new Color((float)255 / 255, (float)255 / 255, (float)255 / 255));
		_group_color.Add(11, new Color((float)255 / 255, (float)255 / 255, (float)255 / 255));
        _language_path = "/chinese/";
        _flag_texture = Resources.Load<Texture2D>("image/target_pos");
        for (int i = 0; i <= 11; i++)
        {
            string path = "texture/group_" + i.ToString();

            Texture2D tex = Resources.Load<Texture2D>(path);
            _group_Textures[i] = tex;
            //Resources.Load<Texture2D>("texture/")
        }
    }


    public string GetLanguagePath()
    {
        return _language_path;
    }
    public Texture GetFlagTexture()
    {
        return _flag_texture;
    }

    public Texture GetGroupTexture(int group)
    {
        Texture t = null;
        if(_group_Textures.ContainsKey(group))
        {
            t = _group_Textures[group];
        }
        return t;
    }

    public Dictionary<int, Color> getGroupColors()
	{
		return _group_color;
	}

	public Color get_color_by_group(int group)
	{
		return _group_color [group];
	}
     public static global_instance Instance 
     { 
          get  
          { 
             if (instance == null) 
             {
                 instance = new global_instance(); 
             } 
             return instance; 
          } 
      }
    
    public void SetMapData(MapData temp)
    {
        _map_data = temp;
        global_instance.instance._ngui_edit_manager._ui_section_gold.SetGoldCount(_map_data.gold_);
    }

    public void ClearMapData()
    {
        _map_data = null;
    }

    public void SetMapData(message.CrashMapData temp)
    {
        if(_map_data == null)
        {
            _map_data = new MapData();
        }
        _map_data.set_info(temp);
    }

    public MapData GetMapData()
    {
        return _map_data;
    }

  
    

	public long getTime()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64 (ts.TotalSeconds);
	}



    public Color _current_color;
    public int _current_group;
    public crash_manager _crash_manager;
    public crash_mole_grid_manager _crash_mole_grid_manager;
    public ngui_edit_manager _ngui_edit_manager;
    public u3dclient _net_client;
    public client_session _client_session;
    public CrashPlayer _player;
    public bool _can_set_group = true;
    private MapData _map_data = null;
    public global_game_type _global_game_type = global_game_type.global_game_type_game;
	public OfficilMapManager _officilMapManager = new OfficilMapManager ();
	public RankMapManager _rankMapManager = new RankMapManager ();
    public TaskManager _taskManager = new TaskManager();
    public bool _in_login = false;
    public FileHelper _file_helper;


}

