using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectiles : NetworkBehaviour
{

    private Rigidbody2D body;
    public float speed = 2f;

    // Use this for initialization
    void Awake() {
        body = GetComponent<Rigidbody2D>();
        speed = 2f;
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
