using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

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
      GameObject weaponBody = availableWeapons[currentSelectedLeftWeaponIndex].transform.Find("Body").gameObject;
      uiWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      hovercraftWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    } else {
      if ((currentSelectedRightWeaponIndex + 1) >= availableWeapons.Length) {
        currentSelectedRightWeaponIndex = 0;
      } else {
        currentSelectedRightWeaponIndex++;
      }
      GameObject weaponBody = availableWeapons[currentSelectedRightWeaponIndex].transform.Find("Body").gameObject;
      uiWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      hovercraftWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
  }

  public void PreviousWeapon(string side) {
    if (side == "left") {
      if ((currentSelectedLeftWeaponIndex - 1) < 0) {
        currentSelectedLeftWeaponIndex = availableWeapons.Length - 1;
      } else {
        currentSelectedLeftWeaponIndex--;
      }
      GameObject weaponBody = availableWeapons[currentSelectedLeftWeaponIndex].transform.Find("Body").gameObject;
      uiWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      hovercraftWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;

    } else {

      if ((currentSelectedRightWeaponIndex - 1) >= availableWeapons.Length) {
        currentSelectedRightWeaponIndex = 0;
      } else {
        currentSelectedRightWeaponIndex++;
      }
      GameObject weaponBody = availableWeapons[currentSelectedRightWeaponIndex].transform.Find("Body").gameObject;
      uiWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      hovercraftWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
  }
}
