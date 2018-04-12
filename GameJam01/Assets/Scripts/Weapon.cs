using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour {

    public Projectiles projectileType; // Contains damage, speed etc...
    public float fireRate;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FireProjectile(Quaternion gunRotation) {
        GameObject bullet1 = Instantiate(projectileType.gameObject, transform.position, Quaternion.identity) as GameObject;
        bullet1.GetComponent<Projectiles>().Fire(gunRotation);
        
        if (isLocalPlayer) {
            bullet1.GetComponent<Projectiles>().isFromMyPlayer = true;
        }
    }
}
