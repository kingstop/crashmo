using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using UnityEngine;

public class OfficilMapManager
{
    protected Dictionary<int, Dictionary<int, message.CrashMapData>> _officilMap = new Dictionary<int, Dictionary<int, message.CrashMapData>>();
	protected List<int> _chapter_ids = new List<int> ();
	protected Dictionary<int, string> _officil_chapter_names = new Dictionary<int, string>();
    protected List<KeyValuePair<int, int>> _new_map = new List<KeyValuePair<int, int>>();
    protected List<KeyValuePair<int, int>> _remove_map = new List<KeyValuePair<int, int>>();

    public void ClearAll()
    {
        _officilMap.Clear();
        _chapter_ids.Clear();
        _officil_chapter_names.Clear();
    }

    public void LoadFromLocal()
    {
        _officilMap.Clear();
        ArrayList array_list =  global_instance.Instance._file_helper.LoadFile(Application.persistentDataPath, "MapName.txt");
        if(array_list != null)
        {
            if (array_list.Count != 0)
            {
                string map_names = (string)array_list[0];
                string[] map_name_array = map_names.Split(' ');
                foreach (string entry in map_name_array)
                {
                    ArrayList temp_list = global_instance.Instance._file_helper.LoadFile(Application.persistentDataPath, entry + "map.txt");
                    if(temp_list != null)
                    {
                        string body_str = "";
                        for (int i = 0; i < temp_list.Count; i++)
                        {
                            body_str += (string)temp_list[i];
                        }
                        byte[] body = Convert.FromBase64String(body_str);
                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        mem.Write(body, 0, body.Length);
                        message.CrashMapData CrashMap = ProtoBuf.Serializer.Deserialize<message.CrashMapData>(mem);
                        if (_officilMap.ContainsKey(CrashMap.Chapter) == false)
                        {
                            _officilMap[CrashMap.Chapter] = new Dictionary<int, message.CrashMapData>();
                        }
                        _officilMap[CrashMap.Chapter][CrashMap.Section] = CrashMap;
                        //global_instance.Instance._file_helper.Loadbin(Application.persistentDataPath, entry + ".assetbundle", LoadBinMap);
                    }

                }
            }
        }
    }

    public void UpdateLocalMap()
    {
        string name_map;
        foreach (KeyValuePair<int, int> entry_remove in _remove_map)
        {
            name_map = entry_remove.Key.ToString() + "-" + entry_remove.Value.ToString();
            global_instance.Instance._file_helper.DeleteFile(Application.persistentDataPath, name_map + "map.txt");
        }
        global_instance.Instance._file_helper.DeleteFile(Application.persistentDataPath, "MapName.txt");
        _remove_map.Clear();
        string str_names = "";
        name_map = "";
        foreach (KeyValuePair<int, Dictionary<int, message.CrashMapData>> entry_chapter_key_pair in _officilMap)
        {
            Dictionary<int, message.CrashMapData> entry_chapter = entry_chapter_key_pair.Value;
            foreach (KeyValuePair<int, message.CrashMapData> entry_pair in entry_chapter)
            {
                if (str_names != "")
                {
                    str_names += " ";
                }
                message.CrashMapData map = entry_pair.Value;
                KeyValuePair<int, int> pair_entry_new = new KeyValuePair<int, int>(map.Chapter, map.Section);

                if(_new_map.Contains(pair_entry_new))
                {
                    int chapter_id = map.Chapter;
                    int section = map.Section;
                    name_map = chapter_id.ToString() + "-" + section.ToString();
                    str_names += name_map;
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    ProtoBuf.Serializer.Serialize<global::ProtoBuf.IExtensible>(mem, map);
                    byte[] bytes = mem.ToArray();
                    string temp_base64 = Convert.ToBase64String(bytes);
                    global_instance.Instance._file_helper.CreateFile(Application.persistentDataPath, name_map + "map.txt", temp_base64);
                }                
            }
        }
        _new_map.Clear();
        global_instance.Instance._file_helper.CreateFile(Application.persistentDataPath, "MapName.txt", str_names);

    }
    public void SaveOfficilMap()
    {
        DelOfficilMap();
        string str_names = "";
        string name_map;
        foreach(KeyValuePair< int, Dictionary < int, message.CrashMapData >> entry_chapter_key_pair in _officilMap)
        {
            Dictionary<int, message.CrashMapData> entry_chapter = entry_chapter_key_pair.Value;
            foreach (KeyValuePair<int, message.CrashMapData> entry_pair in entry_chapter)
            {
                if(str_names != "")
                {
                    str_names += " ";
                }
                message.CrashMapData map = entry_pair.Value;
                int chapter_id = map.Chapter;
                int section = map.Section;
                name_map = chapter_id.ToString() + "-" + section.ToString();
                str_names += name_map;
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                ProtoBuf.Serializer.Serialize<global::ProtoBuf.IExtensible>(mem, map);
                byte[] bytes =  mem.ToArray();
                UInt32 char_base64_out_length = (UInt32)bytes.Length * 2;
                //char[] base64_out = new char[char_base64_out_length];
                
                string temp_base64 = Convert.ToBase64String(bytes);
                global_instance.Instance._file_helper.CreateFile(Application.persistentDataPath, name_map + "map.txt", temp_base64);
                //global_instance.Instance._file_helper.CreateModelFile(Application.persistentDataPath, name_map + ".assetbundle", bytes, bytes.Length);
            }
        }
        //DeleteFile(Application.persistentDataPath, "MapName.txt");
        global_instance.Instance._file_helper.CreateFile(Application.persistentDataPath, "MapName.txt", str_names);
    }
    public void DelOfficilMap()
    {
        ArrayList entry_array = global_instance.Instance._file_helper.LoadFile(Application.persistentDataPath, "MapName.txt");
        if(entry_array != null)
        {
            if (entry_array.Count != 0)
            {
                string all_file_name = (string)entry_array[0];
                string[] string_array = all_file_name.Split(' ');
                foreach (string entry in string_array)
                {
                    global_instance.Instance._file_helper.DeleteFile(Application.persistentDataPath, entry + "map.txt");
                }
            }
            global_instance.Instance._file_helper.DeleteFile(Application.persistentDataPath, "MapName.txt");
        }
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
		message.MsgC2SOfficeStatusReq msg = new message.MsgC2SOfficeStatusReq();	
		global_instance.Instance._net_client.send(msg);
        _officilMap.Clear();
        _chapter_ids.Clear();
    }

