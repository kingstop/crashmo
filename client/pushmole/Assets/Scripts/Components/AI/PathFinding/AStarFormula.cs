using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A star formula.公式
/// </summary>

public class AStarFormula 
{
	const int COST = 10;
	const int COST2 = 14;
	const int ERROR_COST = int.MaxValue;

	public static int HCost(Tile start , Tile end)
	{
		int step = Mathf.Abs (start.mRow-end.mRow)+Mathf.Abs(start.mColumn-end.mColumn);
		return step * COST;
	}

	public static int GCost(Tile start , Tile neighbor)
	{
		if (!IsNeighbor (start, neighbor))
		{
			Debug.LogError ("Error neighbor !");
			return ERROR_COST;
		}

		if (start.mRow == neighbor.mRow || start.mColumn == neighbor.mColumn)
			return COST+start.mGcost;
		else
			return COST2+start.mGcost;
	}

	public static bool IsNeighbor(Tile start , Tile target)
	{
		if (Mathf.Abs (start.mRow - target.mRow) > 1 || Mathf.Abs (start.mColumn - target.mColumn) > 1)
			return false;
		else
			return true;
	}


}
