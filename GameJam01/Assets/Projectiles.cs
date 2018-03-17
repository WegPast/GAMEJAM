using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour {

    private Rigidbody2D body;
    public float speed = 10f;



	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity  = new Vector3(0, speed, 0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
