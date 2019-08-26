using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should be added to a gameobject  . 
/// </summary>

public class Main : MonoBehaviour
{
	Game mGame;
    float mGameSpeed;

    void Awake()
    {
		DontDestroyOnLoad(gameObject);

        mGameSpeed = 1;
		mGame = Game.Instance;
		mGame.Init();

		#if UNITY_EDITOR
		new TestSystem() ;
		#endif
    }

    void Start()
    {
        mGame.Enter();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < mGameSpeed; i++)
        {
            mGame.Update(Time.fixedDeltaTime);
        }
    }

    void OnDestroy()
    {
        mGame.Leave();
    }
}
