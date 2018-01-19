using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GoogleVR.Demos;

public class GameRunner : MonoBehaviour {
	
	public int numWalls;
	public float distBetweenW;
	public float startDistAway;
	public GameObject wall;
	public GameObject player;
	public GameObject startScreen;
	public GameObject motorButton;
	public GameObject wing1Button;
	public bool stopped;
	public GameObject score;
	public GameObject inputManager;
	List<GameObject> walls;

	public void clearWalls() {
		// restart player's position
		player.transform.position = new Vector3 (0f,5f,0f);

		// destroy any old walls
		foreach (GameObject wall in walls) {
			Destroy (wall);
		}
		walls.Clear ();
	}

	public void roundStarted() {
		print ("Restarted");
		// set score to 0
		score.GetComponent<TextMeshPro>().text = "0";

		// activate the controller pointer
		inputManager.GetComponent<RFInputManager>().setGamePointer();

		// generate new walls
		GameObject obj;
		float x;
		// Instantiate walls for player to jump through
		for (int i = 0; i < numWalls; i++) {
			x = (i+1)*distBetweenW + startDistAway;
			obj = (GameObject) Instantiate(wall,new Vector3(x, 0, 0), Quaternion.identity);
			obj.GetComponentInChildren<addToScore> ().scoreView = score;
			obj.GetComponent<PipeMovement> ().gameManager = this;
			obj.GetComponent<PipeMovement> ().startDistAway = startDistAway;
			obj.GetComponent<PipeMovement> ().distBetweenW = distBetweenW;
			obj.GetComponent<PipeMovement> ().numWalls = numWalls;
			obj.GetComponent<PipeMovement> ().loopBackDist = -1f*((float)numWalls)*0.5f*distBetweenW;
			walls.Add (obj);
		}
		stopped = false;
	}

	// Use this for initialization
	void Start () {
		stopped = true;
		walls = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (stopped) {
			// Activate the Gaze pointer 
			inputManager.GetComponent<RFInputManager>().setMenuPointer();
		}
		if (walls.Count > 0 && stopped) {
			clearWalls ();
			startScreen.SetActive (true);
			motorButton.SetActive (true);
			wing1Button.SetActive (true);
//			roundStarted ();
//		} else {
//		
		}
	}
}
