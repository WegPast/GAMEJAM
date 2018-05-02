using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour
{

  public Projectiles projectileType; // Contains damage, speed etc...

  [Header("Weapon's properties")]
  [Range(0.05f, 30f)]
  public float fireRate;

  [Header("Weapon's info"), TextArea(1,5)]
  public string description;

  public GameObject fireSpot;

  private GameObject parentGameObject;
  private Animator animator;

  public void Start()
  {
    animator = GetComponent<Animator>();
  }

  public GameObject FireProjectile(GameObject gun, bool isFiredFromLocalPlayer)
  {
    Vector3 projectilePos = fireSpot.transform.position;

    GameObject projectile = Instantiate(projectileType.gameObject, projectilePos, Quaternion.identity) as GameObject;
    projectile.GetComponent<Projectiles>().Fire(fireSpot.transform.rotation);
    if (isFiredFromLocalPlayer)
    {
      projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
    }

    return projectile;
  }

  //Animation
  public void AnimationFiring()
  {
    animator.SetTrigger("fire");
  }
}
