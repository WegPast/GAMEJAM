using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ennemy : NetworkBehaviour {

  [
      Header("Drop chance of bonus"),
      Range(0, 100)
  ]
  public int dropChance;
  [Header("Enemy characteristic")]
  public float movementSpeed = 1F;
  [
      Header("Prefab"),
      Tooltip("GameObject à faire spawn à la mort!")
  ]
  public GameObject prefab;

  private Animator animator;
  private PlayerControl currentTarget;
  private LifeManager lifeManager;
  private float contactDist = 0F;

  // Use this for initialization
  void Start() {
    animator = GetComponent<Animator>();
    // récupération du component LifeManager permettant de gérer la vie de mon ennemmi
    lifeManager = this.GetComponent<LifeManager>();
  }

  // Update is called once per frame
  // Scripting de l'ennemy
  void Update() {
    // Gestion de la mort et du drop
    HandleDeath();
    // Gestion du ciblage et amorce du deplacement vers un joueur pour l'attaquer
    HandleTargetingPlayer();
  }

  //private void OnTriggerEnter2D(Collider2D collision) {

  //  Projectiles isProjectiles = collision.gameObject.GetComponent<Projectiles>();
  //  ProjectileInnerBody isProjectileInnerBody = collision.gameObject.GetComponent<ProjectileInnerBody>();
  //  Explosive isExplosive = collision.gameObject.GetComponent<Explosive>();
  //  Debug.Log(name + " has been hit by explosive : " + isExplosive + " | projInnerBdy : " + isProjectileInnerBody + " | projctl : " + isProjectiles);
  //  if ((isProjectiles || isProjectileInnerBody || isExplosive) && this.animator) {
  //    animator.SetTrigger("BEING_HIT");
  //  }
  //}

  private void OnCollisionEnter2D(Collision2D collision) {
    PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
    if (player) {
      // Ici on compare la vie du joueur à celle du mon instance d'ennemy
      HandleCollisionDamages(player.GetComponent<LifeManager>());
    }
  }


  private void HandleCollisionDamages(LifeManager playerLifeManager) {
    int dammageToPlayer = this.lifeManager.lifeValue;
    this.lifeManager.Hit(playerLifeManager.lifeValue);
    playerLifeManager.Hit(dammageToPlayer);
  }

  private void HandleTargetingPlayer() {
    // Si je n'ai pas de cible
    if (this.currentTarget != null) {
      if (Vector2.Distance(transform.position, currentTarget.transform.position) >= contactDist) {
        // Init mouvement guide line
        Vector2 axe = this.currentTarget.transform.position - this.gameObject.transform.position;
        axe.Normalize();
        this.gameObject.transform.Translate(axe * movementSpeed * Time.deltaTime);

        Vector3 difference = this.currentTarget.transform.position - this.transform.position;
        difference.Normalize();
        float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.gameObject.transform.Find("enemy_sprite").transform.rotation = Quaternion.Euler(0f, 0f, rotation);
      }
    } else {
      PlayerControl[] potentialTarget = GameObject.FindObjectsOfType<PlayerControl>();
      if (potentialTarget != null && potentialTarget.Length > 0) {
        this.currentTarget = potentialTarget[Random.Range(1, potentialTarget.Length + 1) - 1];
      }
    }
  }

  private void HandleDeath() {
    // Si j'ai un préfab, qu'il y a des chance de drop et que l'ennemy n'a plus de vie.
    if (this.lifeManager && this.lifeManager.lifeValue == 0.0f) {
      if (this.prefab != null && this.dropChance != 0) {
        if (this.dropChance == 100 || Random.Range(0, 101) <= this.dropChance) {
          Instantiate(this.prefab, this.gameObject.transform.position, Quaternion.identity);
        }
      }
      Destroy(this.gameObject);
    }
  }
}
