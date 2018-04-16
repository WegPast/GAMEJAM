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
