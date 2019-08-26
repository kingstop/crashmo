using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility.
/// </summary>

public static class Utility
{
    //  Set Visible . 
    public static void SetVisible(this GameObject go, bool visible)
    {
        if (go.activeInHierarchy != visible)
            go.SetActive(visible);
	}


	public static Vector2 RotateMatrix(Vector2 v2,float angle)
	{
		float x = v2.x;
		float y = v2.y;

		float rad = angle * Mathf.Deg2Rad;
		float sin = Mathf.Sin (rad);
		float cos = Mathf.Cos (rad);

		float newx = x * cos + y * sin;
		float newy = x * (-sin) + y * cos;
		return new Vector2 (newx,newy);
	}

}
