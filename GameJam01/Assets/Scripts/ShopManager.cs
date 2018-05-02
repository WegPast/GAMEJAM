using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

  [Header("Left side")]
  public Image uiWeaponSpriteLeft;
  public Image hovercraftWeaponSpriteLeft;
  public Image uiWeaponSpriteRight;
  public Image hovercraftWeaponSpriteRight;

  [Header("Available weapons")]
  public Weapon[] availableWeapons;

  // Use this for initialization
  void Start () {
    foreach (var item in availableWeapons) {

      Debug.Log("--- Weapon : "+item.name+"----");
      Debug.Log("Description : \n"+item.description);
      Debug.Log("Firerate : "+item.fireRate);
      Debug.Log("Projectile : "+item.projectileType.name);
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
