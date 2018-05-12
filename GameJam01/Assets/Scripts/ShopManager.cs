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
  public Image[] leftProjectilesSprites;
  public Button[] leftProjectilesBtn;

  [Space(20f)]
  [Header("Right side Weapon")]
  public Image imgWeaponSpriteRight;
  public Text txtWeaponDescRight;
  public Text txtWeaponSpecRight;
  public Image imgHCWeaponSpriteRight;

  [Header("Right side Projectile")]
  public Text txtProjectileDescRight;
  public Text txtProjectileSpecRight;
  public Image[] rightProjectilesSprites;
  public Button[] rightProjectilesBtn;

  [Header("Weapons")]
  [Space(20f)]
  [Header("--- Available weapons and projectiles ---")]
  public Weapon[] availableWeapons;

  [Header("Projectiles")]
  public Projectiles[] availableRifleProjectiles;
  public Projectiles[] availableMultigunProjectiles;


  [Header("Miscellenious")]
  public Sprite noProjectileSprite;

  private int currentSelectedLeftWeaponIndex = 0;
  private int currentSelectedRightWeaponIndex = 0;

  private Projectiles currentSelectedLeftProjectile;
  private Projectiles currentSelectedRightProjectile;

  private int currentSelectedLeftProjectileIndex = 0;
  private int currentSelectedRightProjectileIndex = 0;

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
    int leftIndex = 0, rightIndex = 0;
    string side;

    if (playerDataManager) {
      leftIndex = GetWeaponIndexByShopName(playerDataManager
        .localPlayerLeftWeapon
        );
    }
    if (leftIndex >= 0) {
      side = "left";
      currentSelectedLeftWeaponIndex = leftIndex;
      SwitchWeaponsSprite(side, currentSelectedLeftWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedLeftWeaponIndex);
      SwitchProjectileSprite(side, currentSelectedLeftWeaponIndex);
      SelectLeftAmmoType(GetProjectileIndexByShopName(availableWeapons[currentSelectedLeftWeaponIndex].projectileType.shopName, availableWeapons[currentSelectedLeftWeaponIndex]));

    }

    if (playerDataManager) {
      rightIndex = GetWeaponIndexByShopName(playerDataManager.localPlayerRightWeapon);
    }
    if (rightIndex >= 0) {
      side = "right";
      currentSelectedRightWeaponIndex = rightIndex;
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedRightWeaponIndex);
      SwitchProjectileSprite(side, currentSelectedRightWeaponIndex);
      SelectRightAmmoType(GetProjectileIndexByShopName(availableWeapons[currentSelectedRightWeaponIndex].projectileType.shopName, availableWeapons[currentSelectedRightWeaponIndex]));

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
      SwitchProjectileSprite(side, currentSelectedLeftWeaponIndex);
      SelectLeftAmmoType(0);
      SwitchProjectileInfos(side, 0);
    } else {
      if ((currentSelectedRightWeaponIndex + 1) >= availableWeapons.Length) {
        currentSelectedRightWeaponIndex = 0;
      } else {
        currentSelectedRightWeaponIndex++;
      }
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedRightWeaponIndex);
      SwitchProjectileSprite(side, currentSelectedRightWeaponIndex);

      SelectRightAmmoType(0);
      SwitchProjectileInfos(side, 0);
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
      SwitchProjectileSprite(side, currentSelectedLeftWeaponIndex);
      SelectLeftAmmoType(0);
      SwitchProjectileInfos(side, 0);
    } else {
      if ((currentSelectedRightWeaponIndex - 1) < 0) {
        currentSelectedRightWeaponIndex = availableWeapons.Length - 1;
      } else {
        currentSelectedRightWeaponIndex--;
      }
      SwitchWeaponsSprite(side, currentSelectedRightWeaponIndex);
      SwitchWeaponInfos(side, currentSelectedRightWeaponIndex);
      SwitchProjectileSprite(side, currentSelectedRightWeaponIndex);

      SelectRightAmmoType(0);
      SwitchProjectileInfos(side, 0);
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

  public void SwitchProjectileSprite(string side, int index) {

    Projectiles selectedProjectile = availableWeapons[index].projectileType;
    Projectiles[] availableProjectile = availableWeapons[index].availableProjectiles;
    if (side == "left") {
      for (int i = 0; i < leftProjectilesSprites.Length - 1; i++) {
        if (i <= (availableProjectile.Length - 1)) {
          leftProjectilesSprites[i].sprite = availableProjectile[i].GetComponent<SpriteRenderer>().sprite;
        } else {
          leftProjectilesSprites[i].sprite = noProjectileSprite;
        }

      }
    }
    if (side == "right") {
      for (int i = 0; i < rightProjectilesSprites.Length - 1; i++) {
        if (i <= (availableProjectile.Length - 1)) {
          rightProjectilesSprites[i].sprite = availableProjectile[i].GetComponent<SpriteRenderer>().sprite;
        } else {
          rightProjectilesSprites[i].sprite = noProjectileSprite;
        }

      }
    }
  }

  public void SwitchProjectileInfos(string side, int projectileIndex) {
    Weapon selectedWeapon = availableWeapons[0]; // default weapon

    if (side == "left") {
      selectedWeapon = availableWeapons[currentSelectedLeftWeaponIndex];
    }
    if (side == "right") {
      selectedWeapon = availableWeapons[currentSelectedRightWeaponIndex];
    }

    // fetching projectile informations
    string projectileDesc = selectedWeapon.availableProjectiles[projectileIndex].description;
    string projectileDmg = selectedWeapon.availableProjectiles[projectileIndex].damage.ToString();
    string projectileSpeed = selectedWeapon.availableProjectiles[projectileIndex].speed.ToString();
    string description = projectileDesc != "" ? projectileDesc : "- No description found. -";
    string damageTxt = projectileDmg != "" ? projectileDmg : "--";
    string speedTxt = projectileSpeed != "" ? projectileSpeed : "--";

    if (side == "left") {
      txtProjectileDescLeft.text = description;
      txtProjectileSpecLeft.text = "DMG : " + damageTxt + "\n" +
                                   "SPEED : " + speedTxt;
    }
    if (side == "right") {
      txtProjectileDescRight.text = description;
      txtProjectileSpecRight.text = "DMG : " + damageTxt + "\n" +
                                "SPEED : " + speedTxt;
    }
  }

  public void SwitchWeaponInfos(string side, int index) {
    Weapon selectedWeapon = availableWeapons[index];
    string description = selectedWeapon.description != "" ? selectedWeapon.description : "- No description found. -";
    string firerateTxt = selectedWeapon.fireRate.ToString() != "" ? selectedWeapon.fireRate.ToString() : "--";
    //Projectiles projectileType = selectedWeapon.projectileType;  // to be put in another fn
    if (side == "left") {
      txtWeaponDescLeft.text = description;
      txtWeaponSpecLeft.text = "FR : " + firerateTxt;
    }
    if (side == "right") {
      txtWeaponDescRight.text = description;
      txtWeaponSpecRight.text = "FR : " + firerateTxt;
    }
  }

  public void SelectLeftAmmoType(int index) {
    if (index <= availableWeapons[currentSelectedLeftWeaponIndex].availableProjectiles.Length - 1) {
      currentSelectedLeftProjectile = availableWeapons[currentSelectedLeftWeaponIndex].availableProjectiles[index];
      currentSelectedLeftProjectileIndex = GetProjectileIndexByShopName(availableWeapons[currentSelectedLeftWeaponIndex].projectileType.shopName, availableWeapons[currentSelectedLeftWeaponIndex]);
      Button clickedBtn = leftProjectilesBtn[index];
      clickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 1f);
      SwitchProjectileInfos("left", index);
      for (int i = 0; i < leftProjectilesBtn.Length; i++) {
        if(i != index) {
          Debug.Log(index + "  "+ i);
          Button notClickedBtn = leftProjectilesBtn[i];
          notClickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 0f);
        }
      }
    }
  }

  public void SelectRightAmmoType(int index) {
    if (index <= availableWeapons[currentSelectedRightWeaponIndex].availableProjectiles.Length - 1) {
      currentSelectedRightProjectile = availableWeapons[currentSelectedRightWeaponIndex].availableProjectiles[index];
      currentSelectedRightProjectileIndex = GetProjectileIndexByShopName(availableWeapons[currentSelectedRightWeaponIndex].projectileType.shopName, availableWeapons[currentSelectedRightWeaponIndex]);
      Button clickedBtn = rightProjectilesBtn[index];
      clickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 1f);
      SwitchProjectileInfos("right", index);
      for (int i = 0; i < rightProjectilesBtn.Length; i++) {
        if (i != index) {
          Button notClickedBtn = rightProjectilesBtn[i];
          notClickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 0f);
        }
      }
    }
  }


  public int GetWeaponIndexByShopName(string shopName) {
    int index = 0;
    foreach (var weapon in availableWeapons) {
      if (weapon.shopName == shopName) {
        return index;
      }
      index++;
    }
    return -1;
  }

  /// <summary>
  /// Return the index of weapon available projectiles array by its shop name and the weapon pass in parameter
  /// </summary>
  /// <param name="shopName">The projectile shopName</param>
  /// <param name="weapon">The Weapon(script)</param>
  /// <returns>The index, or -1 if not found</returns>
  public int GetProjectileIndexByShopName(string shopName, Weapon weapon) {
    int index = 0;
    Projectiles[] availableProjectiles = weapon.availableProjectiles;
    foreach (var projectile in availableProjectiles) {
      if (projectile.shopName == shopName) {
        return index;
      }
      index++;
    }
    return -1;
  }

  public void DisplayMenu(string menuName) {

    switch (menuName) {

      case "skill":
        weaponMenu.SetActive(false);
        skillsMenu.SetActive(true);
        break;

      case "weapon":
        weaponMenu.SetActive(true);
        skillsMenu.SetActive(false);
        break;

      default:
        Debug.LogError("Menu '" + menuName + "' not found");
        break;
    }

  }

  public void TestSaveData() {
    Debug.Log("currentSelectedLeftWeaponIndex " + currentSelectedLeftWeaponIndex
      + "\n currentSelectedLeftProjectile " + currentSelectedLeftProjectile
      + "\n currentSelectedRightWeaponIndex " + currentSelectedRightWeaponIndex
      + "\n currentSelectedRightProjectile " + currentSelectedRightProjectile);
  }
}
