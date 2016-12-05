using UnityEngine;
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
    private int _section_number;
    MainPanel()
    {
        _current_page = page_type.page_type_official;
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

    public void PlayClick()
    {
        global_instance.Instance._global_game_type = global_game_type.global_game_type_game;
        message.CrashMapData MapDataTemp = null;
        if (_current_page == page_type.page_type_official)
        {
            MapDataTemp = global_instance.Instance._player.getOfficilMap(_section_number, (int)_current_map_index);
        }
        else
        {
            MapDataTemp = GetCurrentSelectMapData();
        }
        
        if(MapDataTemp != null)
        {
            MapData temp = new MapData();
            temp.set_info(MapDataTemp);
            global_instance.Instance.SetMapData(temp);
            global_instance.Instance._crash_mole_grid_manager.set_max_height(temp.height_);
            global_instance.Instance._crash_mole_grid_manager.set_max_width(temp.width_);
            global_instance.Instance._ngui_edit_manager._main_panel.gameObject.SetActive(false);
            global_instance.Instance._ngui_edit_manager.update_game_type(game_type.game);
        }
    }
    public void CreateClick()
    {
        global_instance.Instance._global_game_type = global_game_type.global_game_type_edit;
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.create);
        this.gameObject.SetActive(false);
    }

    message.CrashMapData GetCurrentSelectMapData()
    {
        CrashMapData MapData = null;
        if (_current_map_index != 0)
        {
            foreach (CrashMapData entry in global_instance.Instance._player.GetInfo().CompleteMap)
            {
                if (entry.Data.map_index == _current_map_index)
                {
                    MapData = entry;
                    break;
                }
            }

            if (MapData == null)
            {
                foreach (CrashMapData entry in global_instance.Instance._player.GetInfo().IncompleteMap)
                {
                    if (entry.Data.map_index == _current_map_index)
                    {
                        MapData = entry;
                        break;
                    }
                }
            }

        }
        return MapData;
    }
    public void EditClick()
    {
        message.CrashMapData MapDataTemp = GetCurrentSelectMapData();
        MapData temp = new MapData();
        temp.set_info(MapDataTemp);
        global_instance.Instance.SetMapData(temp);
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
            entry.transform.parent = null;
            Destroy(entry.gameObject);
        }
        _items.Clear();

        foreach(ChooseItemEntry entry in _officil_items)
        {
            entry.transform.parent = null;
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
        foreach (CrashMapData entry in Info.CompleteMap)
        {
            addItem(entry);
        }
    }

    void EnterSelfIncomplete()
    {
        SelfButtonChange(true);
        CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
        foreach (CrashMapData entry in Info.IncompleteMap)
        {
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
        ChooseItemEntry temp = CreateItemEntry();
        temp._txt_1.text = entry.MapName;
        temp._txt_2.text = entry.CreaterName;
        System.DateTime time = new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(entry.create_time);
        temp._txt_3.text = time.ToString();
        
        MapData temp_ = new MapData();
        temp_.set_info(entry);
        temp.SetTexture(temp_.CreateTexture());
        temp.gameObject.SetActive(true);
        temp.transform.parent = _items_container.transform;
        _items.Add(temp);
        if (self)
        {
            temp._map_index = entry.Data.map_index;
        }
        else
        {
            temp._map_index = (ulong)entry.number;
        }
    }

    void Awake()
    {
        _source_item = Resources.Load<GameObject>("prefab/Button_item");
        _current_page = page_type.page_type_official;
        EnterPage(page_type.page_type_self_complete);
    }

    public void refrashCurrentPage(page_type page)
    {
        _current_map_index = 0;
        ClearItems();
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
                    _edit_section_obj_button.SetActive(true);
                    Dictionary<int, string> officil_section_names = global_instance.Instance._player.get_officil_section_names();
                    foreach (KeyValuePair<int, string> key_temp in officil_section_names)
                    {
                        ChooseItemEntry temp = CreateItemEntry();
                        temp._txt_1.text = key_temp.Key.ToString();
                        temp._txt_2.text = key_temp.Value;
                        temp._map_index = (ulong)key_temp.Key;
                        temp.transform.parent = _officil_items_container.transform;
                        temp.gameObject.SetActive(true);
                        _officil_items.Add(temp);
                    }
                }
                break;
        }
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
        CrashPlayerInfo info = global_instance.Instance._player.GetInfo();
        selectTile((int)page);
        refrashCurrentPage(page);
    }


    void Start()
    {


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
                    _section_number = (int)_current_map_index;
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
                List<CrashMapData> list_maps = global_instance.Instance._player.getPageMaps((int)_current_map_index);
                _play_obj_button.SetActive(true);
                _create_obj_button.SetActive(false);
                _edit_obj_button.SetActive(false);
                _edit_section_obj_button.SetActive(false);
                _Back_obj_button.SetActive(true);
                foreach (ChooseItemEntry entry_temp in _officil_items)
                {
                    entry_temp.transform.parent = null;
                    Destroy(entry_temp.gameObject);
                }               
                _officil_items.Clear();
                foreach(CrashMapData entry_map in list_maps)
                {
                    addItem(entry_map, false);
                }
                _current_map_index = 0;
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
