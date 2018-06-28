using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMap : MonoBehaviour {
    public Grid grid;
    public WrapContent wrap;
    public GameObject prefab;
    List<RectTransform> allItem = new List<RectTransform>();

    void Awake()
    {

    }
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 1; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(wrap.transform);
            obj.transform.localScale = Vector3.one;
            obj.name = i.ToString();
            obj.transform.Find("Text").GetComponent<Text>().text = i.ToString();
            allItem.Add((RectTransform)obj.transform);
        }
        grid.SetDirty();
        wrap.OnValueChange = (RectTransform item, int index, int realIndex) =>
        {

            int count = allItem.Count;
            if (index < count)
            {
                allItem[index].Find("Text").GetComponent<Text>().text = realIndex.ToString();
            }
            else
            {
                int c = 0;
                c++;
            }

        };

        wrap.CenterChange = (RectTransform item, RectTransform item_old) =>
        {
            if(item != null)
            {
                CenterScaleComponent scale = item.GetComponent<CenterScaleComponent>();
                if(scale == null)
                {
                    scale = item.gameObject.AddComponent<CenterScaleComponent>();
                    scale.SetContent(wrap);
                }
                scale.enabled = true;
            }

            if(item_old != null)
            {
                CenterScaleComponent old_scale = item_old.GetComponent<CenterScaleComponent>();
                if (old_scale != null)
                {
                    //old_scale.gameObject.SetActive(false);
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
