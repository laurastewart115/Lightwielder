using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour {

	//Instantiate variables
	public Transform target;
	public float minDistance;
	public float maxDistance;
	public float height;

	private Transform myTransform;

	private float x;
	private float y;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;

	private float wantedRotAngle;
	private float wantedHeight;
	private float currentRotAngle;
	private float currentHeight;
	private Quaternion currentRot;
	//public float heightDamp = 2.0f;
	//public float rotDamp = 3.0f;

	// Use this for initialization
	void Start () {
		//Set a debug log
		if (target == null) {
			Debug.LogWarning ("There's no target, you fool!");
		}

		//Set camera distances
		minDistance = 10.0f;
		maxDistance = 20.0f;
		height = 5.0f;

		myTransform = transform;

		CameraSetup();
	}

	void Update () {
		//if (Input.GetMouseButtonDown (1)) {

		//}
	}

	void FixedUpdate() {

	}

	void LateUpdate() {
		//myTransform.position = new Vector3 (target.position.x, target.position.y + height, target.position.z - minDistance);
		//myTransform.LookAt (target.position);

		if (Input.GetMouseButton (1)) {

			x += Input.GetAxis ("Mouse X") * xSpeed * minDistance * 0.02f;
			y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

			//y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler (y, x, 0);

			minDistance = Mathf.Clamp (minDistance - Input.GetAxis ("Mouse ScrollWheel") * 5, minDistance, maxDistance);

			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) {
				minDistance -= hit.distance;
			}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -minDistance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;
		} 
		else {
			CameraSetup ();
			x = 0;
			y = 0;

			//Calculate the current rotation angles
			wantedRotAngle = target.eulerAngles.y;
			wantedHeight = target.position.y;

			currentRotAngle = myTransform.eulerAngles.y;
			currentHeight = myTransform.position.y;

			//Damp y axis rotation
			currentRotAngle = Mathf.Lerp(currentRotAngle, wantedRotAngle, 0);

			//Damp height rotation
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 0);

			//Convert angle to rotation
			currentRot = Quaternion.Euler(0, currentRotAngle, 0);

			//Set x-z position
			myTransform.position = target.position;
			myTransform.position -= currentRot * Vector3.forward * minDistance;

			//Set cam height
			myTransform.position = new Vector3(myTransform.position.x, currentHeight, myTransform.position.z);

			//Look at target
			myTransform.LookAt(target);
		}
	}

	public void CameraSetup() {
		myTransform.position = new Vector3 (target.position.x, target.position.y + height, target.position.z - minDistance);
		myTransform.LookAt (target.position);
	}
}
