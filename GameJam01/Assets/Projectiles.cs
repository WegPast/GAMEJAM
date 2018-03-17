using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour {

    private Rigidbody2D body;
    public float speed = 300f;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity = (transform.parent.rotation * Vector2.up) * speed;
    }
	
}
