using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ennemy : NetworkBehaviour
{

    private PlayerControl currentTarget;
    private float contactDist = 0F;

    [Header("Enemy characteristic")]
    public float movementSpeed = 1F;

    // Use this for initialization
    void Start() {
        //movementSpeed = 0.05F;
    }

    // Update is called once per frame
    void Update() {
        if (currentTarget != null) {
            if (Vector2.Distance(transform.position, currentTarget.transform.position) >= contactDist) {
                // Init mouvement guide line
                Vector2 axe = currentTarget.transform.position - gameObject.transform.position;
                axe.Normalize();
                gameObject.transform.Translate(axe * movementSpeed * Time.deltaTime);
            }
        } else {
            PlayerControl[] potentialTarget = GameObject.FindObjectsOfType<PlayerControl>();
            if (potentialTarget != null && potentialTarget.Length > 0) {
                int luckyBastard = Random.Range(1, potentialTarget.Length + 1);
                currentTarget = potentialTarget[luckyBastard - 1];
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Projectiles>() || collision.gameObject.GetComponent<PlayerControl>()) {
            Destroy(gameObject);
        }
    }
}
