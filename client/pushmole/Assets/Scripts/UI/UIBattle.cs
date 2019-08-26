using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : UINode
{
	Text txtTimeLeft;
	Button btnAttack;

	int mTimeLeft = -1;

	public override void Init ()
	{
		base.Init ();

		this.mTimeLeft = -1;
	}

	public override void Enter ()
	{
		base.Enter ();

		txtTimeLeft = uicomponent.GetText ("TimeLeft");
		btnAttack = uicomponent.GetButton ("ButtonAttack");

		AloneDataManager<GameInfo>.Instance.OnDataUpdate += this.OnDataChange;

		this.UpdateTimeLeft ();
		btnAttack.onClick.AddListener (this.OnButtonAttackClick);
	}

	void OnDataChange (GameInfo data)
	{
		this.UpdateTimeLeft ();
	}

	void UpdateTimeLeft ()
	{
		int time = Mathf.Clamp ((int)(AloneDataManager<GameInfo>.Instance.Data.mCurrentTimeLeft + 1), 0, GameInfo.GameTimeLimit);

		if (mTimeLeft != time)
		{
			this.mTimeLeft = time;
			this.txtTimeLeft.text = string.Format (Lauguage.WarnFormat, this.mTimeLeft);
		}
	}

	public override void Leave ()
	{
		base.Leave ();

		this.mTimeLeft = -1;
		AloneDataManager<GameInfo>.Instance.OnDataUpdate -= this.OnDataChange;
	}

	void OnButtonAttackClick ()
	{
		AloneEventCenter<UserInputEvent>.Instance.OnEvent (new UserInputEvent(UserInputEvent.UserInputCommand.Attack,Vector3.zero,0));
	}
}
