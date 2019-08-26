using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 场景加载只是逻辑中的一个步骤节点，而不是核心，核心是加载完成后的GamePlay。
/// 为了使得核心点紧凑，这里的SceneLoader里不会有游戏战斗初始化的逻辑，而是放在Game里去控制。
/// 重命名原因：
/// 最初设计——该类命名为BaseScene类，在相应的场景加载完毕后，后执行GamePlay.Instance.Enter来进入游戏；；；
/// 后来为了降低该类在逻辑中的重要性，将其改为SceneLoader，仅仅作为场景加载使用。
/// </summary>

public class SceneLoader : Node
{
    public Action OnSceneLoadFinish;

    string mSceneName;

    public SceneLoader(string name)
    {
        this.mSceneName = name;
    }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene(mSceneName);
        SceneManager.sceneLoaded += this.SceneLoaded;
    }

    public override void Leave()
    {
        base.Leave();
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (OnSceneLoadFinish != null)
        {
            OnSceneLoadFinish.Invoke();
        }

        this.RunningStatus = RunningStatus.Success;
    }

}
