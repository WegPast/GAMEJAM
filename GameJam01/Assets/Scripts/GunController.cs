﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject attachedWeapon;

    private GameObject instantiatedWeapon;

    private void Start() {
        if (HasWeaponAttached()) {
            InstantiateWeapon();
        }
    }

    private void InstantiateWeapon() {
        instantiatedWeapon = Instantiate(attachedWeapon, transform.position, Quaternion.identity);
        instantiatedWeapon.transform.parent = transform;

        GameObject fireSpot = instantiatedWeapon.transform.Find("FireSpot").gameObject;
        fireSpot.transform.parent = instantiatedWeapon.transform;
        instantiatedWeapon.GetComponent<Weapon>().SetFireSpot(fireSpot.transform.position);
        Debug.Log(fireSpot.transform.position);

        // By default instantiated object take same parent transforme properties. 
        // GunRight has a scale x of -1. So here we reset weapon scale.
        instantiatedWeapon.transform.localScale = new Vector3(1f, 1f, 1F);
    }

    public void ChangeWeapon(GameObject newWeapon) {

        attachedWeapon = newWeapon;
        if (IsWeaponInstatiated()) {
            Destroy(instantiatedWeapon);
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
        if (instantiatedWeapon != null) {
            return true;
        } else {
            return false;
        }
    }

}
