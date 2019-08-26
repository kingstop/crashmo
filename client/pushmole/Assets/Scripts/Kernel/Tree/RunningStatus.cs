using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运行状态 . 
/// </summary>

public enum RunningStatus
{
    Inactive,
    Ready,              //  准备好运行
    Running,
    Failure,
    Success,
}
