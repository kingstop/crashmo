using UnityEngine;
using System.Collections;

/// <summary>
/// 协程的实质就是一个迭代器：
/// 其中的yield retun就是等待下一帧
/// <http://www.zhihu.com/question/23895384>
/// <http://blog.csdn.net/fredomyan/article/details/44132719>
/// </summary>

public class CoroutineTest : MonoBehaviour
{
    int MAX = 10;
    int current = 0;

    void Start()
    {
        //CoroutineCenter.Instance.StartCoroutine(this.Coroutine());
        //CoroutineCenter.Instance.StopCoroutine(this.Coroutine());
    }

    IEnumerator Coroutine()
    {
        for (current = 0; current < MAX; current++)
        {
            Debug.Log(Time.time + this.GetType().Name);
            yield return null;
        }
    }

    void Update()
    {
        if (current <= MAX)
        {
            //Debug.Log("[" + Time.time + "]");
        }
    }
}
