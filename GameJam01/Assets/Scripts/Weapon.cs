using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour
{

    public Projectiles projectileType; // Contains damage, speed etc...
    public float fireRate;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void FireProjectile(Quaternion gunRotation) {
        GameObject projectile = Instantiate(projectileType.gameObject, transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectiles>().Fire(gunRotation);

        if (isLocalPlayer) {
            projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
        }
    }
}
