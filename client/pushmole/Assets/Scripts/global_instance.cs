using UnityEngine;
using System;

public enum global_game_type
{
    global_game_type_edit,
    global_game_type_game
}

public class global_instance
{
     private static global_instance instance;


     private global_instance() {
         _crash_manager = new crash_manager();
         _player = new CrashPlayer();
         _client_session = new client_session();
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


}

