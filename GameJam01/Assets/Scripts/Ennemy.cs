using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour {

	private CharaMono currentTarget;
	private float contactDist = 0F;
	[Header("Enemy characteristic")]
	public float movementSpeed = 1F;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// if "this" has a taget
		if (currentTarget != null) {
			if (Vector2.Distance (transform.position, currentTarget.transform.position) >= contactDist) {
				// Init mouvement guide line
				Vector2 axe = currentTarget.transform.position - gameObject.transform.position;
				axe.Normalize ();
				gameObject.transform.Translate (axe * movementSpeed * Time.deltaTime);
			}
		} else {
			CharaMono[] potentialTarget = GameObject.FindObjectsOfType<CharaMono>();
			if (potentialTarget != null && potentialTarget.Length > 0) {

				int luckyBastard = Random.Range (1, potentialTarget.Length);
				Debug.Log ("lucky bastard: " + luckyBastard);
				currentTarget = potentialTarget[luckyBastard-1];
			}	
		}
	}

}
