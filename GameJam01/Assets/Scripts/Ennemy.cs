using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ennemy : NetworkBehaviour
{
    [
        Header("Drop chance of bonus"),
        RangeAttribute(0, 100)
    ]
    public int dropChance;
    
    [Header("Enemy characteristic")]
    public float movementSpeed = 1F;

    [
        Header("Prefab"),
        Tooltip("GameObject à faire spawn à la mort!")
    ]
    public GameObject prefab;

    private PlayerControl currentTarget;
    private LifeManager lifeManager;
    private float contactDist = 0F;

    // Use this for initialization
    void Start() {
        // récupération du component LifeManager permettant de gérer la vie de mon ennemmi
        lifeManager = this.GetComponent<LifeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gestion de la mort et du drop
        handleDeath();
        // Gestion du ciblage et amorce du deplacement vers un joueur pour l'attaquer
        handleTargetingPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Projectiles>()) {
            if (collision.gameObject.GetComponent<Projectiles>().isFromMyPlayer) {
                GameManager.nbEnnemiesKilled++;
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.GetComponent<PlayerControl>()) {
            Destroy(gameObject);
        }
    }

    private void handleTargetingPlayer()
    {
        if (currentTarget != null)
        {
            if (Vector2.Distance(transform.position, currentTarget.transform.position) >= contactDist)
            {
                // Init mouvement guide line
                Vector2 axe = currentTarget.transform.position - gameObject.transform.position;
                axe.Normalize();
                gameObject.transform.Translate(axe * movementSpeed * Time.deltaTime);

                Vector3 difference = currentTarget.transform.position - transform.position;
                difference.Normalize();
                float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                gameObject.transform.Find("enemy_sprite").transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            }
        }
        else
        {
            PlayerControl[] potentialTarget = GameObject.FindObjectsOfType<PlayerControl>();
            if (potentialTarget != null && potentialTarget.Length > 0)
            {
                int luckyBastard = Random.Range(1, potentialTarget.Length + 1);
                currentTarget = potentialTarget[luckyBastard - 1];
            }
        }
    }

    private void handleDeath()
    {
        // Si j'ai un préfab, qu'il y a des chance de drop et que l'ennemy n'a plus de vie.
        if (prefab != null && dropChance != 0 && lifeManager.lifeValue == 0.0f)
        {
            Debug.Log("Aaaargh je meuuuuuuuuuuuuuuuuur!");
            if (dropChance == 100 || Random.Range(0, 101) <= dropChance)
            {
                Debug.Log("youhou on va spawn une caisse");
                Instantiate(prefab, new Vector3(), Quaternion.identity);
            }
        }
    }
}
