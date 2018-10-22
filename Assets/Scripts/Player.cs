using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	//Instantiate animator and controller
	private Animator anim;
	private CharacterController controller;
	private CapsuleCollider collider;

	//Instantiate movement variables
	private float runSpeed = 17.0f;
	private float backSpeed = 10.0f;
	private float airSpeed = 15.0f;
	private float turnSpeed = 90.0f;
	private float jumpSpeed = 24.0f;
	private float gravity;
	private float moveDirY;
	private Vector3 moveDirection = Vector3.zero;
	private int airDirChangeCount;

	//Instantiate grounded/jump checks
	public bool grounded = true;
	private bool hasJumped = false;

	//Create variables for the cat
	private bool catIsPicked;
	private bool catIsHome;

	//Create variables for UI
	public Text questGoal;
	public Text winText;

	void Start () {
		//Instantiate variables
		controller = GetComponent<CharacterController> ();
		collider = GetComponent<CapsuleCollider> ();
		anim = gameObject.GetComponentInChildren<Animator>();
		catIsPicked = false;
		catIsHome = false;
		hasJumped = false;
		grounded = true;
		gravity = 30.0f;
		moveDirection = Vector3.zero;
		moveDirY = 0.0f;
		airDirChangeCount = 0;

		//Set the quest goal text
		questGoal.text = "Save the cat from the tree";
		winText.text = " ";
	}

	void FixedUpdate () {
		//Set grounded
		grounded = controller.isGrounded;

		//Check if the character has landed from a jump
		if (grounded == true) {
			hasJumped = false;
		}

		//Set animation parameter
		if (grounded == true) {
			//Set run
			if (Input.GetKey ("w") || Input.GetKey ("up")) {
				anim.SetInteger ("AnimParam", 1);
				runSpeed = 17.0f;
			}
			//Set walk backwards
			else if (Input.GetKey ("s") || Input.GetKey ("down")) {
				anim.SetInteger ("AnimParam", 2);
				runSpeed = backSpeed;
			}
			//Set jump
			else if (Input.GetKey ("space")) {
				anim.SetInteger ("AnimParam", 3);
			}
			//Set idle
			else {
				anim.SetInteger ("AnimParam", 0);
			}
		}
		//Set jump animation if in the air
		else if (hasJumped == true) {
			anim.SetInteger ("AnimParam", 3);
		}
		else {
			anim.SetInteger ("AnimParam", 0);
		}

		//Set movement variables
		if (grounded == true) {
			moveDirection = transform.forward * Input.GetAxis ("Vertical") * runSpeed;
			moveDirY = 0.0f;
			airDirChangeCount = 0;
		} 
		else if (grounded == false && Input.GetKey("w") && airDirChangeCount <= 8) {
			jumpDirChange ();
		}
		else if (grounded == false && Input.GetKey("up") && airDirChangeCount <= 8) {
			jumpDirChange ();
		}

		//Jump if grounded and the spacebar is pressed
		if (Input.GetKey ("space") && grounded == true) {
			moveDirection.y = jumpSpeed;
			hasJumped = true;
		}

		//Move the character
		float turn = Input.GetAxis ("Horizontal");
		transform.Rotate (0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);

		//Apply gravity
		if (hasJumped == false) {
			moveDirection.y -= 40.0f * Time.deltaTime;
		} 
		else {
			moveDirection.y -= gravity * Time.deltaTime;
		}
		moveDirY = moveDirection.y;
	} //End of FixedUpdate

	//Character changes direction mid-air
	private void jumpDirChange() {
		//Allow the character to change direction in the air
		moveDirection = transform.forward * Input.GetAxis ("Vertical") * airSpeed;
		moveDirection.y = moveDirY;

		//Increment to restrict the movement in the air
		airDirChangeCount++;
	}

	//Check if the cat is picked up and the character returned home
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Kitty")) {
			other.gameObject.SetActive(false);
			catIsPicked = true;
			questGoal.text = "Return home";
		} 
		else if (other.gameObject.CompareTag ("House") && catIsPicked == true) {
			catIsHome = true;
			winText.text = "Quest Complete!";
		}
	} //End of OnTriggerEnter
} //End of class
