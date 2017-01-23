using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class OfficilMapManager
{
    protected Dictionary<int, Dictionary<int, message.CrashMapData>> _officilMap = new Dictionary<int, Dictionary<int, message.CrashMapData>>();
	protected List<int> _chapter_ids = new List<int> ();
	protected Dictionary<int, string> _officil_chapter_names = new Dictionary<int, string>();

    public void ClearAll()
    {
        _officilMap.Clear();
        _chapter_ids.Clear();
        _officil_chapter_names.Clear();
    }

    public void ClearChapter(int chapter_id)
    {
        if(_officilMap.ContainsKey(chapter_id))
        {
            _officilMap[chapter_id].Clear();
        }        
    }
    public void ReloadAll()
    {

    }
	public List<message.CrashMapData> getChapterMaps(int chapter)
	{
		List<message.CrashMapData> temp = new List<message.CrashMapData>();
		if (_officilMap.ContainsKey(chapter) == true)
		{
			Dictionary<int, message.CrashMapData> dic = _officilMap[chapter];
			foreach(KeyValuePair<int, message.CrashMapData> key_pair in dic)
			{
				temp.Add(key_pair.Value);
			}
		}
		return temp;
	}
	public message.CrashMapData getOfficilMap(int page, int number)
	{
		message.CrashMapData temp = null;
		if (_officilMap.ContainsKey(page) == true)
		{
			temp = _officilMap[page][number];
		}
		return temp;
	}

	public void LoadSections ()
	{
		message.MsgC2SOfficeStatusReq msg = new message.MsgC2SOfficeStatusReq ();	
		global_instance.Instance._net_client.send (msg);
        _officilMap.Clear();
        _chapter_ids.Clear();
    }

	public void parseOfficeStatus(message.MsgS2COfficeStatusACK msg)
	{

		foreach (int secion_id in msg.chapter_id) 
		{
			_chapter_ids.Add (secion_id);
		}

		if (_chapter_ids.Count != 0) 
		{
			message.MsgC2SOfficeMapReq MsgReq = new message.MsgC2SOfficeMapReq ();
			MsgReq.chapter_id = _chapter_ids [0];
			MsgReq.section_id = -1;
            MsgReq.map_count = 5;
			global_instance.Instance._net_client.send (MsgReq);
		}
		else
		{
			endOfficilMapLoad ();
		}
	}

	public void parseOfficeChaper(message.MsgS2COfficeMapACK msg)
	{
		int chapter_id = msg.chapter_id;
		int session_id = 0;

        if(_officilMap.ContainsKey(chapter_id) == false)
        {
            _officilMap.Add(chapter_id, new Dictionary<int, message.CrashMapData>());
        }

		if (_officilMap.ContainsKey (chapter_id) == true) 
		{
			foreach (message.CrashMapData entry in msg.maps) 
			{
				_officilMap[chapter_id][entry.Section] = entry;	
				session_id = entry.Section;
			}
		}
		int req_chapter_id = -1;
		int req_section_id = -1;
		if (msg.section_count == _officilMap [chapter_id].Count) 
		{
			bool next_chapter_id = false;
			foreach (int chapter_id_entry in _chapter_ids) 
			{
				if (next_chapter_id == false) 
				{
					if (chapter_id == chapter_id_entry) 
					{
						next_chapter_id = true;
					}
				} 
				else 
				{
					req_chapter_id = chapter_id_entry;
					break;
				}
			}
		} 
		else 
		{
			req_chapter_id = chapter_id;
			req_section_id = session_id;
		}

		if (req_chapter_id == -1 && req_section_id == -1) 
		{
			endOfficilMapLoad ();
		}
		else 
		{
			if (chapter_id != -1) 
			{
				message.MsgC2SOfficeMapReq MsgReq = new message.MsgC2SOfficeMapReq ();
				MsgReq.chapter_id = req_chapter_id;
				MsgReq.section_id = req_section_id;
				global_instance.Instance._net_client.send (MsgReq);
			}
		}
	}

	public void addMap(message.CrashMapData map)
	{
		if (_officilMap.ContainsKey (map.Section) == false) 
		{
			Dictionary<int, message.CrashMapData> dic_maps = new Dictionary<int, message.CrashMapData> ();
			_officilMap [map.Chapter] = dic_maps;
		}
		_officilMap [map.Chapter] [map.Section] = map;
	}

	public void delMap(string name)
	{
		foreach(KeyValuePair<int, Dictionary<int, message.CrashMapData>> list_entry in _officilMap)
		{
			foreach(KeyValuePair<int, message.CrashMapData> key_entry in list_entry.Value)
			{
				if(key_entry.Value.MapName == name)
				{
					list_entry.Value.Remove(key_entry.Key);
					break;
				}
			} 
		}
	}

	public void addChapterName(int section, string chapter_name)
	{
		_officil_chapter_names[section] = chapter_name;
	}

	public Dictionary<int, string> getChapterNames()
	{
		return _officil_chapter_names;
	}

	protected void endOfficilMapLoad()
	{
        if(global_instance.Instance._in_login)
        {
            global_instance.Instance._ngui_edit_manager._login_obj.SetActive(false);
            global_instance.Instance._ngui_edit_manager.show_main_panel();
        }
        global_instance.Instance._in_login = false;

    }




}