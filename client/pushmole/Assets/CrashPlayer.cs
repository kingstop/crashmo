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

    public void add_map(CrashMapData temp)
    {
        if(_officil_map.ContainsKey(temp.Section) == false)
        {
            Dictionary<int, CrashMapData> temp_dic = new Dictionary<int, CrashMapData>();
            _officil_map[temp.Section] = temp_dic;
        }
        _officil_map[temp.Section][temp.number] = temp;        
    }

    public void del_map(string name)
    {
        bool find_temp = false;
        foreach(KeyValuePair<int, Dictionary<int, CrashMapData>> entrypair in _officil_map)
        {
            foreach(KeyValuePair<int, CrashMapData> pair_temp in entrypair.Value)
            {
                CrashMapData entry = pair_temp.Value;
                if(entry.MapName == name)
                {
                    entrypair.Value.Remove(pair_temp.Key);
                    find_temp = true;
                    break;
                }                
            }
            if(find_temp == true)
            {
                if(entrypair.Value.Count == 0)
                {
                    _officil_map.Remove(entrypair.Key);
                    break;
                }
            }

        }
    }

    public List<CrashMapData> getPageMaps(int page)
    {
        List<CrashMapData> temp = new List<CrashMapData>();
        if (_officil_map.ContainsKey(page) == true)
        {
            Dictionary<int, CrashMapData> dic = _officil_map[page];
            foreach(KeyValuePair<int, CrashMapData> key_pair in dic)
            {
                temp.Add(key_pair.Value);
            }
        }
        return temp;
    }
    public CrashMapData getOfficilMap(int page, int number)
    {
        CrashMapData temp = null;
        if (_officil_map.ContainsKey(page) == true)
        {
            temp = _officil_map[page][number];
        }
        return temp;
    }

	public Dictionary<int, string>  get_officil_section_names()
	{
		return _officil_section_names;
	}

    protected Dictionary<int, Dictionary<int, CrashMapData>> _officil_map = new Dictionary<int, Dictionary<int, CrashMapData>>();
	protected Dictionary<int, string> _officil_section_names = new Dictionary<int, string>();
    protected  CrashPlayerInfo _info;
}
