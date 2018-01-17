using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Test : MonoBehaviour {

    public Grid grid;
    public WrapContent wrap;
    public GameObject prefab;
    List<RectTransform> allItem = new List<RectTransform>();
    void Awake()
    {

        for (int i = 0; i < 15; i++)
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
        wrap.SetDirty();
        Vector2 ve = new Vector2(960, 1000);
        wrap.CenterItem();
        wrap.AutoCenter(ve);

    }
    void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
