﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectiles : NetworkBehaviour
{

    private Rigidbody2D body;

    public bool isFromMyPlayer = false;
    public float speed = 300f;
    public int damage = 10;

    // Use this for initialization
    void Awake() {
        transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
        body = GetComponent<Rigidbody2D>();
    }

    public void Fire(Quaternion projectilRotation) {
        transform.rotation = projectilRotation;
        body.velocity = (projectilRotation * Vector2.up) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Ennemy>() || collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
