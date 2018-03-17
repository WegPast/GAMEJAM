﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{

    private Rigidbody2D body;
    public float speed = 300f;

    // Use this for initialization
    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    public void Fire(Quaternion projectilRotation) {
        transform.rotation = projectilRotation;
        body.velocity = (projectilRotation * Vector2.up) * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Ennemy>() || collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }

}
