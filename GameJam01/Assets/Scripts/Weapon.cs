using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour
{

    public Projectiles projectileType; // Contains damage, speed etc...

    [Header("Weapon's properties")]
    [Range(0.05f, 2f)]
    public float fireRate;

    private GameObject fireSpot;


    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void FireProjectile(GameObject gun) {
        fireSpot = transform.GetChild(transform.childCount - 1).gameObject;

        GameObject projectile = Instantiate(projectileType.gameObject, fireSpot.transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectiles>().Fire(gun.transform.rotation);

        if (isLocalPlayer) {
            projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
        }
    }
}
