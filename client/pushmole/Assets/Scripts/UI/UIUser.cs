
#if UITEST


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.UI;

public class UIUser : UINode
{


    TestData mUserInfo;
    Text mIDText;

	public override void Init()
    {
		mUserInfo = Game.Instance.mDataManager.UserInfo;
        mUserInfo = new TestData();

        mIDText = GameObject.FindObjectOfType<Text>();
    }

	public override void Release()
    {
    }

    void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }


}
#endif
