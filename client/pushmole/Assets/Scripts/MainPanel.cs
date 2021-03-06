﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using message;

public enum page_type
{
    page_type_self_complete,
    page_type_self_incomplete,
    page_type_rank,
    page_type_official,
}

public enum offcil_page_type
{
    offcil_page_type_no,
    offcil_page_type_section,
    offcil_page_type_number
}

public enum containers_type_panel
{
    containers_type_panel_main,
    containers_type_panel_officil
}
public class MainPanel : MonoBehaviour
{
    // Use this for initialization
    public GameObject[] _obj_containers;
    public ChooseItemEntry[] _choose_Item_entry;
    public GameObject _play_obj_button;
    public GameObject _create_obj_button;
    public GameObject _edit_obj_button;
    public GameObject _Back_obj_button;
    public GameObject _edit_task_obj;
    public GameObject _edit_section_obj_button;
    public Button[] _title_button;
    public int _page_count;
    public GameObject _items_container;
    public GameObject _officil_items_container;
    private List<ChooseItemEntry> _items = new List<ChooseItemEntry>();
    private List<ChooseItemEntry> _officil_items = new List<ChooseItemEntry>();
    //private page_type current_page = page_type.page_type_self_complete;
    private GameObject _source_item;
    private ulong _current_map_index;
    public page_type _current_page;
    private offcil_page_type _officil_page_type;
    private int _chapter_id;
	private int _section_id;
    MainPanel()
    {
		_current_page = page_type.page_type_self_complete;
    }

   
    protected void OnRefresh()
    {
        foreach(ChooseItemEntry entry in _items)
        {
            entry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        foreach (ChooseItemEntry entry in _officil_items)
        {
            entry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
    
	public int GetChapterID()
	{
		return _chapter_id;
	}
	public ulong getSection()
	{
		return _current_map_index;
	}

	public page_type getPageType()
	{
		return _current_page;
	}

    void selectTile(int index)
    {
        int titile_count = _title_button.Length;
        for (int i = 0; i < titile_count; i++)
        {
            MainPanelTitleButtonItem temp = _title_button[i].GetComponent<MainPanelTitleButtonItem>();
            if (i == index)
            {
                temp.Select();
            }
            else
            {
                temp.Unselect();
            }
        }
        _current_page = (page_type)index;
    }

    public void OnUpdateMapInfo()
    {
        global_instance.Instance._global_game_type = global_game_type.global_game_type_game;
        message.CrashMapData MapDataTemp = null;

        if (_current_page == page_type.page_type_official)
        {

            MapDataTemp = global_instance.Instance._officilMapManager.getOfficilMap(_chapter_id, (int)_current_map_index);
        }
        else
        {
            MapDataTemp = GetCurrentSelectMapData();
        }

        if (MapDataTemp != null)
        {
            MapData temp = new MapData();
            temp.set_info(MapDataTemp);
            global_instance.Instance.SetMapData(temp);
            global_instance.Instance._crash_mole_grid_manager.set_max_height(temp.height_);
            global_instance.Instance._crash_mole_grid_manager.set_max_width(temp.width_);
            global_instance.Instance._ngui_edit_manager._main_panel.gameObject.SetActive(false);
        }

    }
    public void PlayClick()
    {
        OnUpdateMapInfo();
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.game);
        global_instance.Instance._global_game_type = global_game_type.global_game_type_game;
    }
    public void CreateClick()
    {        
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.create);
        this.gameObject.SetActive(false);
		global_instance.Instance._global_game_type = global_game_type.global_game_type_edit;
    }

    public void OnTaskEditClick()
    {
        global_instance.Instance._ngui_edit_manager._task_edit_panel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnSaveClick()
    {
        global_instance.Instance._officilMapManager.SaveOfficilMap();
    }

    message.CrashMapData GetCurrentSelectMapData()
    {
        CrashMapData MapData = null;
        if (_current_map_index != 0)
        {
            MapData = global_instance.Instance._player.getUserMap(_current_map_index);
        }
        return MapData;
    }
    public void EditClick()
    {
        OnUpdateMapInfo();
        global_instance.Instance._global_game_type = global_game_type.global_game_type_edit;
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.edit);
        this.gameObject.SetActive(false);
    }

    public void BackClick()
    {
        EnterToContainer(containers_type_panel.containers_type_panel_officil);
        refrashCurrentPage(_current_page);
    }


    public void EditSectionClick()
    {
        global_instance.Instance._ngui_edit_manager._sectionEditPanel.setActive(true);
        this.gameObject.SetActive(false);       
    }

    void ClearItems()
    {
        foreach (ChooseItemEntry entry in _items)
        {
            entry.transform.SetParent(null);
            Destroy(entry.gameObject);
        }
        _items.Clear();

        foreach(ChooseItemEntry entry in _officil_items)
        {
            entry.transform.SetParent(null);
            Destroy(entry.gameObject);
            
        }
        _officil_items.Clear();
    }

    void SelfButtonChange(bool b)
    {
        _play_obj_button.SetActive(b);
        _create_obj_button.SetActive(b);
        _edit_obj_button.SetActive(b);
    }
    void EnterSelfComplete()
    {
        SelfButtonChange(true);
        CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
        foreach (ulong id in Info.CompleteMap)
        {
            CrashMapData entry =global_instance.Instance._player.getUserMap(id);
            addItem(entry);
        }
    }

    void EnterSelfIncomplete()
    {
        SelfButtonChange(true);
        CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
        foreach (ulong id in Info.IncompleteMap)
        {
            CrashMapData entry = global_instance.Instance._player.getUserMap(id);
            addItem(entry);
        }
    }

    ChooseItemEntry CreateItemEntry()
    {
        GameObject obj_temp = Object.Instantiate<GameObject>(_source_item);
        ChooseItemEntry entry = obj_temp.GetComponent<ChooseItemEntry>();
        entry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        return entry;
    }


    void addItem(CrashMapData entry, bool self = true)
    {
        if(entry == null)
        {
            return;
        }
        ChooseItemEntry temp = CreateItemEntry();

        System.DateTime time = new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(entry.create_time);
        temp._txt_3.text = time.ToString();
        
        MapData temp_ = new MapData();
        temp_.set_info(entry);
        temp.SetTexture(temp_.CreateTexture());
        temp.gameObject.SetActive(true);
		temp.transform.SetParent(_items_container.transform);
        _items.Add(temp);
        if (self)
        {
            temp._map_index = entry.Data.map_index;
			temp._txt_1.text = entry.MapName;
			temp._txt_2.text = entry.CreaterName;
        }
        else
        {
			temp._txt_1.text = entry.Section.ToString();
			temp._txt_2.text = entry.MapName;
            temp._map_index = (ulong)entry.Section;
        }
    }

    void Awake()
    {
		RectTransform parentRectTranform = _items_container.GetComponentInParent<RectTransform> ();
		RectTransform[] cur = _items_container.GetComponents<RectTransform> ();
		if (cur.Length != 0) 
		{
			cur [0] = parentRectTranform;
		}

		parentRectTranform = _officil_items_container.GetComponentInParent<RectTransform> ();
		if (cur.Length != 0) 
		{
			cur [0] = parentRectTranform;
		}

		cur = _officil_items_container.GetComponents<RectTransform> ();
        _source_item = Resources.Load<GameObject>("prefab/Button_item");
        _current_page = page_type.page_type_official;
        EnterPage(page_type.page_type_self_complete);
    }

    public void refrashCurrentPage(page_type page)
    {
        _current_map_index = 0;
        ClearItems();
		_edit_section_obj_button.SetActive(false);

        switch (page)
        {
            case page_type.page_type_self_complete:
                {
                    EnterSelfComplete();

                }
                break;
            case page_type.page_type_self_incomplete:
                {
                    EnterSelfIncomplete();
                }
                break;
            case page_type.page_type_rank:
                {

                }
                break;
            case page_type.page_type_official:
                {
                    _officil_page_type = offcil_page_type.offcil_page_type_section;
                    SelfButtonChange(false);
					
                    _Back_obj_button.SetActive(false);
					if (global_instance.Instance._player.isadmin ()) 
					{
						_edit_section_obj_button.SetActive(true);
					}                    
					Dictionary<int, string> officil_section_names = global_instance.Instance._officilMapManager.getChapterNames();
                    foreach (KeyValuePair<int, string> key_temp in officil_section_names)
                    {
                        ChooseItemEntry temp = CreateItemEntry();
                        temp._txt_1.text = key_temp.Key.ToString();
                        temp._txt_2.text = key_temp.Value;
                        temp._map_index = (ulong)key_temp.Key;
                        temp.transform.SetParent(_officil_items_container.transform);
                        temp.gameObject.SetActive(true);
                        _officil_items.Add(temp);
                    }
                }
                break;
        }
        OnRefresh();
    }
    void EnterPage(page_type page)
    {

        if (_current_page == page)
        {
            return;
        }
        if (page == page_type.page_type_official && _current_page != page_type.page_type_official)
        {
            EnterToContainer(containers_type_panel.containers_type_panel_officil);
        }
        else if (page != page_type.page_type_official && _current_page == page_type.page_type_official)
        {
            EnterToContainer(containers_type_panel.containers_type_panel_main);
        }
        selectTile((int)page);
        refrashCurrentPage(page);
    }


    void Start()
    {
		
        if(global_instance.Instance._player.isadmin())
        {
            _edit_task_obj.SetActive(true);
        }
        else
        {
            _edit_task_obj.SetActive(false);
        }

        
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void ItemButtonClick(ChooseItemEntry entry)
    {
        List<ChooseItemEntry> temp_list = null;
        bool b_set_section = false;
        if(_current_page == page_type.page_type_official && _officil_page_type == offcil_page_type.offcil_page_type_section)
        {
            b_set_section = true;
            temp_list = _officil_items;
        }
        else
        {
            temp_list = _items;
        }

        foreach (ChooseItemEntry temp in temp_list)
        {
            if (temp == entry)
            {
                temp.select();
                _current_map_index = temp._map_index;
                if (b_set_section)
                {
					_chapter_id = (int)_current_map_index;
                }

            }
            else
            {
                temp.unselect();
            }
        }

        if (_current_page == page_type.page_type_official)
        {
            if (_officil_page_type == offcil_page_type.offcil_page_type_section)
            {
                EnterToContainer(containers_type_panel.containers_type_panel_main);
                _officil_page_type = offcil_page_type.offcil_page_type_number;
				List<CrashMapData> list_maps = global_instance.Instance._officilMapManager.getChapterMaps((int)_current_map_index);
				bool is_admin = false;
				if (global_instance.Instance._player.isadmin ()) 
				{
					is_admin = true;
				}
				_edit_obj_button.SetActive(is_admin);
				_create_obj_button.SetActive(is_admin);
                _play_obj_button.SetActive(true);
                _edit_section_obj_button.SetActive(false);
                _Back_obj_button.SetActive(true);
                foreach (ChooseItemEntry entry_temp in _officil_items)
                {
					entry_temp.transform.SetParent(null);
                    Destroy(entry_temp.gameObject);
                }               
                _officil_items.Clear();
                foreach(CrashMapData entry_map in list_maps)
                {
                    addItem(entry_map, false);
                }
                _current_map_index = 0;
                OnRefresh();
            }

        }
    }

    private void EnterToContainer(containers_type_panel panel_type)
    {
        int count_temp = _obj_containers.Length;
        for (int i = 0; i < count_temp; i++)
        {
            if (i == (int)panel_type)
            {
                _obj_containers[i].SetActive(true);
            }
            else
            {
                _obj_containers[i].SetActive(false);
            }
        }
        if(panel_type == containers_type_panel.containers_type_panel_main)
        {
            if(_current_page == page_type.page_type_official)
            {
                SelfButtonChange(false);
                _Back_obj_button.SetActive(false);
                _play_obj_button.SetActive(false);
                _edit_section_obj_button.SetActive(true);
            }
            else
            {
                _play_obj_button.SetActive(true);
                _create_obj_button.SetActive(true);
                _edit_obj_button.SetActive(true);
                _Back_obj_button.SetActive(false);
                _edit_section_obj_button.SetActive(false);
            }
        }
    }

    public page_type GetPageType()
    {
        return _current_page;
    }
    public void TitleButtionClick(Button btn)
    {
        int count = _title_button.Length;
        int click_buttion_index = -1;
        for (int i = 0; i < count; i++)
        {
            if (_title_button[i] == btn)
            {
                click_buttion_index = i;
                break;
            }
        }

        if (click_buttion_index != -1)
        {
            EnterPage((page_type)click_buttion_index);

        }
    }
}
