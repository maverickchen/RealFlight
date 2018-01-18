using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {
	public float speed;            // The speed that the player will move at.
	public float gravity;
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	public GameRunner gameManager;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	void applyGravity() {
		movement.Set(0f, -1f, 0f);
		movement = movement.normalized * gravity;
		playerRigidbody.AddForce (movement, ForceMode.Impulse);
	}

	void jump() {
		movement.Set(0f, 1f, 0f);
		movement = movement.normalized * speed;
		playerRigidbody.AddForce (movement, ForceMode.Impulse);
	}

	// Player hit something; stop the game. 
	void OnCollisionEnter(){
		playerRigidbody.velocity = Vector3.zero;
		playerRigidbody.angularVelocity = Vector3.zero;
		gameManager.stopped = true;
	}

	// Listen for button presses and apply gravity.
	void Update () {
		if (gameManager.stopped) {
			return;
		}
		applyGravity();
		#if UNITY_EDITOR || UNITY_EDITOR_64
		float v = Input.GetAxisRaw ("Vertical");
		if (v > 0) {
			jump();
		}
		#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];
			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began)
			{
				// If so, add jump force
				jump();
			}
		}
		#endif 

	}
}
