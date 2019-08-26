using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

/// <summary>
/// User interface game end.
/// </summary>

public class UIBattleEnd : UINode
{
	Text txtBattleResult ;
	Button btnExitBattle ;

	public override void Enter ()
	{
		base.Enter ();

		txtBattleResult = uicomponent.GetText ("TextBattleResult");
		btnExitBattle = uicomponent.GetButton ("ButtonExitBattle");

		txtBattleResult.text = AloneDataManager<GameInfo>.Instance.Data.mBattleStatus.ToString ();
		btnExitBattle.onClick.AddListener (this.OnExitBattleClick);
	}

	void OnExitBattleClick()
	{
		AloneEventCenter<ExitBattleEvent>.Instance.OnEvent(new ExitBattleEvent(){mExitReason = "User Click the Exit Battle button . "}) ;
	}

}
