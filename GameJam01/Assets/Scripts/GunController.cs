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

  private void Start()
  {
    InstantiateWeapon();
  }

  private void InstantiateWeapon()
  {
    instantiatedWeapon = Instantiate(attachedWeapon, transform.position, transform.parent.rotation);
    instantiatedWeapon.transform.parent = transform;

    // By default instantiated object take same parent transforme properties. 
    // GunRight has a scale x of -1. So here we reset weapon scale.
    instantiatedWeapon.transform.localScale = Vector3.one;
  }

  public void ChangeWeapon(GameObject newWeapon)
  {

    attachedWeapon = newWeapon;
    if (instantiatedWeapon)
    {
      Destroy(instantiatedWeapon);
    }
    InstantiateWeapon();

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

  public void UpdateDeltaFiringTime()
  {
    if (deltaTimeFire >= 1 / instantiatedWeapon.GetComponent<Weapon>().fireRate)
    {
      deltaTimeFire = 0;
    }
    deltaTimeFire += Time.deltaTime;
  }

  public void ResetDeltatTime()
  {
    deltaTimeFire = 0;
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
