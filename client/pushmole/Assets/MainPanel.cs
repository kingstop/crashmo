using UnityEngine;
using System.Collections;
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
public class MainPanel : MonoBehaviour {

	// Use this for initialization
    public ChooseItemEntry[] _choose_Item_entry;
    public Button[] _unselected;
    public Button[] _selected;
    public int _page_count;
    public GameObject _items_container;
    private List<ChooseItemEntry> _items = new List<ChooseItemEntry>();
    private page_type current_page = page_type.page_type_self_complete;
    private GameObject _source_item;
   
    void selectTile(int index)
    {
        int titile_count  = _unselected.Length;
        for(int i  = 0; i < titile_count; i ++)
        {
            if(i == index)
            {
                _unselected[i].gameObject.SetActive(false);
                _selected[i].gameObject.SetActive(true);
            }
            else
            {
                _unselected[i].gameObject.SetActive(true);
                _selected[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayClick()
    {
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.game);
    }
    public void CreateClick()
    {
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.create);
        this.gameObject.SetActive(false);
    }

    public void EditClick()
    {
        global_instance.Instance._ngui_edit_manager.update_game_type(game_type.edit);
        this.gameObject.SetActive(false);
    }
    void ClearItems()
    {
        foreach(ChooseItemEntry entry in _items)
        {
            entry.transform.parent = null;
            Destroy(entry.gameObject);
        }
        _items.Clear();
    }

    void EnterSelfComplete()
    {        
        CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();           
        foreach(CrashMapData entry in Info.CompleteMap)
        {
            addSelfItem(entry);
        }
    }

    void EnterSelfIncomplete()
    {
        CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
        foreach (CrashMapData entry in Info.IncompleteMap)
        {
            addSelfItem(entry);
        }
    }

    ChooseItemEntry CreateItemEntry()
    {
        GameObject obj_temp = Object.Instantiate<GameObject>(_source_item);
        ChooseItemEntry entry = obj_temp.GetComponent<ChooseItemEntry>();
        return entry;
    }
    void addSelfItem(CrashMapData entry)
    {
        ChooseItemEntry temp = CreateItemEntry();
        temp._txt_1.text = entry.MapName;
        temp._txt_2.text = entry.CreaterName;
        System.DateTime time = new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(entry.create_time);
        temp._txt_3.text = time.ToString();
        temp._map_index = entry.Data.map_index;
        temp.transform.parent = _items_container.transform;
        _items.Add(temp);
    }
    
    void Awake()
    {
        _source_item = Resources.Load<GameObject>("prefab/Button_item");
    }
    void EnterPage(page_type page)
    {
        ClearItems();
        CrashPlayerInfo info = global_instance.Instance._player.GetInfo();
        switch(page)
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
        }
    }


	void Start () {
        selectTile(0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ItemButtonClick(Button btn)
    {


    }

    public void TitleButtionClick(Button btn)
    {
        int count = _unselected.Length;
        int click_buttion_index = -1;
        for (int i = 0; i < count; i ++ )
        {
            if(_unselected[i] == btn)
            {
                click_buttion_index = i;
                break;
            }
        }

        if(click_buttion_index != -1)
        {
            selectTile(click_buttion_index);
        }
    }
}
