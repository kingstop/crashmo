using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelChapter : MonoBehaviour {
    public Grid grid;
    public WrapContent wrap;
    public GameObject prefab;
	public GameObject _container;
    List<RectTransform> _allItem = new List<RectTransform>();
	protected page_type _current_page;

    void Awake()
    {

    }

	public void setMapType(page_type type)
	{
		_current_page = type;
	}

	public void clearAllItem()
	{
		_allItem.Clear ();
	}
		
    // Use this for initialization
    void Start () {

		Dictionary<int, string> officil_section_names = global_instance.Instance._officilMapManager.getChapterNames();
		foreach (KeyValuePair<int, string> key_temp in officil_section_names)
		{
			GameObject obj = Instantiate(prefab);

			ChapterButton btn = obj.GetComponent<ChapterButton> ();
			btn._txt_1= key_temp.Key.ToString();
			btn._txt_2 = key_temp.Value;
			btn._map_index = (ulong)key_temp.Key;
			obj.transform.SetParent(_container.transform);
			obj.gameObject.SetActive(true);
			obj.transform.localScale = Vector3.one;
			_allItem.Add((RectTransform)obj.transform);
			//_officil_items.Add(temp);
		}
		/*
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(wrap.transform);
            obj.transform.localScale = Vector3.one;
            obj.name = i.ToString();
            obj.transform.Find("Text").GetComponent<Text>().text = i.ToString();
			_allItem.Add((RectTransform)obj.transform);
			obj.SetActive (true);
        }
        */
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
