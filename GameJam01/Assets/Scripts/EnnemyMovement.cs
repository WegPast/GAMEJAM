using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour {
	private Character[] potentialTarget;
	private Character currentTarget;
	private int contactDist = 5;
	private int detectionDist = 10000;

	// Use this for initialization
	void Start () {
		potentialTarget = GameObject.FindObjectsOfType<Character>();
		int luckyBastard = Random.Range (0, potentialTarget.Length);
		currentTarget = potentialTarget [luckyBastard+1];
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (currentTarget);
		if (Vector2.Distance (transform.position, currentTarget.transform.position) >= contactDist) {
			transform.position += transform.forward * Time.deltaTime;
			if(Vector2.Distance(transform.position, currentTarget.transform.position) <= detectionDist) {
				// Attack
			}
		}
	}

	public void Move(Vector2 wishedMove) {
		gameObject.transform.position = wishedMove;
	}
}
