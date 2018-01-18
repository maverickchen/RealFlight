// When the object collides with the threshold attached to a wall, increments the score


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class addToScore : MonoBehaviour {

	public GameObject scoreView;
	TextMeshPro score;
	// Use this for initialization
	void Start () {
		score = scoreView.GetComponent<TextMeshPro>();
	}

	void OnTriggerEnter(Collider other) {
		if (score == null) {
			score = scoreView.GetComponent<TextMeshPro>();
		}
		int newScore = int.Parse (score.text) + 1;
		score.text = newScore.ToString();
		print (score.text);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}