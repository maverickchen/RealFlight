// Controls the pipe's forward movement. 
// Pipes are spawned to have a random height for the player to fly through.
// Pipes move in the -x direction towards the player, and players only move in the y direction, up and down. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMovement : MonoBehaviour {
	public GameObject pipe;
	public GameRunner gameManager;
	public float speed;
	public float loopBackDist;
	public float startDistAway;
	public float distBetweenW;
	public float numWalls;

	// Use this for initialization
	void Start () {
		float height = Random.Range(1f, 10f);
		pipe.transform.position = new Vector3(pipe.transform.position.x, 
											height, 
											pipe.transform.position.z);
	}

	void loopBack () {
		float height = Random.Range(1f, 10f);
		float x = pipe.transform.position.x + (numWalls * distBetweenW);
		pipe.transform.position = new Vector3(x, 
			height, 
			pipe.transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		if (!gameManager.stopped) {
			float xIncrease = Time.deltaTime * speed;
			pipe.transform.Translate (-xIncrease, 0, 0);
			if (pipe.transform.position.x <= loopBackDist) {
				loopBack ();
			}
		}
	}
}
