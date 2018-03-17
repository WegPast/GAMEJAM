using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour {
	private Character[] potentialTarget;
	private Character currentTarget;

	// Use this for initialization
	void Start () {
		potentialTarget = GameObject.FindObjectsOfType<Character>();
		int luckyBastard = Random.Range (0, potentialTarget.Length);
		currentTarget = potentialTarget [luckyBastard];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Brain () {
		
	}

	public void Move(Vector2 wishedMove) {
		gameObject.transform.position = wishedMove;
	}
}
