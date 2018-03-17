using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour {
	private Character[] potentialTarget;
	private Character currentTarget;
	private int contactDist = 5;
	private int detectionDist = 10000;
	private int movementSpeed = 1;

	// Use this for initialization
	void Start () {
		potentialTarget = GameObject.FindObjectsOfType<Character>();
		if (potentialTarget != null && potentialTarget.Length > 0) {
			int luckyBastard = Random.Range (0, potentialTarget.Length);
			currentTarget = potentialTarget [luckyBastard+1];
		}	

	}
	
	// Update is called once per frame
	void Update () {
		if (currentTarget != null) {
			gameObject.transform.LookAt (currentTarget.transform);
			if (Vector2.Distance (transform.position, currentTarget.transform.position) >= contactDist) {
				// Init mouvement guide line
				Vector2 axe = currentTarget.transform.position - gameObject.transform.position;
				axe.Normalize ();
				gameObject.transform.Translate (axe * movementSpeed * Time.deltaTime);

				if (Vector2.Distance (transform.position, currentTarget.transform.position) <= detectionDist) {
					// Attack
				}
			}
		}
	}

}