    List<ulong> GetChapterMapIndex(int chapter_id)
    {
        List<ulong> entry_list = new List<ulong>();
        if (_officilMap.ContainsKey(chapter_id) == true)
        {
            Dictionary<int, message.CrashMapData> chapter_sections = _officilMap[chapter_id];
            foreach (KeyValuePair<int, message.CrashMapData> entry in chapter_sections)
            {
                entry_list.Add(entry.Value.Data.map_index);
            }            
        }
        return entry_list;
    }
    
	public void parseOfficeStatus(message.MsgS2COfficeStatusACK msg)
	{
        List<int> no_sections = new List<int>();
        foreach (message.MsgChapterStatus entry in msg.chapter_status)
        {
            _chapter_ids.Add(entry.chapter_id);            
            if(_officilMap.ContainsKey(entry.chapter_id) == true)
            {
                foreach(KeyValuePair<int , message.CrashMapData> self_map_entry in _officilMap[entry.chapter_id])
                {
                    bool need_remove = true;
                    message.CrashMapData temp_map = self_map_entry.Value;
                    ulong map_index = temp_map.Data.map_index;
                    foreach(ulong server_map_index in entry.map_indexs)
                    {
                        if(server_map_index == map_index)
                        {
                            need_remove = false;
                            break;
                        }
                    }
                    if(need_remove)
                    {
                        no_sections.Add(temp_map.Section);
                    }
                }
                foreach(int remove_section_id in no_sections)
                {
                    KeyValuePair<int, int> pair_entry = new KeyValuePair<int, int>(entry.chapter_id, remove_section_id);
                    _remove_map.Add(pair_entry);
                    _officilMap[entry.chapter_id].Remove(remove_section_id);
                }                
            }            
        }
  //      msg.chapter_status

		//foreach (int secion_id in msg.chapter_id) 
		//{
		//	_chapter_ids.Add (secion_id);
		//}

		if (_chapter_ids.Count != 0) 
		{
			message.MsgC2SOfficeMapReq MsgReq = new message.MsgC2SOfficeMapReq ();
			MsgReq.chapter_id = _chapter_ids [0];
            List<ulong> map_indexs = GetChapterMapIndex(MsgReq.chapter_id);
            foreach(ulong map_index in map_indexs)
            {
                MsgReq.map_indexs.Add(map_index);
            }
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
                KeyValuePair<int, int> entry_pair = new KeyValuePair<int, int>(chapter_id, session_id);
                _new_map.Add(entry_pair);
            }
		}
		int req_chapter_id = -1;
		int req_section_id = -1;
		if (msg.maps.Count == 0) 
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
                List<ulong> map_indexs = GetChapterMapIndex(req_chapter_id);
                foreach (ulong map_index in map_indexs)
                {
                    MsgReq.map_indexs.Add(map_index);
                }
                global_instance.Instance._net_client.send (MsgReq);
			}
		}
	}

	public void addMap(message.CrashMapData map)
	{
		if (_officilMap.ContainsKey (map.Chapter) == false) 
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
        UpdateLocalMap();
        if (global_instance.Instance._in_login)
        {
            global_instance.Instance._ngui_edit_manager._login_obj.SetActive(false);
            global_instance.Instance._ngui_edit_manager.show_main_panel();
        }
        global_instance.Instance._in_login = false;

    }




}