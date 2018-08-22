using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour {


  [Header("Weapon's properties")]
  [Range(0.05f, 50f)]
  public float fireRate;
  public Projectiles projectileType; // Contains damage, speed etc...
  [Tooltip("Weapon's availbale projectiles")]
  public Projectiles[] availableProjectiles;

  [Header("Weapon's informations"), TextArea(1, 5), Tooltip("A short description for the weapon")]
  public string description;
  [Tooltip("Weapon's name for the shop")]
  public string shopName;
  [Tooltip("Weapon's price for the shop")]
  public int shopPrice;


  [Header("Behaviours"), Space(10f)]
  public GameObject fireSpot;
  [Range(0f, 20f)]
  public float spreading = 0f;

  private GameObject parentGameObject;
  private Animator animator;

  public void Start() {
    animator = GetComponent<Animator>();
  }

  public GameObject FireProjectile(GameObject gun, bool isFiredFromLocalPlayer) {

    Vector3 projectilePos = fireSpot.transform.position;
    Quaternion fireSpotRotation = fireSpot.transform.rotation;
    if (spreading > 0f) {
      float randomSpread = Random.Range(-spreading, spreading);
      Quaternion rotat = Quaternion.Euler(fireSpot.transform.rotation.x, fireSpot.transform.rotation.y, fireSpot.transform.rotation.z + randomSpread);
      fireSpotRotation = fireSpot.transform.rotation * rotat;
    }
    GameObject projectile = Instantiate(projectileType.gameObject, projectilePos, fireSpotRotation) as GameObject;
    projectile.GetComponent<Projectiles>().Fire(fireSpotRotation);
    if (isFiredFromLocalPlayer) {
      projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
    }

    return projectile;
  }

  //Animation
  public void AnimationFiring() {
    animator.SetTrigger("fire");
  }
}
