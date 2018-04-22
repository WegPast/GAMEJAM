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

    public GameObject fireSpot;

    private GameObject parentGameObject;
    private Animator animator;

    public void Start() {
        animator = GetComponent<Animator>();
    }

    public void FireProjectile(GameObject gun) {

        //if (isLocalPlayer) {
        Vector3 projectilePos = fireSpot.transform.position;

        GameObject projectile = Instantiate(projectileType.gameObject, projectilePos, Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectiles>().Fire(fireSpot.transform.rotation);
        projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
        animator.SetTrigger("fire");
        NetworkServer.Spawn(projectile);
        //}
    }
}
