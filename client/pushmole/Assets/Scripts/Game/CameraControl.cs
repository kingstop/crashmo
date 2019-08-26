using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : LogicNode
{
	Vector3 mTargetPosition ;
	Transform mCameraTrans ;
	Vector3 mCameraOffset ;

	public CameraControl()
	{
		AloneEventCenter<PlayerPositionChangeEvent>.Instance.AddListener (this,this.OnPlayerPositionChange);
	}

	public override void Release ()
	{
		base.Release ();
		AloneEventCenter<PlayerPositionChangeEvent>.Instance.AddListener (this,this.OnPlayerPositionChange);
	}

	public void SetCamera(Camera camera,Vector3 offset)
	{
		this.mCameraTrans = camera.transform;
		this.mCameraOffset = offset;
	}


	public override void Update (float deltaTime)
	{
		base.Update (deltaTime);

		if (this.mCameraTrans != null&&this.mCameraTrans.position!=mTargetPosition)
		{
			this.mCameraTrans.position = mTargetPosition+mCameraOffset;
		}	
	}

	public void OnPlayerPositionChange(PlayerPositionChangeEvent Event)
	{
		this.mTargetPosition = Event.mPosition;
	}

}
