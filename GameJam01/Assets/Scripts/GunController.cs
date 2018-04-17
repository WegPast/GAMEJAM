using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject attachedWeapon;

    private GameObject instantiatedWepaon;

    private void Start() {
        if (HasWeaponAttached()) {
            InstantiateWeapon();
        }
    }

    private void InstantiateWeapon() {
        instantiatedWepaon = Instantiate(attachedWeapon, transform.position, Quaternion.identity);
        instantiatedWepaon.transform.parent = transform;

        // By default instantiated object take same parent transforme properties. 
        // GunRight has a scale x of -1. So here we reset weapon scale.
        instantiatedWepaon.transform.localScale = new Vector3(1f, 1f, 1F);
    }

    public void ChangeWeapon(GameObject newWeapon) {

        attachedWeapon = newWeapon;
        if (IsWeaponInstatiated()) {
            Destroy(instantiatedWepaon);
        }
        InstantiateWeapon();
        
    }

    public GameObject GetAttachedWeapon() {
        if (HasWeaponAttached()) {
            return attachedWeapon;
        } else {
            return null;
        }
    }

    public bool HasWeaponAttached() {
        if (attachedWeapon != null) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsWeaponInstatiated() {
        if (instantiatedWepaon != null) {
            return true;
        } else {
            return false;
        }
    }

}
