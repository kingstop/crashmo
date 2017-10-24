using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 扩展NGUIScroll滑动类，需要继承该类进行开发
/// </summary>
public abstract  class UGUI_DynamicScrollBase : MonoBehaviour
{

    //每个列表项数据初始化
    abstract protected void ResetItemData(GameObject go, int index);
    

    public RectTransform scrollRect;

    public GridLayoutGroup grid;

	public Vector2 direction = new Vector2 (0, 1);

    //目标prefab
    public GameObject prefab;
    //宽度
    public int cellHeight = 60;
    //高度
    public int cellWidth = 700;
    //裁剪区的高度
    private float m_height;
    //裁剪区的宽度
    private int m_maxLine;
    //当前滑动的列表
    protected GameObject[] m_cellList;
    //当前需要滑动的列表总数
    private int m_dataListCount;

    //最后一次的滑动位置
    private float lastY = -1;
    //
    private Vector3 defaultVec;


    // Use this for initialization
    protected void BaseOnEnable()
    {
        GetConfiguration();
    }

    // Update is called once per frame
    protected void BaseUpdate()
	{	
		if (grid.transform.localPosition.y != lastY)
		{	
			lastY = grid.transform.localPosition.y;
            Validate();

        }
    }
    //设置当前列表中的
    protected int DataListCount
    {
        get { return m_dataListCount; }

        set{

            m_dataListCount = value;
            
            AddItem(m_dataListCount);

            PlayMoveAnimation(m_cellList);
        }
    }



    #region private Functions

    //初始化配置数据
    private void GetConfiguration()
    {
        //物体默认位置
        defaultVec = new Vector3(0, cellHeight, 0);
        //裁剪区域的高度
		m_height = scrollRect.rect.height;
        //裁剪区域中最多显示的cellItem数量
        m_maxLine = Mathf.CeilToInt(m_height / cellHeight) + 1;
        //初始化CellList
        m_cellList = new GameObject[m_maxLine];
        //创建Item，默认为不可显示状态
        CreateItem();

    }
    //创建Item
    private void CreateItem()
    {
        for (int i = 0; i < m_maxLine; i++)
        {
            GameObject go = null;
            go = (GameObject)Instantiate(prefab);
            go.gameObject.SetActive(false);
			go.transform.parent = prefab.transform.parent;
            go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector3(cellWidth,cellHeight*-i, 0);
            m_cellList[i] = go;
            go.gameObject.SetActive(false);
        }
    }
	
    private void Validate()
    {
		Vector3 position = grid.GetComponent<RectTransform> ().anchoredPosition;

        float _ver = Mathf.Max(position.y, 0);

        int startIndex = Mathf.FloorToInt(_ver / cellHeight);
	
        int endIndex = Mathf.Min(DataListCount, startIndex + m_maxLine);
        GameObject cell;
        int index = 0;

        for (int i = startIndex; i < startIndex + m_maxLine; i++)
        {
            cell = m_cellList[index];

            if (i < endIndex)
            {
                //开始渲染
                cell.gameObject.SetActive(true);
                //重新填充数据
                ResetItemData(cell, i);
                //改变位置
				((RectTransform)cell.transform).anchoredPosition = new Vector3(i*-cellHeight*direction.x, i * -cellHeight*direction.y);
                cell.name = "Item_" + index;
            }
            else
            {
                cell.transform.localPosition = defaultVec;
                cell.gameObject.SetActive(false);
            }

            index++;
        }
    }


    //根据新的数量来重新绘制
    private void AddItem(int count)
    {
		grid.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(cellWidth, count * cellHeight);
        Validate();
        
    }
    


    //播放开始加载的位移动画
    void PlayMoveAnimation(GameObject[] list)
    {
//        Vector3 to;
//        Vector3 from;
//        for (int i = 0; i < list.Length; i++)
//        {
//            from = list[i].transform.localPosition;
//            from = new Vector3(cellWidth*AllScale.ResolutionScale, from.y, 0);
//            to = new Vector3(0, from.y, 0);
//            list[i].transform.localPosition = from;
//            TweenPosition tp = TweenPosition.Begin(list[i], 0.8f, to);
//            tp.delay = 0.1f;
//            tp.from = from;
//            tp.to = to;
//            tp.duration = (i + 2) * 0.1f;
//            tp.method = UITweener.Method.EaseIn;
//        }
    }

    #endregion


   
}
