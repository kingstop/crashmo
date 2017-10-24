using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UGUI_ListManager : UGUI_DynamicScrollBase
{

    /// <summary>
    /// 使用方法：
    /// 1.继承NGUIDynamicScrollBase类
    /// 2.实现ResetItemData方法
    /// 3.调用BaseOnEnable方法
    /// 4.在update中添加父类的BaseUpdate方法
    /// 5.初始化数据
    /// 6.设置当前需要显示的数据长度
    /// </summary>
   

    void Awake()
    {
     //   prefab = Resources.Load("Item") as GameObject;
        
    }

    void OnEnable()
    {
        BaseOnEnable();

    }

	//重新刷新数据
    public void RefreshData(int _count)
    {
        base.DataListCount = _count;
    }

	
	// Update is called once per frame
	void Update () {
        base.BaseUpdate();
	}

    /// <summary>
    /// //初始化显示数据
    /// </summary>
    protected override void ResetItemData(GameObject go, int index)
    {
        //得到绑定在每个列表项中的脚本
        //  go.GetComponent<UiQuestItem>().InitData(quest_items[index], index);

		UILoopItem item = go.GetComponent<UILoopItem> ();
		item.Data(index);
		
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 50, 50),"Switch"))
        {
            //这是最大长度
            RefreshData(20);
        }
    }

}
