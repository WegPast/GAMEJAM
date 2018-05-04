using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

  public GameObject weaponMenu;
  public GameObject skillsMenu;

  [Header("Left side")]
  public Image uiWeaponSpriteLeft;
  public Image hovercraftWeaponSpriteLeft;
  public Image uiWeaponSpriteRight;
  public Image hovercraftWeaponSpriteRight;

  [Header("Available weapons")]
  public Weapon[] availableWeapons;

  private int currentSelectedLeftWeaponIndex = 0;
  private int currentSelectedRightWeaponIndex = 0;

  // Use this for initialization
  void Start() {
    //foreach (var item in availableWeapons) {

    //  Debug.Log("--- Weapon : " + item.name + " ---");
    //  Debug.Log("Description : \n" + item.description);
    //  Debug.Log("Firerate : " + item.fireRate);
    //  Debug.Log("Projectile : " + item.projectileType.name);
    //}
  }

  // Update is called once per frame
  void Update() {

  }

  public void NextWeapon(string side) {
    if (side == "left") {
      if ((currentSelectedLeftWeaponIndex + 1) >= availableWeapons.Length) {
        currentSelectedLeftWeaponIndex = 0;
      } else {
        currentSelectedLeftWeaponIndex++;
      }
      SwitchWeaponsSprite(side, currentSelectedLeftWeaponIndex);
    } else {
      if ((currentSelectedRightWeaponIndex + 1) >= availableWeapons.Length) {
        currentSelectedRightWeaponIndex = 0;
      } else {
        currentSelectedRightWeaponIndex++;
      }
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
    }
  }

  public void PreviousWeapon(string side) {
    if (side == "left") {
      if ((currentSelectedLeftWeaponIndex - 1) < 0) {
        currentSelectedLeftWeaponIndex = availableWeapons.Length - 1;
      } else {
        currentSelectedLeftWeaponIndex--;
      }
      SwitchWeaponsSprite(side, currentSelectedLeftWeaponIndex);
    } else {
      if ((currentSelectedRightWeaponIndex - 1) < 0) {
        currentSelectedRightWeaponIndex = availableWeapons.Length - 1;
      } else {
        currentSelectedRightWeaponIndex--;
      }
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
    }
  }

  public void SwitchWeaponsSprite(string side, int index) {

    GameObject weaponBody = availableWeapons[index].transform.Find("Body").gameObject;
    if (side == "left") {
      uiWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      hovercraftWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
    if (side == "right") {
      uiWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      hovercraftWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
  }

  public void SwitchWeaponInfos(string side, int index) {
    Weapon selectedWeapon = availableWeapons[index];
    string description = selectedWeapon.description;
    float firerate = selectedWeapon.fireRate;
    Projectiles projectileType = selectedWeapon.projectileType;
    if (side == "left") {
    }
    if (side == "right") {
    }
  }

}
