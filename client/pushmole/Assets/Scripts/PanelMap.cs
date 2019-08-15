using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using message;

public class PanelMap : MonoBehaviour {
    public UIAnimationText AniTextName_;
    public UIAnimationText AniTextCreater_;
    public UIAnimationText AniTextTime_;
    public Grid grid;
    public WrapContent wrap;
    public GameObject prefab;
    public Button edit_btn_;
    public Button publish_btn_;
    List<RectTransform> _allItem = new List<RectTransform>();
	protected page_type _current_page;
	protected int _charpter_id;
	protected float _center_scale;
    protected ulong _map_index;
    protected bool _show_detail;
    protected float _wave_x;

    public void onMapBtnClick(ulong map_index)
    {
        if(_map_index == map_index)
        {
            CrashMapData info = getMapDate(_map_index);
            if(info != null)
            {
                MapData data = new MapData();
                data.set_info(info);
                global_instance.Instance.SetMapData(data);
                global_instance.Instance._ngui_edit_manager.update_game_type(game_type.game);
            }
            
        }
    }

    public float getExtraWave()
    {
        return _wave_x;
    }

    public void onEditClick()
    {

    }

    public void onPublishClick()
    {

    }

    public void onBackClick()
    {
        global_instance.Instance._ngui_edit_manager.HidMapPanel();
        global_instance.Instance._ngui_edit_manager.show_main_panel();
        
    }


	void setChapterID(int charpter_id)
	{
		_charpter_id = charpter_id;
	}
	
    void setDetailActive(bool b)
    {
        _show_detail = b;
        AniTextName_.gameObject.SetActive(b);
        AniTextCreater_.gameObject.SetActive(b);
        AniTextTime_.gameObject.SetActive(b);
    }

	void EnterOfficilMap()
	{
		clearAllItem ();          
		List<CrashMapData> list_maps = global_instance.Instance._officilMapManager.getChapterMaps(_charpter_id);
		foreach(CrashMapData entry_map in list_maps)
		{
			addItem(entry_map, false);
		}
	}

	void EnterSelfComplete()
	{
		clearAllItem ();
		//SelfButtonChange(true);
		CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
        if(Info != null)
        {
            foreach (ulong id in Info.CompleteMap)
            {
                CrashMapData entry = global_instance.Instance._player.getUserMap(id);
                addItem(entry);
            }
        }
    }

    void setMapName(string name)
    {
        AniTextName_.text_.text = name;
    }

    void setMapCreateTime(string time)
    {
        AniTextTime_.text_.text = time;
    }

    void setMapCreaterName(string name)
    {
        AniTextCreater_.text_.text = name;
    }

    public void onCenterStop(GameObject obj)
    {        
        MapButton btn = obj.GetComponent<MapButton>();
        if(btn != null)
        {
            btn.setCallBack(onMapBtnClick);
            CrashMapData mapdate = getMapDate(btn.getMapIndex());
            if (mapdate != null)
            {
                setMapName(mapdate.MapName);
                setMapCreaterName(mapdate.CreaterName);
                System.DateTime time = new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(mapdate.create_time);
                setMapCreateTime(time.ToString());
                setDetailActive(true);
                _map_index = btn.getMapIndex();
            }
        }        
    }

    public void onCenterCancelStop(GameObject obj)
    {
        setDetailActive(false);
    }

	void EnterSelfIncomplete()
	{
		clearAllItem ();
		//SelfButtonChange(true);
		CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
        if(Info != null)
        {
            foreach (ulong id in Info.IncompleteMap)
            {
                CrashMapData entry = global_instance.Instance._player.getUserMap(id);
                if (entry != null)
                {
                    addItem(entry);
                }
            }
        }
    }

    CrashMapData getMapDate(ulong index)
    {
        CrashMapData entry = null;
        if (_current_page == page_type.page_type_self_complete 
            || _current_page == page_type.page_type_self_incomplete)
        {
            entry = global_instance.Instance._player.getUserMap(index);
        }
        else if(_current_page == page_type.page_type_official)
        {
            entry = global_instance.Instance._officilMapManager.getOfficilMap(_charpter_id, (int)index);
        }
        return entry;
    }

    private void Update()
    {
       // _wave_x += 0.01f;
    }

    void Awake()
    {
        string path_language = global_instance.Instance.GetLanguagePath();
        _center_scale = 1.0f;
    }

	public void setPage(page_type page)
	{
		_current_page = page;
	}

	public void addItem(CrashMapData entry, bool self = true)
	{
        if(entry == null)
        {
            return; 
        }

		System.DateTime time = new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(entry.create_time);
		//temp._txt_3.text = time.ToString();

		MapData temp_ = new MapData();
		temp_.set_info(entry);
		GameObject obj = Instantiate(prefab);
		if (obj != null) 
		{
			obj.transform.SetParent(wrap.transform);
			obj.transform.localScale = Vector3.one;
			obj.name = entry.MapName;
			//obj.transform.Find("Text").GetComponent<Text>().text = i.ToString();
			MapButton entry_map = obj.GetComponent<MapButton>();
            entry_map.setExtraXCallBack(getExtraWave);
            if (entry_map != null) 
			{
				entry_map.SetTexture (temp_.CreateTexture());
				if (self)
				{
					entry_map.setMapIndex (entry.Data.map_index);
					entry_map.setText1 (entry.MapName);
					entry_map.setText2 (entry.CreaterName);
				}
				else
				{
					entry_map.setMapIndex ((ulong)entry.Section);
					entry_map.setText1 (entry.MapName);
					entry_map.setText2 (entry.CreaterName);
				}
			}
			_allItem.Add((RectTransform)obj.transform);			
		}
	}

	public void setMapType(page_type type)
	{
		_current_page = type;
	}

	public void clearAllItem()
	{
		_allItem.Clear ();
	}

	void init()
	{
		switch (_current_page) 
		{
		case page_type.page_type_official:
			EnterOfficilMap ();
			break;

		case page_type.page_type_rank:
			break;

		case page_type.page_type_self_complete:
			EnterSelfComplete ();
			break;

		case page_type.page_type_self_incomplete:
			EnterSelfIncomplete();
			break;
		}
        setDetailActive(false);
    }

    // Use this for initialization
    void Start () {
		init ();
        grid.SetDirty();
        _wave_x = 0;

        wrap.OnValueChange = (RectTransform item, int index, int realIndex) =>
		{

			CenterScaleComponent entry = item.GetComponent<CenterScaleComponent>();
			if(entry)
			{
				_center_scale = entry.getScale();
			}
			else
			{
				_center_scale = 1.0f;
			}
		};

		wrap.CenterChange = (RectTransform item, RectTransform item_old) =>
		{
			Debug.Log("CenterChange");
			if(item != null)
			{
				CenterScaleComponent scale = item.GetComponent<CenterScaleComponent>();
				if(scale == null)
				{
					scale = item.gameObject.AddComponent<CenterScaleComponent>();                
				}
				scale.enabled = true;
				scale.SetContent(wrap);
				scale.setCenter(true);
                scale.setCancelStopFunction(onCenterCancelStop);
                scale.setStopFunction(onCenterStop);
            }

			if(item_old != null)
			{
				CenterScaleComponent old_scale = item_old.GetComponent<CenterScaleComponent>();
				if (old_scale != null)
				{
					//old_scale.gameObject.SetActive(false);
					old_scale.setCenter(false);
					old_scale.enabled = false;
				}
			}
		};

	

		wrap.SetDirty();
		Vector2 ve = new Vector2(960, 1000);
		wrap.CenterItem();
		wrap.AutoCenter(ve);
    }
	
}
