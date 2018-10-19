using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using message;

public class PanelMap : MonoBehaviour {
    public Grid grid;
    public WrapContent wrap;
    public GameObject prefab;
    List<RectTransform> _allItem = new List<RectTransform>();
	protected page_type _current_page;
	protected int _charpter_id;
	void setChapterID(int charpter_id)
	{
		_charpter_id = charpter_id;
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
		foreach (ulong id in Info.CompleteMap)
		{
			CrashMapData entry =global_instance.Instance._player.getUserMap(id);
			addItem(entry);
		}
	}

	void EnterSelfIncomplete()
	{
		clearAllItem ();
		//SelfButtonChange(true);
		CrashPlayerInfo Info = global_instance.Instance._player.GetInfo();
		foreach (ulong id in Info.IncompleteMap)
		{
			CrashMapData entry = global_instance.Instance._player.getUserMap(id);
			addItem(entry);
		}
	}


    void Awake()
    {

    }

	public void setPage(page_type page)
	{
		_current_page = page;
	}

	public void addItem(CrashMapData entry, bool self = true)
	{
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
	}

    // Use this for initialization
    void Start () {
		init ();
        grid.SetDirty();
        wrap.OnValueChange = (RectTransform item, int index, int realIndex) =>
        {

			int count = _allItem.Count;
            if (index < count)
            {
				_allItem[index].Find("Text").GetComponent<Text>().text = realIndex.ToString();
            }
            else
            {
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
