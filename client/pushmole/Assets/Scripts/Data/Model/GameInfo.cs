using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Info . 
/// </summary>

[System.Serializable]
public class GameInfo
{
	//	游戏时间限制120s。
	public const int GameTimeLimit = 120;

	public const int PathFindingLimit = 500;

	public const int MapRow = 20;

	public const int MapColumn = 20;

	public const float MapTileLength = 1;

	public static Vector3 CameraOffset = new Vector3 (9,11,-8.5f);

	public float mCurrentTimeLeft = GameTimeLimit ;

	public EBattleStatus mBattleStatus = EBattleStatus.Playing ;

	public void Reset()
	{
		mCurrentTimeLeft = GameTimeLimit;
		mBattleStatus = EBattleStatus.Playing;
	}

}


public enum EBattleStatus
{
	Playing,
	Win,
	Lose,
	Exit,
}
