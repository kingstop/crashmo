using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using message;

public class CrashPlayer
{
    public CrashPlayer()
    {

    }
    ~CrashPlayer()
    {

    }

    public void SetInfo(CrashPlayerInfo info)
    {
        _info = info;
    }

    public CrashPlayerInfo GetInfo()
    {
        return _info;
    }

    public void AddTask(TaskInfo info)
    {
        _info.current_task.Add(info);
    }


    public void delMap(MapType type, ulong map_index)
    {
        switch(type)
        {
            case MapType.CompleteMap:
                {
                    foreach(ulong entry_long in _info.CompleteMap)
                    {
                        if(map_index == entry_long)
                        {
                            _info.CompleteMap.Remove(map_index);
                            break;
                        }
                    }

                }
                break;
            case MapType.ImcompleteMap:
                {
                    foreach(ulong entry_long in _info.IncompleteMap)
                    {
                        if (entry_long == map_index)
                        {
                            _info.CompleteMap.Remove(entry_long);
                            break;
                        }
                    }
                }
                break;
            case MapType.OfficeMap:
                {
                    foreach(KeyValuePair<int, Dictionary<int, ulong>> list_entry in _officil_map)
                    {
                        foreach(KeyValuePair<int, ulong> key_entry in list_entry.Value)
                        {
                            if(key_entry.Value == map_index)
                            {
                                list_entry.Value.Remove(key_entry.Key);
                                break;
                            }
                        } 
                    }
                }
                break;
        }

    }



    public List<message.intPair> get_resource()
    {
        return _info.resources;
    }


    public bool isadmin()
    {
        return _info.isadmin;
    }

    public void addUserMap(CrashMapData data , MapType type)
    {
		
        
        _maps[data.Data.map_index] = data;
        switch (type)
        {
            case MapType.CompleteMap:
                {
                    _info.CompleteMap.Add(data.Data.map_index);
                }
                break;
            case MapType.ImcompleteMap:
                {
                    _info.IncompleteMap.Add(data.Data.map_index);
                }
                break;
            case MapType.OfficeMap:
                {
                    global_instance.Instance._officilMapManager.addMap(data);
                }
                break;
        }
        
    }

    public CrashMapData getUserMap(UInt64 index)
    {
        CrashMapData map = null;
        if(_maps.ContainsKey(index) == true)
        {
            map = _maps[index];
        }
        return map;
    }


    protected Dictionary<UInt64, CrashMapData> _maps = new Dictionary<ulong, CrashMapData>();
    protected Dictionary<int, Dictionary<int, ulong>> _officil_map = new Dictionary<int, Dictionary<int, ulong>>();
	protected Dictionary<int, string> _officil_section_names = new Dictionary<int, string>();
    protected  CrashPlayerInfo _info;
}
