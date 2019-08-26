using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIComponent : MonoBehaviour
{
    public List<Transform> Transforms;
    public List<Text> Texts;
    public List<Image> Images;
    public List<Button> Buttons;
    public List<ScrollRect> ScrollViews;
    public List<GridLayoutGroup> Grids;
    public List<HorizontalLayoutGroup> HorizontalLayout;
    public List<VerticalLayoutGroup> VerticalLayout;
    public List<Slider> Sliders;
    public List<Toggle> Toggles;
    public List<Dropdown> DropDowns;
    public List<InputField> InputFields;
    public List<UIComponent> uicompents;
    private Dictionary<string, Transform> transDic = new Dictionary<string, Transform>();
    private Dictionary<string, Text> textDic = new Dictionary<string, Text>();
    private Dictionary<string, Image> imageDic = new Dictionary<string, Image>();
    private Dictionary<string, Button> buttonDic = new Dictionary<string, Button>();
    private Dictionary<string, ScrollRect> scrollDic = new Dictionary<string, ScrollRect>();
    private Dictionary<string, GridLayoutGroup> gridDic = new Dictionary<string, GridLayoutGroup>();
    private Dictionary<string, HorizontalLayoutGroup> horizontalDic = new Dictionary<string, HorizontalLayoutGroup>();
    private Dictionary<string, VerticalLayoutGroup> verticalDic = new Dictionary<string, VerticalLayoutGroup>();
    private Dictionary<string, Slider> sliderDic = new Dictionary<string, Slider>();
    private Dictionary<string, Toggle> toggleDic = new Dictionary<string, Toggle>();
    private Dictionary<string, Dropdown> dropDic = new Dictionary<string, Dropdown>();
    private Dictionary<string, InputField> inputDic = new Dictionary<string, InputField>();
    private Dictionary<string, UIComponent> uicompentsDic = new Dictionary<string, UIComponent>();

    public void Init()
    {
        if (Transforms != null)
        {
            transDic.Clear();
            foreach (Transform tr in Transforms)
            {
                transDic.Add(tr.name, tr);
            }
        }
        if (Texts != null)
        {
            textDic.Clear();
            foreach (Text t in Texts)
            {
                textDic.Add(t.transform.name, t);
            }
        }
        if (Images != null)
        {
            imageDic.Clear();
            foreach (Image i in Images)
            {
                imageDic.Add(i.transform.name, i);
            }
        }
        if (Buttons != null)
        {
            buttonDic.Clear();
            foreach (Button b in Buttons)
            {
                buttonDic.Add(b.transform.name, b);
            }
        }
        if (ScrollViews != null)
        {
            scrollDic.Clear();
            foreach (ScrollRect s in ScrollViews)
            {
                scrollDic.Add(s.transform.name, s);
            }
        }
        if (Grids != null)
        {
            gridDic.Clear();
            foreach (GridLayoutGroup g in Grids)
            {
                gridDic.Add(g.transform.name, g);
            }
        }
        if (HorizontalLayout != null)
        {
            horizontalDic.Clear();
            foreach (HorizontalLayoutGroup h in HorizontalLayout)
            {
                horizontalDic.Add(h.transform.name, h);
            }
        }
        if (VerticalLayout != null)
        {
            verticalDic.Clear();
            foreach (VerticalLayoutGroup v in VerticalLayout)
            {
                verticalDic.Add(v.transform.name, v);
            }
        }
        if (Sliders != null)
        {
            sliderDic.Clear();
            foreach (Slider sl in Sliders)
            {
                sliderDic.Add(sl.transform.name, sl);
            }
        }
        if (Toggles != null)
        {
            toggleDic.Clear();
            foreach (Toggle tg in Toggles)
            {
                toggleDic.Add(tg.transform.name, tg);
            }
        }
        if (DropDowns != null)
        {
            dropDic.Clear();
            foreach (Dropdown drop in DropDowns)
            {
                dropDic.Add(drop.transform.name, drop);
            }
        }
        if (InputFields != null)
        {
            inputDic.Clear();
            foreach (InputField input in InputFields)
            {
                inputDic.Add(input.transform.name, input);
            }
        }
        if (uicompents != null)
        {
            uicompentsDic.Clear();
            foreach (UIComponent uicompent in uicompents)
            {
                uicompentsDic.Add(uicompent.transform.name, uicompent);
            }
        }
    }

    public Transform GetTransform(string name)
    {
        Transform trans = null;
        if (transDic.ContainsKey(name))
        {
            trans = transDic[name];
        }
        return trans;
    }

    public Text GetText(string name)
    {
        Text text = null;
        if (textDic.ContainsKey(name))
        {
            text = textDic[name];
        }
        return text;
    }

    public Image GetImage(string name)
    {
        Image image = null;
        if (imageDic.ContainsKey(name))
        {
            image = imageDic[name];
        }
        return image;
    }

    public Button GetButton(string name)
    {
        Button button = null;
        if (buttonDic.ContainsKey(name))
        {
            button = buttonDic[name];
        }
        return button;
    }

    public ScrollRect GetScrollView(string name)
    {
        ScrollRect scroll = null;
        if (scrollDic.ContainsKey(name))
        {
            scroll = scrollDic[name];
        }
        return scroll;
    }

    public GridLayoutGroup GetGridLayout(string name)
    {
        GridLayoutGroup grid = null;
        if (gridDic.ContainsKey(name))
        {
            grid = gridDic[name];
        }
        return grid;
    }

    public HorizontalLayoutGroup GetHLayOut(string name)
    {
        HorizontalLayoutGroup hLayout = null;
        if (horizontalDic.ContainsKey(name))
        {
            hLayout = horizontalDic[name];
        }
        return hLayout;
    }

    public VerticalLayoutGroup GetVLayout(string name)
    {
        VerticalLayoutGroup vLayout = null;
        if (verticalDic.ContainsKey(name))
        {
            vLayout = verticalDic[name];
        }
        return vLayout;
    }

    public Slider GetSlider(string name)
    {
        Slider s = null;
        if (sliderDic.ContainsKey(name))
        {
            s = sliderDic[name];
        }
        return s;
    }

    public Toggle GetToggle(string name)
    {
        Toggle t = null;
        if (toggleDic.ContainsKey(name))
        {
            t = toggleDic[name];
        }
        return t;
    }

    public Dropdown GetDrodown(string name)
    {
        Dropdown drop = null;
        if (dropDic.ContainsKey(name))
        {
            drop = dropDic[name];
        }
        return drop;
    }

    public InputField GetInput(string name)
    {
        InputField input = null;
        if (inputDic.ContainsKey(name))
        {
            input = inputDic[name];
        }
        return input;
    }

    public UIComponent GetUICompent(string name)
    {
        UIComponent uicompent = null;
        if (uicompentsDic.ContainsKey(name))
        {
            uicompent = uicompentsDic[name];
        }
        return uicompent;
    }

}
