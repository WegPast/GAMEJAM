using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

  public GameObject weaponMenu;
  public GameObject skillsMenu;
  public Text playerCreditsText;
  public PlayerDataManager playerDataManager;
  public Image creditEffect;

  [Header("Left side Weapon")]
  public Image leftImgWeaponSprite;
  public Text leftTxtWeaponName;
  public Text leftTxtWeaponPrice;
  public Text leftTxtWeaponDesc;
  public Text leftTxtWeaponSpec;
  public Text leftTxtWeaponStatus;
  public Image leftImgHCWeaponSprite;

  [Header("Left side Projectile")]
  public Text leftTxtProjectileName;
  public Text leftTxtProjectilePrice;
  public Text leftTxtProjectileDesc;
  public Text leftTxtProjectileSpec;
  public Text[] leftTxtProjectileStatus = new Text[5];
  public Image[] leftProjectilesSprites;
  public Button[] leftProjectilesBtn;
  public Text leftTextBtnBuyProj;
  public Text leftTextBtnBuyWeap;

  [Header("Right side Weapon"), Space(20f)]
  public Image rightImgWeaponSprite;
  public Text rightTxtWeaponName;
  public Text rightTxtWeaponPrice;
  public Text rightTxtWeaponDesc;
  public Text rightTxtWeaponSpec;
  public Text rightTxtWeaponStatus;
  public Image rightImgHCWeaponSprite;
  public Text rightTextBtnBuyWeap;

  [Header("Right side Projectile")]
  public Text rightTxtProjectileName;
  public Text rightTxtProjectilePrice;
  public Text rightTxtProjectileDesc;
  public Text rightTxtProjectileSpec;
  public Text[] rightTxtProjectileStatus = new Text[5];
  public Image[] rightProjectilesSprites;
  public Button[] rightProjectilesBtn;
  public Text rightTextBtnBuyProj;

  [Header("Weapons"), Header("--- Available weapons ---"), Space(20f)]
  private Weapon[] availablePlayerWeapons;

  //[Header("Projectiles")]
  //public Projectiles[] availableRifleProjectiles;
  //public Projectiles[] availableMultigunProjectiles;

  [Header("Shop Manager properties")]
  public Sprite noProjectileSprite;

  // === Private vars ===
  // Currently selected weapons
  private int leftCurrentSelectedWeaponIndex = 0;
  private int rightCurrentSelectedWeaponIndex = 0;
  private int leftCurrentSelectedWeaponPrice = 0;
  private int rightCurrentSelectedWeaponPrice = 0;
  private int leftCurrentEquippedWeaponIndex = 0;
  private int rightCurrentEquippedWeaponIndex = 0;

  // Currently selected projectiles
  private Projectiles leftCurrentSelectedProjectile;
  private int leftCurrentSelectedProjectileIndex = 0; // index in availabeProjectile of a corresponding weapon
  private int leftCurrentSelectedProjectilePrice = 0;
  private Projectiles rightCurrentSelectedProjectile;
  private int rightCurrentSelectedProjectileIndex = 0; // index in availabeProjectile of a corresponding weapon
  private int rightCurrentSelectedProjectilePrice = 0;
  private int leftCurrentEquippedProjectileIndex = 0;
  private int rightCurrentEquippedProjectileIndex = 0;

  // index will be the selected weapon, the value the projectile index in the available projectile weapon'array
  private int[] leftSelectedProjByWeap;
  private int[] rightSelectedProjByWeap;

  public class ItemShopStatus {
    public Color color;
    public string text;

    public ItemShopStatus(Color color, string text) {
      this.color = color;
      this.text = text;
    }
  }

  private ItemShopStatus UNLOCKED = new ItemShopStatus(new Color(0f, 1f, 0f, 1f), "unlocked");
  private ItemShopStatus LOCKED = new ItemShopStatus(new Color(1f, 0f, 0f, 1f), "locked");
  private ItemShopStatus EQUIPPED = new ItemShopStatus(new Color(0f, 0.68f, 1f, 1f), "equipped");
  private ItemShopStatus DEFAULT = new ItemShopStatus(Color.white, "default");


  // Use this for initialization
  void Start() {
    playerDataManager = FindObjectOfType<PlayerDataManager>();
    if (!playerDataManager) {
      Debug.LogError("ShopManager can't find any PlayerDataManager !");
    }
    availablePlayerWeapons = playerDataManager.availableWeapons;
    leftSelectedProjByWeap = new int[availablePlayerWeapons.Length];
    rightSelectedProjByWeap = new int[availablePlayerWeapons.Length];
    rightSelectedProjByWeap[0] = 0;
    InitShop();
  }

  // Update is called once per frame
  void Update() {

  }

  public void InitShop() {
    int leftIndex = 0, rightIndex = 0;
    int leftProjectileIndex = 0, rightProjectileIndex = 0;
    string side;

    // general initialization
    if (playerDataManager) {
      playerCreditsText.text = playerDataManager.localPlayerCredits.ToString();
    }

    if (playerDataManager) {
      leftIndex = playerDataManager.localPlayerLeftWeaponIndex;
      leftProjectileIndex = playerDataManager.localPlayerLeftProjectilesIndex;
    }
    if (leftIndex >= 0) {
      side = "left";
      leftCurrentSelectedWeaponIndex = leftIndex;
      leftCurrentSelectedProjectileIndex = leftProjectileIndex;
      leftCurrentEquippedProjectileIndex = leftProjectileIndex;
      SwitchWeaponsSprite(side, leftCurrentSelectedWeaponIndex);
      SwitchWeaponInfos(side, leftCurrentSelectedWeaponIndex);
      SwitchProjectileSprite(side, leftCurrentSelectedWeaponIndex);
      SelectLeftAmmoType(leftProjectileIndex);
    }

    if (playerDataManager) {
      rightIndex = playerDataManager.localPlayerRightWeaponIndex;
      rightProjectileIndex = playerDataManager.localPlayerRightProjectilesIndex;
    }
    if (rightIndex >= 0) {
      side = "right";
      rightCurrentSelectedWeaponIndex = rightIndex;
      rightCurrentSelectedProjectileIndex = rightProjectileIndex;
      rightCurrentEquippedProjectileIndex = rightProjectileIndex;
      SwitchWeaponsSprite(side, rightCurrentSelectedWeaponIndex);
      SwitchWeaponInfos(side, rightCurrentSelectedWeaponIndex);
      SwitchProjectileSprite(side, rightCurrentSelectedWeaponIndex);
      SelectRightAmmoType(rightProjectileIndex);
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
      if ((leftCurrentSelectedWeaponIndex + 1) >= availablePlayerWeapons.Length) {
        leftCurrentSelectedWeaponIndex = 0;
      } else {
        leftCurrentSelectedWeaponIndex++;
      }
      SwitchWeaponsSprite(side, leftCurrentSelectedWeaponIndex);
      SwitchWeaponInfos(side, leftCurrentSelectedWeaponIndex);
      SwitchProjectileSprite(side, leftCurrentSelectedWeaponIndex);
      SelectLeftAmmoType(leftSelectedProjByWeap[leftCurrentSelectedWeaponIndex]);
      SwitchProjectileInfos(side, leftSelectedProjByWeap[leftCurrentSelectedWeaponIndex]);
      UpdateProjectilesStatus(side);
    } else {
      if ((rightCurrentSelectedWeaponIndex + 1) >= availablePlayerWeapons.Length) {
        rightCurrentSelectedWeaponIndex = 0;
      } else {
        rightCurrentSelectedWeaponIndex++;
      }
      SwitchWeaponsSprite(side, rightCurrentSelectedWeaponIndex);
      SwitchWeaponInfos(side, rightCurrentSelectedWeaponIndex);
      SwitchProjectileSprite(side, rightCurrentSelectedWeaponIndex);

      SelectRightAmmoType(rightSelectedProjByWeap[rightCurrentSelectedWeaponIndex]);
      SwitchProjectileInfos(side, rightSelectedProjByWeap[rightCurrentSelectedWeaponIndex]);
      UpdateProjectilesStatus(side);
    }
  }

  public void PreviousWeapon(string side) {
    if (side == "left") {
      if ((leftCurrentSelectedWeaponIndex - 1) < 0) {
        leftCurrentSelectedWeaponIndex = availablePlayerWeapons.Length - 1;
      } else {
        leftCurrentSelectedWeaponIndex--;
      }
      SwitchWeaponsSprite(side, leftCurrentSelectedWeaponIndex);
      SwitchWeaponInfos(side, leftCurrentSelectedWeaponIndex);
      SwitchProjectileSprite(side, leftCurrentSelectedWeaponIndex);
      SelectLeftAmmoType(leftSelectedProjByWeap[leftCurrentSelectedWeaponIndex]);
      SwitchProjectileInfos(side, leftSelectedProjByWeap[leftCurrentSelectedWeaponIndex]);
      UpdateProjectilesStatus(side);
    } else {
      if ((rightCurrentSelectedWeaponIndex - 1) < 0) {
        rightCurrentSelectedWeaponIndex = availablePlayerWeapons.Length - 1;
      } else {
        rightCurrentSelectedWeaponIndex--;
      }
      SwitchWeaponsSprite(side, rightCurrentSelectedWeaponIndex);
      SwitchWeaponInfos(side, rightCurrentSelectedWeaponIndex);
      SwitchProjectileSprite(side, rightCurrentSelectedWeaponIndex);

      SelectRightAmmoType(rightSelectedProjByWeap[rightCurrentSelectedWeaponIndex]);
      SwitchProjectileInfos(side, rightSelectedProjByWeap[rightCurrentSelectedWeaponIndex]);
      UpdateProjectilesStatus(side);
    }
  }

  public void SwitchWeaponsSprite(string side, int index) {

    GameObject weaponBody = availablePlayerWeapons[index].transform.Find("Body").gameObject;
    if (side == "left") {
      leftImgWeaponSprite.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      leftImgHCWeaponSprite.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
    if (side == "right") {
      rightImgWeaponSprite.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
      rightImgHCWeaponSprite.sprite = weaponBody.GetComponent<SpriteRenderer>().sprite;
    }
  }

  public void SwitchProjectileSprite(string side, int index) {

    Projectiles selectedProjectile = availablePlayerWeapons[index].projectileType;
    Projectiles[] availableProjectile = availablePlayerWeapons[index].availableProjectiles;
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
    Weapon selectedWeapon = availablePlayerWeapons[0]; // default weapon

    if (side == "left") {
      selectedWeapon = availablePlayerWeapons[leftCurrentSelectedWeaponIndex];
    }
    if (side == "right") {
      selectedWeapon = availablePlayerWeapons[rightCurrentSelectedWeaponIndex];
    }

    // fetching projectile informations
    string projectileDesc = selectedWeapon.availableProjectiles[projectileIndex].description;
    string projectileDmg = selectedWeapon.availableProjectiles[projectileIndex].damage.ToString();
    string projectileSpeed = selectedWeapon.availableProjectiles[projectileIndex].speed.ToString();
    string projectilePrice = selectedWeapon.availableProjectiles[projectileIndex].shopPrice.ToString();
    string description = projectileDesc != "" ? projectileDesc : "- No description found. -";
    string damageTxt = projectileDmg != "" ? projectileDmg : "--";
    string speedTxt = projectileSpeed != "" ? projectileSpeed : "--";
    string priceTxt = projectilePrice != "" ? projectilePrice : "--";

    if (side == "left") {
      leftTxtProjectileDesc.text = description;
      leftTxtProjectilePrice.text = priceTxt + " cr";
      leftTxtProjectileSpec.text = "DMG : " + damageTxt + "\n" +
                                   "SPEED : " + speedTxt;
      leftCurrentSelectedProjectilePrice = selectedWeapon.availableProjectiles[projectileIndex].shopPrice;
      SwitchProjectileName("left");
    }
    if (side == "right") {
      rightTxtProjectileDesc.text = description;
      rightTxtProjectilePrice.text = priceTxt + " cr";
      rightTxtProjectileSpec.text = "DMG : " + damageTxt + "\n" +
                                "SPEED : " + speedTxt;
      rightCurrentSelectedProjectilePrice = selectedWeapon.availableProjectiles[projectileIndex].shopPrice;
      SwitchProjectileName("right");
    }
  }

  public void SwitchWeaponInfos(string side, int index) {
    Weapon selectedWeapon = availablePlayerWeapons[index];
    string description = selectedWeapon.description != "" ? selectedWeapon.description : "- No description found. -";
    string firerateTxt = selectedWeapon.fireRate.ToString() != "" ? selectedWeapon.fireRate.ToString() : "--";
    string priceTxt = selectedWeapon.shopPrice.ToString() != "" ? selectedWeapon.shopPrice.ToString() : "--";
    //Projectiles projectileType = selectedWeapon.projectileType;  // to be put in another fn
    if (side == "left") {
      leftTxtWeaponDesc.text = description;
      leftTxtWeaponSpec.text = "FR : " + firerateTxt;
      leftTxtWeaponPrice.text = priceTxt + " cr";
      leftCurrentSelectedWeaponPrice = selectedWeapon.shopPrice;
      leftCurrentSelectedProjectileIndex = 0;
      leftCurrentEquippedProjectileIndex = 0;
      SelectLeftAmmoType(0);
      SwitchWeaponName(side);
      UpdateProjectilesStatus(side);
    }
    if (side == "right") {
      rightTxtWeaponDesc.text = description;
      rightTxtWeaponSpec.text = "FR : " + firerateTxt;
      rightTxtWeaponPrice.text = priceTxt + " cr";
      rightCurrentSelectedWeaponPrice = selectedWeapon.shopPrice;
      rightCurrentSelectedProjectileIndex = 0;
      rightCurrentEquippedProjectileIndex = 0;
      SelectRightAmmoType(0);
      SwitchWeaponName(side);
      UpdateProjectilesStatus(side);
    }
  }

  public void SelectLeftAmmoType(int btnIndex) {
    if (btnIndex <= availablePlayerWeapons[leftCurrentSelectedWeaponIndex].availableProjectiles.Length - 1) {
      leftCurrentSelectedProjectile = availablePlayerWeapons[leftCurrentSelectedWeaponIndex].availableProjectiles[btnIndex];
      leftCurrentSelectedProjectileIndex = btnIndex;
      leftSelectedProjByWeap[leftCurrentSelectedWeaponIndex] = btnIndex;
      Button clickedBtn = leftProjectilesBtn[btnIndex];
      clickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 1f);
      SwitchProjectileInfos("left", btnIndex);
      UpdateProjectilesStatus("left");
      for (int i = 0; i < leftProjectilesBtn.Length; i++) {
        if (i != btnIndex) {
          Button notClickedBtn = leftProjectilesBtn[i];
          notClickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 0f);
        }
      }
    }
  }

  public void SelectRightAmmoType(int btnIndex) {
    if (btnIndex <= availablePlayerWeapons[rightCurrentSelectedWeaponIndex].availableProjectiles.Length - 1) {
      rightCurrentSelectedProjectile = availablePlayerWeapons[rightCurrentSelectedWeaponIndex].availableProjectiles[btnIndex];
      rightCurrentSelectedProjectileIndex = btnIndex;
      rightSelectedProjByWeap[rightCurrentSelectedWeaponIndex] = btnIndex;
      Debug.Log(rightCurrentSelectedProjectileIndex);
      Button clickedBtn = rightProjectilesBtn[btnIndex];
      clickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 1f);
      SwitchProjectileInfos("right", btnIndex);
      UpdateProjectilesStatus("right");
      for (int i = 0; i < rightProjectilesBtn.Length; i++) {
        if (i != btnIndex) {
          Button notClickedBtn = rightProjectilesBtn[i];
          notClickedBtn.transform.Find("SelectedProjectileSprite").GetComponent<Image>().color = new Color(0f, 0.6f, 0.03f, 0f);
        }
      }
    }
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

  public void SwitchWeaponName(string side) {
    if (side == "left") {
      leftTxtWeaponName.text = availablePlayerWeapons[leftCurrentSelectedWeaponIndex].shopName;
    }
    if (side == "right") {
      rightTxtWeaponName.text = availablePlayerWeapons[rightCurrentSelectedWeaponIndex].shopName;
    }
  }

  public void SwitchProjectileName(string side) {
    if (side == "left") {
      leftTxtProjectileName.text = availablePlayerWeapons[leftCurrentSelectedWeaponIndex].availableProjectiles[leftCurrentSelectedProjectileIndex].shopName;
    }
    if (side == "right") {
      rightTxtProjectileName.text = availablePlayerWeapons[rightCurrentSelectedWeaponIndex].availableProjectiles[rightCurrentSelectedProjectileIndex].shopName;
    }
  }

  public void UpdateProjectilesStatus(string side) {
    Text[] txtProjectileStatus = null;
    Text txtProjectileCost = null;
    int currentSelectedWeaponIndex = 0, currentSelectedProjectileIndex = 0, currentEquippedProjectileIndex = 0;

    if (side == "left") {
      txtProjectileStatus = leftTxtProjectileStatus;
      txtProjectileCost = leftTxtProjectilePrice;
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      currentSelectedProjectileIndex = leftCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = leftCurrentEquippedProjectileIndex;
    }

    if (side == "right") {
      txtProjectileStatus = rightTxtProjectileStatus;
      txtProjectileCost = rightTxtProjectilePrice;
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      currentSelectedProjectileIndex = rightCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = rightCurrentEquippedProjectileIndex;
    }

    for (int i = 0; i < txtProjectileStatus.Length; i++) {

      // Is the current Item is ... :
      bool isPurchased = playerDataManager.purchasedProjectiles[currentSelectedWeaponIndex, i];
      bool isSelectedItem = (currentSelectedProjectileIndex == i);
      bool isEquippedItem = (currentEquippedProjectileIndex == i);
      bool isDefaultItem = (i == 0);
      //Debug.Log("\n Item["+i+"] isPurchased : " + isPurchased
      //          + "\n isSelectedItem : " + isSelectedItem
      //          + "\n isEquippedItem : " + isEquippedItem
      //          + "\n isDefaultItem : " + isDefaultItem);
      if (isPurchased) {
        if (isEquippedItem) {
          txtProjectileStatus[i].text = EQUIPPED.text;
          txtProjectileStatus[i].color = EQUIPPED.color;
          if (isSelectedItem) {
            txtProjectileCost.text = EQUIPPED.text;
          }
        } else if (isDefaultItem) {
          txtProjectileStatus[i].text = DEFAULT.text;
          txtProjectileStatus[i].color = DEFAULT.color;
          if (isSelectedItem) {
            txtProjectileCost.text = DEFAULT.text;
          }
        } else {
          txtProjectileStatus[i].text = UNLOCKED.text;
          txtProjectileStatus[i].color = UNLOCKED.color;
          if (isSelectedItem) {
            txtProjectileCost.text = UNLOCKED.text;
          }
        }
      } else {
        txtProjectileStatus[i].text = LOCKED.text;
        txtProjectileStatus[i].color = LOCKED.color;
      }
    }
  }

  /// <summary>
  /// Buy or eqquip the selected projectile
  /// </summary>
  /// <param name="side"></param>
  public void BuySelectedProjectile(string side) {
    Text[] txtProjectileStatus = null;
    Text txtProjectileCost = null;
    Text txtBtnBuyProj = null;
    int currentSelectedWeaponIndex = 0, currentSelectedProjectileIndex = 0;

    if (side == "left") {
      txtProjectileStatus = leftTxtProjectileStatus;
      txtProjectileCost = leftTxtProjectilePrice;
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      currentSelectedProjectileIndex = leftCurrentSelectedProjectileIndex;
      txtBtnBuyProj = leftTextBtnBuyProj;
    }

    if (side == "right") {
      txtProjectileStatus = rightTxtProjectileStatus;
      txtProjectileCost = rightTxtProjectilePrice;
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      currentSelectedProjectileIndex = rightCurrentSelectedProjectileIndex;
      txtBtnBuyProj = rightTextBtnBuyProj;
    }

    // buying the projectile ===== 
    int projectilePrice = availablePlayerWeapons[currentSelectedWeaponIndex].availableProjectiles[currentSelectedProjectileIndex].shopPrice;

    bool isAlreadyPurchasedItem = playerDataManager.purchasedProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex];
    if (!isAlreadyPurchasedItem) {
      if ((playerDataManager.localPlayerCredits - projectilePrice) >= 0) {
        playerDataManager.purchasedProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex] = true;
        playerDataManager.SubstractCredit(projectilePrice);
        UpdatePlayerCreditText();
        txtProjectileCost.text = UNLOCKED.text;
      } else {
        creditEffect.GetComponent<Animator>().SetTrigger("execute");
        txtProjectileCost.text = "Not enough Credits";
      }
    }


    // Equip the projectile (if already purchased)  ======
    if (isAlreadyPurchasedItem) {
      if (side == "left") {
        leftCurrentEquippedProjectileIndex = leftCurrentSelectedProjectileIndex;
      }
      if (side == "right") {
        rightCurrentEquippedProjectileIndex = rightCurrentSelectedProjectileIndex;
      }

      txtProjectileStatus[currentSelectedProjectileIndex].text = EQUIPPED.text;
      txtProjectileStatus[currentSelectedProjectileIndex].color = EQUIPPED.color;
      txtProjectileCost.text = EQUIPPED.text;
    }


    UpdateProjectilesStatus(side);
  }

  public void UpdatePlayerCreditText() {
    playerCreditsText.text = playerDataManager.localPlayerCredits.ToString();
  }

  public void AcceptChanges() {
    if (playerDataManager) {
      Hashtable data = new Hashtable();

      // left 
      data["LeftWeapon"] = leftCurrentSelectedWeaponIndex;
      if (!playerDataManager.IsWeaponPurchased(leftCurrentSelectedWeaponIndex)) {
        playerDataManager.purchasedWeapons.Add(leftCurrentSelectedWeaponIndex);
      }

      data["LeftProjectiles"] = leftCurrentSelectedProjectileIndex;
      if (!playerDataManager.IsProjectilePurchased(leftCurrentSelectedProjectileIndex)) {
        playerDataManager.purchasedProjectiles[leftCurrentSelectedWeaponIndex, leftCurrentSelectedProjectileIndex] = true;
      }

      // right
      data["RightWeapon"] = rightCurrentSelectedWeaponIndex;
      if (!playerDataManager.IsWeaponPurchased(rightCurrentSelectedWeaponIndex)) {
        playerDataManager.purchasedWeapons.Add(rightCurrentSelectedWeaponIndex);
      }

      data["RightProjectiles"] = rightCurrentSelectedProjectileIndex;
      if (!playerDataManager.IsProjectilePurchased(rightCurrentSelectedProjectileIndex)) {
        playerDataManager.purchasedProjectiles[leftCurrentSelectedWeaponIndex, rightCurrentSelectedProjectileIndex] = true;
      }

      //Debug.Log("leftCurrentSelectedProjectileIndex : " + leftCurrentSelectedProjectileIndex);
      //Debug.Log("rightCurrentSelectedProjectileIndex : " + rightCurrentSelectedProjectileIndex);
      playerDataManager.SetPlayerData(data);
      FindObjectOfType<GameManager>().StartHost();
    }
  }

}
