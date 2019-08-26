using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coord
{


    // 		世界坐标转UGUI坐标
    public static Vector2 WorldToUGUI(Vector3 worldPos, Canvas canvas)
    {
        Vector2 ugui = Vector2.zero;
        Vector3 screen = Camera.main.WorldToScreenPoint(worldPos);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screen, canvas.worldCamera, out ugui))
        {
        }
        return ugui;
    }


    //		屏幕坐标转世界坐标。
    private static Vector3 ScreenToWorld(Vector2 screen, Camera camera /*= Camera.main*/)
    {
        bool success = false;
        Vector3 world = Vector3.zero;
        Ray ray = camera.ScreenPointToRay(screen);

        //  平面
        GameObject terrain = GameObject.Find("Terrain");

        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.gameObject == terrain)
            {
                success = true;
                world = hit.point;
                break;
            }
        }

        if (!success)
        {
            Debug.LogError("Need a Base Plane !! ");
        }

        return world;
    }

}
