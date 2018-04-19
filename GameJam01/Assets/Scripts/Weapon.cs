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

    public GameObject fireSpot;

    private GameObject parentGameObject;

    public void FireProjectile(GameObject gun) {


        Vector3 projectilePos = fireSpot.transform.position;

        GameObject projectile = Instantiate(projectileType.gameObject, projectilePos , Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectiles>().Fire(fireSpot.transform.rotation);

        if (isLocalPlayer) {
            projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
        }
    }
}
