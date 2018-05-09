using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

  public GameObject weaponMenu;
  public GameObject skillsMenu;
  public PlayerDataManager playerDataManager;

  [Header("Left side Weapon")]
  public Image imgWeaponSpriteLeft;
  public Text txtWeaponDescLeft;
  public Text txtWeaponSpecLeft;
  public Image imgHCWeaponSpriteLeft;

  [Header("Left side Projectile")]
  public Text txtProjectileDescLeft;
  public Text txtProjectileSpecLeft;

  [Header("Right side Weapon")]
  public Image imgWeaponSpriteRight;
  public Image imgHCWeaponSpriteRight;

  [Header("Available weapons")]
  public Weapon[] availableWeapons;


  private int currentSelectedLeftWeaponIndex = 0;
  private int currentSelectedRightWeaponIndex = 0;

  // Use this for initialization
  void Start() {
    playerDataManager = FindObjectOfType<PlayerDataManager>();
    if (!playerDataManager) {
      Debug.LogWarning("ShopManager can't find any PlayerDataManager !");
    }
    InitShop();
  }

  // Update is called once per frame
  void Update() {

  }

  public void InitShop() {

    if (playerDataManager) {
      int leftIndex = GetWeaponIndexByShopName(playerDataManager.localPlayerLeftWeapon.shopName);
      if (leftIndex >= 0) {
        currentSelectedLeftWeaponIndex = leftIndex;
        SwitchWeaponsSprite("left", currentSelectedLeftWeaponIndex);
      }

      int rightIndex = GetWeaponIndexByShopName(playerDataManager.localPlayerRightWeapon.shopName);
      if (rightIndex >= 0) {
        currentSelectedRightWeaponIndex = rightIndex;
        SwitchWeaponsSprite("right", currentSelectedRightWeaponIndex);
      }

    }
  }

  /**
   * Change weapon description/info/sprite depending on side
   * 
   * @param : side (string) the side to change
   * 
   * @TODO: need refactoring !!
   * 
   **/
  public void NextWeapon(string side) {
    if (side == "left") {
      if ((currentSelectedLeftWeaponIndex + 1) >= availableWeapons.Length) {
        currentSelectedLeftWeaponIndex = 0;
      } else {
        currentSelectedLeftWeaponIndex++;
      }
      SwitchWeaponsSprite(side, currentSelectedLeftWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedLeftWeaponIndex);
    } else {
      if ((currentSelectedRightWeaponIndex + 1) >= availableWeapons.Length) {
        currentSelectedRightWeaponIndex = 0;
      } else {
        currentSelectedRightWeaponIndex++;
      }
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedRightWeaponIndex);
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
      SwitchWeaponInfos(side, currentSelectedLeftWeaponIndex);
    } else {
      if ((currentSelectedRightWeaponIndex - 1) < 0) {
        currentSelectedRightWeaponIndex = availableWeapons.Length - 1;
      } else {
        currentSelectedRightWeaponIndex--;
      }
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedRightWeaponIndex);
    }
  }

  public void SwitchWeaponsSprite(string side, int index) {

    GameObject weaponBody = availableWeapons[index].transform.Find("Body").gameObject;
    if (side == "left") {
      imgWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      imgHCWeaponSpriteLeft.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
    if (side == "right") {
      imgWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      imgHCWeaponSpriteRight.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
  }

  public void SwitchWeaponInfos(string side, int index) {
    Weapon selectedWeapon = availableWeapons[index];
    string description = selectedWeapon.description != "" ? selectedWeapon.description : "- No description found. -";
    string firerateTxt = selectedWeapon.fireRate.ToString() != "" ? selectedWeapon.fireRate.ToString() : "unknown";
    //Projectiles projectileType = selectedWeapon.projectileType;  // to be put in another fn
    if (side == "left") {
      txtWeaponDescLeft.text = description;
      txtWeaponSpecLeft.text = "FR : " + firerateTxt;
    }
    if (side == "right") {
      // @TODO the same as above when right panel is finished
    }
  }

  public int GetWeaponIndexByShopName(string shopName) {
    int index = 0;
    foreach (var weapon in availableWeapons) {
      if(weapon.shopName == shopName) {
        return index;
      }
      index++;
    }
    return -1;
  }

}
