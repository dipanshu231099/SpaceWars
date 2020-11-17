using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public Transform target; // the object to rotate around
	public int speed; // the speed of rotation
	private GameObject pauseObject;
	
	void Start() 
	{
		pauseObject = GameObject.Find("PauseStatus");
		if (target == null) 
		{
			target = this.gameObject.transform;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(pauseObject.GetComponent<PauseHandler>().gamePaused)
        {
			return;
        }
		transform.RotateAround(target.transform.position,target.transform.up,speed * Time.deltaTime);
	}
}
