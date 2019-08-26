using UnityEngine;
using System.Collections;

/// <summary>
/// 更新Unity版本
/// </summary>

public class iOS10
{
    //Unable to insert COPY_SEND
    //Unable to deallocate send right

    //https://forums.developer.apple.com/thread/51597
    //http://stackoverflow.com/questions/39676762/error-bsmacherror-port-1607-os-kern-invalid-capability-0x14-unable-to-i

    //I also experienced the similar crash for one of my app. 
    //It turned out to be that I need to set AppDelegate's Window property in didFinishLaunchingWithOptions. I don't know if this is an iOS 10 SDK bug or something new.    
}
