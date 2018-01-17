using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;                  
[SelectionBase]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class Grid : MonoBehaviour {
    public enum Axis{Horizontal = 0, Vertical = 1}
    public enum Corner { 
        UpperLeft,
        UpperRightCenter, 
        UpperRight,
        MiddleCenter,
        LowerLeft,
        LowerLeftCenter,
        LowerRight}
    [SerializeField] private Axis m_startAxis = Axis.Vertical;
    public Axis startAxis{ get { return m_startAxis; }}
    [SerializeField] private Corner m_startCorner = Corner.UpperLeft;
    public Corner startCorner{ get { return m_startCorner; }}
    [SerializeField] private bool m_modifyChildSize = true;
    [SerializeField] private Vector2 m_size = new Vector2(100, 100);
    public Vector2 size{ get { return m_size + interval; }}
    [SerializeField] public Vector2 interval = Vector2.zero;
    private List<RectTransform> childList = new List<RectTransform>();
    private RectTransform rectTransform;
    public RectTransform rect { get { return rectTransform; } }
    private ScrollRect m_scrollRect;
    void Awake()
    {
        rectTransform = (RectTransform)transform;
        m_scrollRect = GetComponentInParent<ScrollRect>();

    }
    void Start(){
        SetDirty();
        m_scrollRect.horizontal = m_startAxis == Axis.Horizontal;
        m_scrollRect.vertical = m_startAxis == Axis.Vertical;
    }

    public void RePosition()
    {
        if(rectTransform == null)
        {
            return;
        }
        rectTransform.pivot = GetPivot();
       
        rectTransform.sizeDelta = GetSize();
        for (int i = 0; i < childList.Count; i++)
        {
            RectTransform item = childList[i];
            Vector2 anchor = GetChildPivot();
            if (m_modifyChildSize)
                item.sizeDelta = m_size;
            item.pivot = anchor;
            item.anchorMin = anchor;
            item.anchorMax = anchor;
            item.anchoredPosition = GetOffset(i);

        }
    }
   
    Vector2 GetPivot()
    {
        Vector2 pivot=Vector2.zero;
        switch (m_startCorner)
        {
            case Corner.UpperLeft:
                pivot = new Vector2(0, 1f);
                break;
            case Corner.UpperRightCenter:
                if(m_startAxis== Axis.Vertical)
                    pivot= new Vector2(0.5f, 1f);
                else
                    pivot = new Vector2(1f, 0.5f);
                break;
            case Corner.UpperRight:
                pivot= new Vector2(1f, 1f);
                break;
            case Corner.MiddleCenter:
                pivot = Vector2.one * 0.5f;
                break;
            case Corner.LowerLeft:
                pivot= new Vector2(0f, 0f);
                break;
            case Corner.LowerLeftCenter:
                if (m_startAxis == Axis.Vertical)
                    pivot = new Vector2(0.5f, 0f);
                else
                    pivot = new Vector2(0f, 0.5f);

                break;
            case Corner.LowerRight:
                pivot = new Vector2(1f, 0f);
                break;
        }
        return pivot;
    }
    Vector2 GetChildPivot(){
        switch (m_startCorner)
        {
            case Corner.UpperLeft: return new Vector2(0f, 1f);
            case Corner.UpperRight:
            case Corner.UpperRightCenter: return Vector2.one;
            case Corner.LowerLeft:
            case Corner.LowerLeftCenter: return Vector2.zero;
            case Corner.LowerRight: return new Vector2(1f, 0f);
            case Corner.MiddleCenter:return Vector2.one * 0.5f;
            default:return Vector2.zero;
        }
    }
   

    int GetDir(){
        int dir = 1;
        Vector2 d = Vector2.one;
        if(m_startAxis== Axis.Vertical)
        {
            switch (m_startCorner)
            {
                case Corner.UpperLeft:
                case Corner.UpperRightCenter:
                case Corner.UpperRight:
                    dir = -1;
                    break;
            }
        }else if(m_startAxis== Axis.Horizontal)
        {
            switch (m_startCorner)
            {
                case Corner.UpperRight:
                case Corner.LowerRight:
                case Corner.UpperRightCenter:
                    dir = -1;
                    break;
            }
        }
        
        return dir;
    }
    Vector2 GetSize() 
    {
        Vector2 _size = size;
        if (m_scrollRect != null && m_scrollRect.movementType == ScrollRect.MovementType.Unrestricted)
            return _size;
            switch (m_startAxis)
        {
            case Axis.Horizontal:
                _size = new Vector2(childList.Count * size.x, m_size.y);
                break;
            case Axis.Vertical:
                _size = new Vector2(m_size.x, childList.Count * size.y);
                break;
        }
        return _size;
    }
    Vector2 GetOffset(int currIndex)
    {
        if (m_startCorner == Corner.MiddleCenter)
            return GetCenterOffset(currIndex);
        Vector2 offset = Vector2.zero;
        if (m_startAxis == Axis.Vertical)
            offset.y = size.y * currIndex;
        if (m_startAxis == Axis.Horizontal)
            offset.x = size.x * currIndex;
        return offset * GetDir();
    }

    Vector2 GetCenterOffset(int currIndex){
        Vector2 offset = Vector2.zero;
	    if (m_startAxis == Axis.Horizontal)
	        offset.x = size.x;
	    if (m_startAxis == Axis.Vertical)
	        offset.y = -size.y;
	    Vector2 delta = offset * 0.5f;
	    Vector2 pos = delta * childList.Count - delta;
	    return currIndex * offset - pos; 
    }

    public void SetDirty(){
        
        if (childList == null) return;
        childList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform _rect = (RectTransform)transform.GetChild(i);
            if(_rect.gameObject.activeSelf){
                childList.Add(_rect);
            }

        }
        RePosition();
    }





}
