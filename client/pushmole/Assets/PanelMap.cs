using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMap : MonoBehaviour {
    public Grid grid;
    public WrapContent wrap;
    public GameObject prefab;
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

	public void init()
	{
		switch (_current_page) 
		{
		case page_type.page_type_official:
			break;

		case page_type.page_type_rank:
			break;

		case page_type.page_type_self_complete:
			break;

		case page_type.page_type_self_incomplete:
			break;
		}
	}

    // Use this for initialization
    void Start () {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(wrap.transform);
            obj.transform.localScale = Vector3.one;
            obj.name = i.ToString();
            obj.transform.Find("Text").GetComponent<Text>().text = i.ToString();
			_allItem.Add((RectTransform)obj.transform);
        }
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
