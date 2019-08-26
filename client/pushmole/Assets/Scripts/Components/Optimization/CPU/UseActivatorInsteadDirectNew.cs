using UnityEngine;
using System.Collections;

public class UseActivatorInsteadDirectNew : MonoBehaviour
{
	private int COUNT = 50000;

	public void Start ()
	{
		//System.Activator.CreateInstance(typeof(Unit));
		//this.Invoke("CreateObject_UseActivator", 5);
		//this.Invoke("CreateObject_DirectNew", 5);
	}

	private void CreateObject_UseActivator ()
	{
		Unit unit = null;

		float start = Time.realtimeSinceStartup;
		float end = Time.realtimeSinceStartup;

		Debug.LogError ("UseActivator：" + Time.realtimeSinceStartup);
		for (int i = 0; i < COUNT; i++)
		{
			unit = System.Activator.CreateInstance (typeof(Unit)) as Unit;
		}

		end = Time.realtimeSinceStartup;

		//Debug.LogError ("UseActivator：" + Time.realtimeSinceStartup + "<1002>" + (end - start));
		Debug.LogError (string.Format("Use Activator : {0} <1002> {1} - {2}",Time.realtimeSinceStartup,end-start,unit.ID));

		this.Invoke ("CreateObject_DirectNew", 5);
	}

	private void CreateObject_UseNew ()
	{
		Unit unit = null;
			
		float start = Time.realtimeSinceStartup;
		float end = Time.realtimeSinceStartup;

		Debug.LogError ("DirectNew：" + Time.realtimeSinceStartup + " 2001");
		for (int i = 0; i < COUNT; i++)
		{
			unit = new Unit ();
		}
		end = Time.realtimeSinceStartup;

		//Debug.LogError ("DirectNew：" + Time.realtimeSinceStartup + " <2002>" + (end - start) + " " + unit.ID);
		Debug.LogError (string.Format("Use Activator : {0} <1002> {1} - {2}",Time.realtimeSinceStartup,end-start,unit.ID));

		//CreateObject_UseActivator();
	}
}
