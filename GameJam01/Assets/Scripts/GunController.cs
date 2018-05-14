using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : MonoBehaviour
{

  public GameObject attachedWeapon;

  private GameObject instantiatedWeapon;
  private float deltaTimeFire;
  private float deltaTimeFireMax;

  private void Start()
  {
    InstantiateWeapon();
    deltaTimeFireMax = 100;
    deltaTimeFire = deltaTimeFireMax;
  }

  private void InstantiateWeapon(Projectiles projectiles = null)
  {
    instantiatedWeapon = Instantiate(attachedWeapon, transform.position, transform.parent.rotation);
    instantiatedWeapon.transform.parent = transform;

    // By default instantiated object take same parent transforme properties. 
    // GunRight has a scale x of -1. So here we reset weapon scale.
    instantiatedWeapon.transform.localScale = Vector3.one;

    if (projectiles) {
      instantiatedWeapon.GetComponent<Weapon>().projectileType = projectiles;
    }
  }

  public void ChangeWeapon(GameObject newWeapon, Projectiles projectiles = null)
  {

    attachedWeapon = newWeapon;
    if (instantiatedWeapon)
    {
      Destroy(instantiatedWeapon);
    }
    InstantiateWeapon(projectiles);

  }

  public GameObject GetAttachedWeapon()
  {
    if (this.instantiatedWeapon == null)
    {
      this.InstantiateWeapon();
    }
    return this.instantiatedWeapon;
  }

  public GameObject FireGun(bool isFiredFromLocalPlayer)
  {
    GameObject projectile = null;
    if (deltaTimeFire >= 1 / instantiatedWeapon.GetComponent<Weapon>().fireRate)
    {
      projectile = instantiatedWeapon.GetComponent<Weapon>().FireProjectile(gameObject, isFiredFromLocalPlayer);
    }

    return projectile;
  }

  public void UpdateDeltaFiringTime(bool isFiring)
  {
    if (isFiring && deltaTimeFire >= 1 / instantiatedWeapon.GetComponent<Weapon>().fireRate)
    {
      deltaTimeFire = 0;
    }
    if(deltaTimeFire< deltaTimeFireMax)
    {
      deltaTimeFire += Time.deltaTime;
    }
    
  }

  //Animation
  public void AnimationFiring()
  {
    if (deltaTimeFire >= 1 / instantiatedWeapon.GetComponent<Weapon>().fireRate)
    {
      instantiatedWeapon.GetComponent<Weapon>().AnimationFiring();
    }
  }

}
