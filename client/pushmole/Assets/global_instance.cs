using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


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

    public Color _current_color;
    public int _current_group;
    public crash_manager _crash_manager;
    public crash_mole_grid_manager _crash_mole_grid_manager;
    public ngui_edit_manager _ngui_edit_manager;
    public u3dclient _net_client;
    public client_session _client_session;
    public CrashPlayer _player;

    private MapData _map_data = null;

}

