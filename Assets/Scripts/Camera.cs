using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
	//Create the target
	public Transform target;

	//Set vars for offset
	private Vector3 offsetPosition = new Vector3(0, 6, -15);
	private Space offsetPositionSpace = Space.Self;

	public bool lookAt = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Refresh ();
	}

	public void Refresh() {
		//Check for a target
		if (target == null) {
			Debug.LogWarning ("There's no camera target, you fool!");

			return;
		}

		//Compute position
		if (offsetPositionSpace == Space.Self) {
			transform.position = target.TransformPoint(offsetPosition);
		} 
		else {
			transform.position = target.position + offsetPosition;
		}

		//Compute rotation
		if (lookAt) {
			transform.LookAt(target);
		} 
		else {
			transform.rotation = target.rotation;
		}

	}
}
