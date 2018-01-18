using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WrapContent : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public bool needloop_;
    class childConfig
    {
        public RectTransform item;
        public int index;
        public int realIndex;
        public childConfig(RectTransform rect, int index, int realIndex)
        {
            this.item = rect;
            this.index = index;
            this.realIndex = realIndex;
        }
    }
    private ScrollRect m_scrollRect;
    private RectTransform m_ViewRect
    {
        get
        {
            if (m_scrollRect.viewport != null)
                return m_scrollRect.viewport;
            return (RectTransform)m_scrollRect.transform;
        }
    }
    private Vector2 maxSize
    {
        get
        {
            return new Vector2(childList.Count * m_grid.size.x, childList.Count * m_grid.size.y);
        }
    }
    private RectTransform m_rectTransform;
    private Grid m_grid;
    private Canvas m_canvas;
    private childConfig m_frist
    {
        get
        {
            if (childList.Count > 0)
                return childList[0];
            return null;
        }
    }
    private childConfig m_last
    {
        get
        {
            if (childList.Count > 0)
                return childList[childList.Count - 1];
            return null;
        }
    }


    public enum ElasticityType
    {
        OneWayElasticity = 0,
        TwoWayElasticity = 1,
    }
    [Range(0, 1)]
    public float stopTime = 0.2f;
    public bool elasticity = false;
    public bool isCenter = true;
    public ElasticityType elasticityType = ElasticityType.OneWayElasticity;
    public Action<RectTransform, int, int> OnValueChange;
    public Action<RectTransform, RectTransform> CenterChange;
    void Awake()
    {
        m_scrollRect = GetComponentInParent<ScrollRect>();
        if (m_scrollRect != null)
            m_scrollRect.onValueChanged.AddListener(ValueChanged);
        m_rectTransform = (RectTransform)transform;
        m_grid = GetComponent<Grid>();
        m_canvas = GetComponentInParent<Canvas>();

    }
    private void Start()
    {
        m_scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        SetDirty();
    }

    bool isDraging;
    Vector2 oldDelta;
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        ((IInitializePotentialDragHandler)m_scrollRect).OnInitializePotentialDrag(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ((IBeginDragHandler)m_scrollRect).OnBeginDrag(eventData);
       
        isDraging = true;
        canMove = false;
        oldDelta = eventData.delta;
    }
    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)m_scrollRect).OnDrag(eventData);
        oldDelta = eventData.delta;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        ((IEndDragHandler)m_scrollRect).OnEndDrag(eventData);
        isDraging = false;
        if(isCenter){
            Vector2 distance = oldDelta - eventData.delta;
            float max = m_grid.startAxis == Grid.Axis.Horizontal ? distance.x : distance.y;
            float maxVelocity = m_grid.startAxis == Grid.Axis.Horizontal ? m_scrollRect.velocity.x : m_scrollRect.velocity.y;
            if (Mathf.Abs(maxVelocity) <= 2500f)
            {
                CenterItem();
                AutoCenter(Mathf.Sign(maxVelocity) * m_grid.size * 4);
            }
        }

    }
    childConfig oldItem;
    void ValueChanged(Vector2 position)
    {

        float x = Mathf.Abs(m_scrollRect.velocity.x);
        float y = Mathf.Abs(m_scrollRect.velocity.y);
        if (!(Mathf.Max(x, y) > 0)) return;
        if(isCenter){
            timer += Time.smoothDeltaTime;
            CenterItem();
            if (!isDraging)
            {
                if (timer >= stopTime)
                {
                    AutoCenter(m_scrollRect.velocity);

                }
            }
        }

        WrapHandler();
    }
    public void AutoCenter(Vector2 velocity){
        if (currItem == null) return;
        dis = m_grid.startAxis == Grid.Axis.Horizontal ? 0 : 1;
        Vector2 itemPos = GetItemPos(currItem.item.anchoredPosition);
        float a = Mathf.Abs(Mathf.Abs(itemPos[dis]+ AnchorOffset()[dis]) - Mathf.Abs(m_rectTransform.anchoredPosition[dis]));
        target = m_rectTransform.anchoredPosition;
        target[dis] = -(itemPos[dis]+AnchorOffset()[dis]);
        myVelocity = velocity;
        m_scrollRect.StopMovement();
        speed = velocity[dis];
        timer = Mathf.Abs(a / speed);
        canMove = true;
    }
    Vector2 AnchorOffset(){
        Vector2 offset = Vector2.zero;
        offset.x = (m_rectTransform.anchorMax.x - 0.5f) * m_ViewRect.rect.width;
        offset.y = (m_rectTransform.anchorMax.y - 0.5f) * m_ViewRect.rect.height;
        return offset;     
               

    }
   public void CenterItem(){

        childConfig temp_current = currItem;
        Vector2 offset = GetOffset();
        int index = GetCenterIndex(offset);
        if (m_grid.startCorner == Grid.Corner.MiddleCenter)
            index = Mathf.CeilToInt(index - childList.Count * 0.5f);
        for (int i = 0; i < childList.Count; i++)
        {
            if (index == childList[i].realIndex)
            {

                //childList[i].item.GetComponent<Button>().image.color = Color.green;
                currItem = childList[i];
                if (currItem != oldItem)
                {
                    timer = 0;
                    oldItem = currItem;
                }
                break;

            }
            else
            {
                //childList[i].item.GetComponent<Button>().image.color = Color.white;

            }
        }
        if(currItem == null && childList.Count > 0)
        {
            currItem = childList[0];
        }

        if (temp_current != currItem && CenterChange != null)
        {
            RectTransform cur = null;
            RectTransform old = null;
            if (currItem != null)
            {
                cur = currItem.item;
            }
            if(temp_current != null)
            {
                old = temp_current.item;
            }
            
            CenterChange(cur, old);
        }

    }
    childConfig currItem;
    Vector2 target;
    Vector2 myVelocity;
    float timer;
    bool canMove;
    float speed;
    int dis;
    void Update()
    {
        if (canMove)
        {
            
            if (!elasticity || elasticityType == ElasticityType.OneWayElasticity)
                OneWay();
            else
                TwoWay();
            WrapHandler();

        }
    }
    void OneWay()
    {
        float deltaTime = Time.unscaledDeltaTime;
        Vector2 position = m_rectTransform.anchoredPosition;
        position[dis] = Mathf.SmoothDamp(m_rectTransform.anchoredPosition[dis], target[dis], ref speed, timer, Mathf.Infinity, deltaTime);
        m_rectTransform.anchoredPosition = position;

        if (Vector2.Distance(m_rectTransform.anchoredPosition, target) < 0.1f)
            canMove = false;
    }
    void TwoWay()
    {
        Vector2 position = m_rectTransform.anchoredPosition;
        float deltaTime = Time.unscaledDeltaTime;
        myVelocity[dis] *= Mathf.Pow(0.135f, deltaTime);
        if (Mathf.Abs(myVelocity[dis]) < 1)
            myVelocity[dis] = 0;
        position[dis] += myVelocity[dis] * deltaTime;
        m_rectTransform.anchoredPosition = position;
        Vector3 newVelocity = (target - m_rectTransform.anchoredPosition) / deltaTime;
        myVelocity = Vector3.Lerp(myVelocity, newVelocity, deltaTime * 10);
        if (Vector2.Distance(m_rectTransform.anchoredPosition, target) < 0.1f)
            canMove = false;
    }
    Vector2 GetItemPos(Vector2 item)
    {
        Vector2 pos = Vector2.zero;
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
                pos.x = item.x + m_grid.size.x * 0.5f - m_grid.interval.x * 0.5f;
                break;
            case Grid.Corner.UpperRight:
            case Grid.Corner.LowerRight:
            case Grid.Corner.UpperRightCenter:
                pos.x = item.x - m_grid.size.x * 0.5f + m_grid.interval.x * 0.5f;
                break;
            case Grid.Corner.MiddleCenter:
                pos = item;
                break;


        }
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.UpperRight:
            case Grid.Corner.UpperRightCenter:
                pos.y = item.y - m_grid.size.y * 0.5f + m_grid.interval.y * 0.5f;
                break;
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerRight:
            case Grid.Corner.LowerLeftCenter:
                pos.y = item.y + m_grid.size.y * 0.5f - m_grid.interval.y * 0.5f;
                break;

        }
        return pos;
    }
    void WrapHandler()
    {

        if(needloop_)
        {
            for (int i = 0; i < childList.Count; i++)
            {
                Vector2 offset;
                int index;
                if (isFirst() && isFirstOut(out offset, out index))
                {
                    m_frist.item.anchoredPosition = offset;
                    UpdateItem(m_frist);
                    childList.Add(m_frist);
                    childList.RemoveAt(0);
                }
                if (isLast() && isLastOut(out offset))
                {
                    m_last.item.anchoredPosition = offset;
                    UpdateItem(m_last);
                    childList.Insert(0, m_last);
                    childList.RemoveAt(childList.Count - 1);
                }
            }

        }
    }
    void UpdateItem(childConfig config)
    {
        //if (m_grid == null) return;
        int realIndex = (m_grid.startAxis == Grid.Axis.Horizontal) ?
            Mathf.CeilToInt(config.item.anchoredPosition.x / m_grid.size.x) :
                      Mathf.CeilToInt(config.item.anchoredPosition.y / m_grid.size.y);
        if (m_grid.startAxis == Grid.Axis.Vertical)
        {
            switch (m_grid.startCorner)
            {
                case Grid.Corner.UpperLeft:
                case Grid.Corner.UpperRight:
                case Grid.Corner.UpperRightCenter:
                case Grid.Corner.MiddleCenter:
                    realIndex = -realIndex;
                    break;
            }
        }
        config.realIndex = realIndex;
        if (OnValueChange != null)
            OnValueChange(config.item, config.index, realIndex);
    }
    bool isFirst()
    {
        bool flag = false;
        if (m_grid.startAxis == Grid.Axis.Horizontal)
        {
            if (m_scrollRect.velocity.x < 0)
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.MiddleCenter:
                    case Grid.Corner.LowerLeft:
                    case Grid.Corner.UpperLeft:
                    case Grid.Corner.LowerLeftCenter:
                        flag = true;
                        break;
                }
            }
            else
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.UpperRight:
                    case Grid.Corner.UpperRightCenter:
                    case Grid.Corner.LowerRight:
                        flag = true;
                        break;
                }
            }
        }
        else
        {
            if (m_scrollRect.velocity.y > 0)
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.MiddleCenter:
                    case Grid.Corner.UpperLeft:
                    case Grid.Corner.UpperRight:
                    case Grid.Corner.UpperRightCenter:
                        flag = true;
                        break;
                }
            }
            else
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.LowerLeft:
                    case Grid.Corner.LowerRight:
                    case Grid.Corner.LowerLeftCenter:
                        flag = true;
                        break;
                }
            }
        }

        return flag;
    }
    bool isLast()
    {
        bool flag = false;
        if (m_grid.startAxis == Grid.Axis.Horizontal)
        {
            if (m_scrollRect.velocity.x > 0)
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.MiddleCenter:
                    case Grid.Corner.LowerLeft:
                    case Grid.Corner.UpperLeft:
                    case Grid.Corner.LowerLeftCenter:
                        flag = true;
                        break;
                }
            }
            else
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.UpperRight:
                    case Grid.Corner.UpperRightCenter:
                    case Grid.Corner.LowerRight:
                        flag = true;
                        break;
                }
            }
        }
        else
        {
            if (m_scrollRect.velocity.y < 0)
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.MiddleCenter:
                    case Grid.Corner.UpperLeft:
                    case Grid.Corner.UpperRight:
                    case Grid.Corner.UpperRightCenter:
                        flag = true;
                        break;
                }
            }
            else
            {
                switch (m_grid.startCorner)
                {
                    case Grid.Corner.LowerLeft:
                    case Grid.Corner.LowerRight:
                    case Grid.Corner.LowerLeftCenter:
                        flag = true;
                        break;
                }
            }
        }

        return flag;
    }
    bool isFirstOut(out Vector2 offset, out int index)
    {
        Vector3 first = GetChildFirstOffset();

        Vector2 firstCondition = GetFirstCondition();
        bool isOut = false;
        offset = m_frist.item.anchoredPosition;
        index = 0;
        if (m_grid.startAxis == Grid.Axis.Horizontal)
        {
            if (firstCondition.x > 0 && first.x > m_grid.size.x*2)
            {
                offset.x = m_last.item.anchoredPosition.x - m_grid.size.x;
                isOut = true;
                index = -1;
            }
            else if (firstCondition.x < 0 && first.x < -m_grid.size.x*2)
            {
                offset.x = m_last.item.anchoredPosition.x + m_grid.size.x;
                isOut = true;
                index = 1;
            }
        }
        else if (m_grid.startAxis == Grid.Axis.Vertical)
        {
            if (firstCondition.y > 0 && first.y > m_grid.size.y*2)
            {
                offset.y = m_last.item.anchoredPosition.y - m_grid.size.y;
                isOut = true;
                index = -1;
            }
            else if (firstCondition.y < 0 && first.y < -m_grid.size.y*2)
            {
                offset.y = m_last.item.anchoredPosition.y + m_grid.size.y;
                isOut = true;
                index = 1;
            }
        }
        return isOut;
    }
    bool isLastOut(out Vector2 offset)
    {
        Vector3 last = GetChildLastOffset();
        Vector2 lastCondition = GetLastCondition();
        bool isOut = false;
        offset = m_last.item.anchoredPosition;
        if (m_grid.startAxis == Grid.Axis.Horizontal)
        {

            if (lastCondition.x > 0 && last.x > m_grid.size.x*2)
            {
                offset.x = m_frist.item.anchoredPosition.x - m_grid.size.x;
                isOut = true;
            }
            else if (lastCondition.x < 0 && last.x < -m_grid.size.x*2)
            {
                offset.x = m_frist.item.anchoredPosition.x + m_grid.size.x;
                isOut = true;
            }

        }
        else if (m_grid.startAxis == Grid.Axis.Vertical)
        {
            if (lastCondition.y > 0 && last.y > m_grid.size.y*2)
            {
                isOut = true;
                offset.y = m_frist.item.anchoredPosition.y - m_grid.size.y;
            }

            else if (lastCondition.y < 0 && last.y < -m_grid.size.y*2)
            {
                offset.y = m_frist.item.anchoredPosition.y + m_grid.size.y;
                isOut = true;

            }
        }

        return isOut;
    }
    Vector2 GetFirstCondition()
    {
        Vector2 dir = Vector2.one;
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.MiddleCenter:
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
                dir.x = -1;
                break;

        }
        switch (m_grid.startCorner)
        {

            //case Grid.Corner.MiddleCenter:
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
            case Grid.Corner.LowerRight:
                dir.y = -1;
                break;

        }
        return dir;
    }
    Vector2 GetLastCondition()
    {
        Vector2 dir = Vector2.one;
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperRight:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.LowerRight:
                dir.x = -1;
                break;
        }
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.UpperRight:
            case Grid.Corner.MiddleCenter:
                dir.y = -1;
                break;
        }
        return dir;
    }
    Vector2 GetChildFirstOffset()
    {
        Vector2 offset = m_ViewRect.InverseTransformPoint(m_frist.item.position);
        Vector2 viewSize = m_ViewRect.rect.size;
        Vector2 pivot = m_ViewRect.pivot;
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.UpperRight:
                offset.y -= viewSize.y * (1 - pivot.y);
                break;
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
            case Grid.Corner.LowerRight:
                offset.y += viewSize.y * pivot.y;
                break;
            case Grid.Corner.MiddleCenter:
                offset.y -= viewSize.y * pivot.y - m_grid.size.y * m_grid.rect.pivot.y;
                offset.x += viewSize.x * pivot.x - m_grid.size.x * m_grid.rect.pivot.x;
                break;
        }
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
                offset.x += viewSize.x * pivot.x;
                break;
            case Grid.Corner.UpperRight:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.LowerRight:
                offset.x -= viewSize.x * (1 - pivot.x);
                break;
        }

        return offset;
    }
    Vector2 GetChildLastOffset()
    {
        Vector2 offset = m_ViewRect.InverseTransformPoint(m_last.item.position);
        Vector2 viewSize = m_ViewRect.rect.size;
        Vector2 pivot = m_ViewRect.pivot;
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.UpperRight:
                offset.y += viewSize.y * pivot.y - m_grid.size.y;
                break;
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
            case Grid.Corner.LowerRight:
                offset.y -= viewSize.y * (1 - pivot.y) - m_grid.size.y;
                break;
            case Grid.Corner.MiddleCenter:
                offset.y += viewSize.y * (1 - pivot.y) - m_grid.size.y * m_grid.rect.pivot.y;
                offset.x -= viewSize.x * (1 - pivot.x) - m_grid.size.x * m_grid.rect.pivot.x;
                break;
        }
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
                offset.x -= viewSize.x * (1 - pivot.x) - m_grid.size.x;

                break;
            case Grid.Corner.UpperRight:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.LowerRight:
                offset.x += viewSize.x * pivot.x - m_grid.size.x;
                break;
        }
        return offset;

    }
    int GetCenterIndex(Vector2 offset)
    {
        int index = 0;
        if (m_grid.startAxis == Grid.Axis.Horizontal)
            index = Mathf.CeilToInt(offset.x);
        else if (m_grid.startAxis == Grid.Axis.Vertical)
            index = Mathf.CeilToInt(offset.y);
        return childList.Count - index;
    }
    public Vector2 GetOffset()
    {
        Vector2 offset = Vector2.zero;
        Vector2 dirOffset = GetCenterOffset();
        Vector2 si = new Vector2(0, childList.Count * (m_grid.size.y));
        switch (m_grid.startCorner)
        {

            case Grid.Corner.MiddleCenter:
                if (elasticity && elasticityType == ElasticityType.OneWayElasticity)
                {
                    offset.x = (maxSize.x * m_rectTransform.pivot.x + m_rectTransform.anchoredPosition.x+ AnchorOffset().x) / m_grid.size.x;
                    offset.y = (maxSize.y * m_rectTransform.pivot.y - m_rectTransform.anchoredPosition.y- AnchorOffset().y) / m_grid.size.y;
                }
                else
                {
                    offset.x = ((maxSize.x + dirOffset.x * 4f) * m_rectTransform.pivot.x + m_rectTransform.anchoredPosition.x+ AnchorOffset().x) / m_grid.size.x;
                    offset.y = ((maxSize.y - dirOffset.y * 4f) * m_rectTransform.pivot.y - m_rectTransform.anchoredPosition.y- AnchorOffset().y) / m_grid.size.y;
                }

                break;
            case Grid.Corner.UpperLeft:
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.UpperRight:
                offset.y = elasticity && elasticityType == ElasticityType.OneWayElasticity ?
                    (maxSize.y * m_rectTransform.pivot.y - m_rectTransform.anchoredPosition.y - AnchorOffset().y) / m_grid.size.y :
                    ((maxSize.y - dirOffset.y * 2) * m_rectTransform.pivot.y - m_rectTransform.anchoredPosition.y- AnchorOffset().y) / m_grid.size.y;
                break;
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
            case Grid.Corner.LowerRight:
                offset.y = elasticity && elasticityType == ElasticityType.OneWayElasticity ?
                    (maxSize.y + m_rectTransform.anchoredPosition.y+ AnchorOffset().y) / m_grid.size.y :
                    ((maxSize.y + dirOffset.y * 2) + m_rectTransform.anchoredPosition.y+ AnchorOffset().y) / m_grid.size.y;
                break;

        }
        switch (m_grid.startCorner)
        {
            case Grid.Corner.UpperLeft:
            case Grid.Corner.LowerLeft:
            case Grid.Corner.LowerLeftCenter:
                offset.x = elasticity && elasticityType == ElasticityType.OneWayElasticity ?
                    (maxSize.x + m_rectTransform.anchoredPosition.x+AnchorOffset().x) / m_grid.size.x :
                    (maxSize.x + dirOffset.x * 2f + m_rectTransform.anchoredPosition.x+AnchorOffset().x) / m_grid.size.x;
                break;
            case Grid.Corner.UpperRightCenter:
            case Grid.Corner.UpperRight:
            case Grid.Corner.LowerRight:
                offset.x = elasticity && elasticityType == ElasticityType.OneWayElasticity ?
                    (maxSize.x + m_rectTransform.anchoredPosition.x - m_grid.size.x+ AnchorOffset().x) / m_grid.size.x :
                    (maxSize.x + dirOffset.x * 2 + m_rectTransform.anchoredPosition.x - m_grid.size.x+ AnchorOffset().x) / m_grid.size.x;
                break;
        }
        return offset;
    }
    public Vector2 GetCenterOffset()
    {
        Vector2 offset = Vector2.zero;
        for (int i = 0; i < 2; i++)
        {
            float dir = 1;
            if (m_scrollRect.velocity[i] > 0f || m_scrollRect.velocity[i] < 0f)
                dir = m_scrollRect.velocity[i] > 0 ? 1 : -1;
            else if (oldDelta[i] > 0f || oldDelta[i] < 0f)
                dir = oldDelta[i] > 0 ? 1 : -1;
            offset[i] = dir * (m_grid.size[i] * 0.5f);
        }
        return offset;
    }
    /// <summary>
    /// 所有子物体列表
    /// </summary>
    List<childConfig> childList = new List<childConfig>();
    /// <summary>
    /// 遍历子物体
    /// </summary>
    public void SetDirty()
    {
        childList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = (RectTransform)transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                childConfig config = new childConfig(child, i, i);
                childList.Add(config);
                UpdateItem(config);
            }
        }
    }

    public Vector2 GetGridSize()
    {
        return m_grid.size;
    }


    public Vector2 GetInterval()
    {
        return m_grid.interval;
    }






}