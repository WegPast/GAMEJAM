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
    private GameObject parentGameObject;

    //public GameObject GetFireSpot() {
    //    return fireSpot;
    //}

    //public void SetFireSpot(Vector3 fSpot) {
    //    fireSpot = fSpot;
    //}

    private void Awake() {
        
    }
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void FireProjectile(GameObject gun) {
        


        fireSpot = gameObject.transform.GetChild(transform.childCount - 1).gameObject;
        fireSpot.transform.parent = gun.transform;

        Debug.Log("weapon ID : " + GetInstanceID());
        Debug.Log("gameObject.weapon ID : " + gameObject.GetInstanceID());
        Debug.Log("firespot ID : " + fireSpot.gameObject.GetInstanceID());

        Vector3 projectilePos = gun.transform.position;

        GameObject projectile = Instantiate(projectileType.gameObject, projectilePos , Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectiles>().Fire(gun.transform.rotation);

        if (isLocalPlayer) {
            projectile.GetComponent<Projectiles>().isFromMyPlayer = true;
        }
    }
}
