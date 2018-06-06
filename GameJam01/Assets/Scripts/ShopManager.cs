using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ShopManager : MonoBehaviour {

  public GameObject weaponMenu;
  public GameObject skillsMenu;
  public Text playerCreditsText;
  public PlayerDataManager playerDataManager;
  public Image creditEffect;
  public Button acceptBtn;

  [Header("Left side Weapon")]
  public Image leftImgWeaponSprite;
  public Text leftTxtWeaponName;
  public Text leftTxtWeaponPrice;
  public Text leftTxtWeaponDesc;
  public Text leftTxtWeaponSpec;
  public Text leftTxtWeaponStatus;
  public Image leftImgHCWeaponSprite;
  private Text leftTextBtnBuyWeap;
  public Button leftBtnBuyWeap;

  [Header("Left side Projectile")]
  public Text leftTxtProjectileName;
  public Text leftTxtProjectilePrice;
  public Text leftTxtProjectileDesc;
  public Text leftTxtProjectileSpec;
  public Text[] leftTxtProjectileStatus = new Text[5];
  public Image[] leftProjectilesSprites;
  public Button[] leftProjectilesBtn;
  public Text leftTextBtnBuyProj;
  public GameObject leftMaskProj;
  public Button[] leftSelectionBtnsProjectile = null;
  public Button leftBtnBuyProj;

  [Header("Right side Weapon"), Space(20f)]
  public Image rightImgWeaponSprite;
  public Text rightTxtWeaponName;
  public Text rightTxtWeaponPrice;
  public Text rightTxtWeaponDesc;
  public Text rightTxtWeaponSpec;
  public Text rightTxtWeaponStatus;
  public Image rightImgHCWeaponSprite;
  private Text rightTextBtnBuyWeap;
  public Button rightBtnBuyWeap;

  [Header("Right side Projectile")]
  public Text rightTxtProjectileName;
  public Text rightTxtProjectilePrice;
  public Text rightTxtProjectileDesc;
  public Text rightTxtProjectileSpec;
  public Text[] rightTxtProjectileStatus = new Text[5];
  public Image[] rightProjectilesSprites;
  public Button[] rightProjectilesBtn;
  public Text rightTextBtnBuyProj;
  public GameObject rightMaskProj;
  public Button[] rightSelectionBtnsProjectile = null;
  public Button rightBtnBuyProj;

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

  // Currently equipped weapons
  private int leftCurrentEquippedWeaponIndex = 0;
  private int rightCurrentEquippedWeaponIndex = 0;

  // Currently selected projectiles
  private Projectiles leftCurrentSelectedProjectile;
  private int leftCurrentSelectedProjectileIndex = 0; // index in availabeProjectile of a corresponding weapon
  private int leftCurrentSelectedProjectilePrice = 0;
  private Projectiles rightCurrentSelectedProjectile;
  private int rightCurrentSelectedProjectileIndex = 0; // index in availabeProjectile of a corresponding weapon
  private int rightCurrentSelectedProjectilePrice = 0;

  // Currently equipped projectiles
  private int leftCurrentEquippedProjectileIndex = 0;
  private int rightCurrentEquippedProjectileIndex = 0;

  // index will be the selected weapon, the value the projectile index in the available projectile weapon'array
  private int[] leftSelectedProjByWeap;
  private int[] rightSelectedProjByWeap;

  public class ItemShopStatus {
    public Color color;
    public string text;
    public string weaponBtnText;
    public string projectileBtnText;

    public ItemShopStatus(Color color, string text, string weaponBtnText = "", string projectileBtnText = "") {
      this.color = color;
      this.text = text;
      this.weaponBtnText = weaponBtnText;
      this.projectileBtnText = projectileBtnText;
    }
  }

  private ItemShopStatus UNLOCKED = new ItemShopStatus(new Color(0f, 1f, 0f, 1f), "unlocked", "Equip this weapon", "Equip this ammo type");
  private ItemShopStatus LOCKED = new ItemShopStatus(new Color(1f, 0f, 0f, 1f), "locked", "Buy this weapon", "Buy this ammo type");
  private ItemShopStatus EQUIPPED = new ItemShopStatus(new Color(0f, 0.68f, 1f, 1f), "equipped", "Equipped weapon", "Equipped ammo type");
  private ItemShopStatus DEFAULT = new ItemShopStatus(Color.white, "default", "Default weapon", "Default ammo type");

  private Color COLOR_DISABLED_BTN = new Color(0.4f, 0.4f, 0.4f, 1f);
  private Color COLOR_DISABLED_TEXT = new Color(0.5f, 0.5f, 0.5f, 1f);
  private Color COLOR_ENABLED_TEXT = new Color(1f, 0.7f, 0f, 1f);

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
    ChangeAcceptBtnStatus();
  }

  public void ChangeAcceptBtnStatus() {

    if (!CheckReadyToAccept()) {
      acceptBtn.interactable = false;
      acceptBtn.GetComponent<Image>().color = COLOR_DISABLED_BTN;
      acceptBtn.GetComponentInChildren<Text>().color = COLOR_DISABLED_TEXT;
    } else {
      acceptBtn.interactable = true;
      acceptBtn.GetComponent<Image>().color = Color.white;
      acceptBtn.GetComponentInChildren<Text>().color = COLOR_ENABLED_TEXT;
    }
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
      leftTextBtnBuyWeap = leftBtnBuyWeap.GetComponentInChildren<Text>();
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
      rightTextBtnBuyWeap = rightBtnBuyWeap.GetComponentInChildren<Text>();
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
      UpdateWeaponStatus(side);
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
      UpdateWeaponStatus(side);
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
      UpdateWeaponStatus(side);
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
      UpdateWeaponStatus(side);
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

      if (index == playerDataManager.localPlayerLeftWeaponIndex) {
        leftCurrentSelectedProjectileIndex = playerDataManager.localPlayerLeftProjectilesIndex;
        leftCurrentEquippedProjectileIndex = playerDataManager.localPlayerLeftProjectilesIndex;
        SelectLeftAmmoType(playerDataManager.localPlayerLeftProjectilesIndex);
      } else {
        //leftCurrentSelectedProjectileIndex = 0;
        //leftCurrentEquippedProjectileIndex = 0;
        //SelectLeftAmmoType(0);
      }

      SwitchWeaponName(side);
      UpdateProjectilesStatus(side);
      UpdateWeaponStatus(side);
    }
    if (side == "right") {
      rightTxtWeaponDesc.text = description;
      rightTxtWeaponSpec.text = "FR : " + firerateTxt;
      rightTxtWeaponPrice.text = priceTxt + " cr";
      rightCurrentSelectedWeaponPrice = selectedWeapon.shopPrice;
      if (index == playerDataManager.localPlayerRightWeaponIndex) {
        rightCurrentSelectedProjectileIndex = playerDataManager.localPlayerRightProjectilesIndex;
        rightCurrentEquippedProjectileIndex = playerDataManager.localPlayerRightProjectilesIndex;
        SelectRightAmmoType(playerDataManager.localPlayerRightProjectilesIndex);
      } else {
        rightCurrentSelectedProjectileIndex = 0;
        rightCurrentEquippedProjectileIndex = 0;
        SelectRightAmmoType(0);
      }
      SwitchWeaponName(side);
      UpdateProjectilesStatus(side);
      UpdateWeaponStatus(side);
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

  public void UpdateWeaponStatus(string side) {
    Text txtWeaponCost = null, txtWeaponStatus = null;
    Button btnWeapBuy = null;
    GameObject maskProj = null;
    bool isPurchased = false;
    int currentSelectedWeaponIndex = 0, currentSelectedProjectileIndex = 0, currentEquippedProjectileIndex = 0, currentEquippedWeaponIndex = 0;

    if (side == "left") {
      txtWeaponCost = leftTxtWeaponPrice;
      txtWeaponStatus = leftTxtWeaponStatus;
      maskProj = leftMaskProj;
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      currentEquippedWeaponIndex = leftCurrentEquippedWeaponIndex;
      currentSelectedProjectileIndex = leftCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = leftCurrentEquippedProjectileIndex;
      btnWeapBuy = leftBtnBuyWeap;
      isPurchased = System.Array.IndexOf<int>(playerDataManager.purchasedLeftWeapons.ToArray(), currentSelectedWeaponIndex) >= 0;
    }

    if (side == "right") {
      txtWeaponCost = rightTxtWeaponPrice;
      txtWeaponStatus = rightTxtWeaponStatus;
      maskProj = rightMaskProj;
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      currentEquippedWeaponIndex = rightCurrentEquippedWeaponIndex;
      currentSelectedProjectileIndex = rightCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = rightCurrentEquippedProjectileIndex;
      btnWeapBuy = rightBtnBuyWeap;
      isPurchased = System.Array.IndexOf<int>(playerDataManager.purchasedRightWeapons.ToArray(), currentSelectedWeaponIndex) >= 0;
    }


    // Is the current Item is ... :
    bool isEquippedItem = (currentEquippedWeaponIndex == currentSelectedWeaponIndex);
    bool isDefaultItem = (currentSelectedWeaponIndex == 0);
    if (isPurchased) {

      txtWeaponStatus.text = UNLOCKED.text;
      txtWeaponStatus.color = UNLOCKED.color;
      txtWeaponCost.text = UNLOCKED.text;

      btnWeapBuy.interactable = true;
      btnWeapBuy.GetComponent<Image>().color = Color.white;
      btnWeapBuy.GetComponentInChildren<Text>().color = COLOR_ENABLED_TEXT;
      if (isEquippedItem) {
        // automatically equip the selected weapon if it is purchased
        txtWeaponStatus.text = EQUIPPED.text;
        txtWeaponStatus.color = EQUIPPED.color;
        txtWeaponCost.text = EQUIPPED.text;
        btnWeapBuy.interactable = false;
        btnWeapBuy.GetComponent<Image>().color = COLOR_DISABLED_BTN;
        btnWeapBuy.GetComponentInChildren<Text>().color = COLOR_DISABLED_TEXT;
      }
      maskProj.SetActive(false);

    } else {
      txtWeaponStatus.text = LOCKED.text;
      txtWeaponStatus.color = LOCKED.color;
      maskProj.SetActive(true);

      btnWeapBuy.interactable = true;
      btnWeapBuy.GetComponent<Image>().color = Color.white;
      btnWeapBuy.GetComponentInChildren<Text>().color = COLOR_ENABLED_TEXT;
    }

    UpdateBuyWeaponBtnText(side);
    UpdateBuyProjectileBtnText(side);
  }


  public void UpdateProjectilesStatus(string side) {
    Text[] txtProjectileStatus = null;
    Button[] selectionBtnsProjectile = null;
    Button btnProjBuy = null;
    Text txtProjectileCost = null;
    bool isPurchased = false; ;
    int currentSelectedWeaponIndex = 0, currentSelectedProjectileIndex = 0, currentEquippedProjectileIndex = 0, currentEquippedWeaponIndex = 0;

    if (side == "left") {
      txtProjectileStatus = leftTxtProjectileStatus;
      txtProjectileCost = leftTxtProjectilePrice;
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      currentEquippedWeaponIndex = leftCurrentEquippedWeaponIndex;
      currentSelectedProjectileIndex = leftCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = leftCurrentEquippedProjectileIndex;
      selectionBtnsProjectile = leftSelectionBtnsProjectile;
      btnProjBuy = leftBtnBuyProj;
    }

    if (side == "right") {
      txtProjectileStatus = rightTxtProjectileStatus;
      txtProjectileCost = rightTxtProjectilePrice;
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      currentEquippedWeaponIndex = rightCurrentEquippedWeaponIndex;
      currentSelectedProjectileIndex = rightCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = rightCurrentEquippedProjectileIndex;
      selectionBtnsProjectile = rightSelectionBtnsProjectile;
      btnProjBuy = rightBtnBuyProj;
    }


    btnProjBuy.interactable = true;
    btnProjBuy.GetComponent<Image>().color = Color.white;
    btnProjBuy.GetComponentInChildren<Text>().color = COLOR_ENABLED_TEXT;

    for (int i = 0; i < txtProjectileStatus.Length; i++) {

      // Is the current Item is ... :
      if (i < availablePlayerWeapons[currentSelectedWeaponIndex].availableProjectiles.Length) {

        if (side == "left") {
          isPurchased = playerDataManager.purchasedLeftProjectiles[currentSelectedWeaponIndex, i];
        }

        if (side == "right") {
          isPurchased = playerDataManager.purchasedRightProjectiles[currentSelectedWeaponIndex, i];
        }

        bool isSelectedItem = (currentSelectedProjectileIndex == i);
        bool isEquippedItem = (currentEquippedProjectileIndex == i && currentSelectedWeaponIndex == currentEquippedWeaponIndex);
        bool isDefaultItem = (i == 0);
        //Debug.Log("\n Item["+i+"] isPurchased : " + isPurchased
        //          + "\n isSelectedItem : " + isSelectedItem
        //          + "\n isEquippedItem : " + isEquippedItem
        //          + "\n isDefaultItem : " + isDefaultItem);

        selectionBtnsProjectile[i].interactable = true;

        if (isPurchased) {
          if (isEquippedItem) {
            txtProjectileStatus[i].text = EQUIPPED.text;
            txtProjectileStatus[i].color = EQUIPPED.color;
            if (isSelectedItem) {
              txtProjectileCost.text = EQUIPPED.text;

              btnProjBuy.interactable = false;
              btnProjBuy.GetComponent<Image>().color = COLOR_DISABLED_BTN;
              btnProjBuy.GetComponentInChildren<Text>().color = COLOR_DISABLED_TEXT;
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
      } else {
        txtProjectileStatus[i].text = "";
        selectionBtnsProjectile[i].interactable = false;
      }
    }



    UpdateBuyProjectileBtnText(side);
  }


  /// <summary>
  /// Buy or eqquip the selected Weapon
  /// </summary>
  /// <param name="side"></param>
  public void BuySelectedWeapon(string side) {
    Text txtWeaponStatus = null;
    Text txtWeaponCost = null;
    Text txtBtnBuyWeap = null;
    bool isAlreadyPurchasedItem = false;
    int currentSelectedWeaponIndex = 0;

    if (side == "left") {
      txtWeaponStatus = leftTxtWeaponStatus;
      txtWeaponCost = leftTxtWeaponPrice;
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      txtBtnBuyWeap = leftTextBtnBuyWeap;

    isAlreadyPurchasedItem = System.Array.IndexOf<int>(playerDataManager.purchasedLeftWeapons.ToArray(), currentSelectedWeaponIndex) >= 0;
    }

    if (side == "right") {
      txtWeaponStatus = rightTxtWeaponStatus;
      txtWeaponCost = rightTxtWeaponPrice;
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      txtBtnBuyWeap = rightTextBtnBuyWeap;
      isAlreadyPurchasedItem = System.Array.IndexOf<int>(playerDataManager.purchasedRightWeapons.ToArray(), currentSelectedWeaponIndex) >= 0;
    }

    // buying the Weapon ===== 
    int weaponPrice = availablePlayerWeapons[currentSelectedWeaponIndex].shopPrice;
    if (!isAlreadyPurchasedItem) {
      if ((playerDataManager.localPlayerCredits - weaponPrice) >= 0) {
        if (side == "left") {
          playerDataManager.purchasedLeftWeapons.Add(currentSelectedWeaponIndex);
        }
        if (side == "right") {
          playerDataManager.purchasedRightWeapons.Add(currentSelectedWeaponIndex);
        }
            playerDataManager.SubstractCredit(weaponPrice);
        UpdatePlayerCreditText();
        //txtWeaponCost.text = UNLOCKED.text;
      } else {
        creditEffect.GetComponent<Animator>().SetTrigger("execute");
        txtWeaponCost.text = "Not enough Credits";
      }
    }


    // Equip the Weapon (if already purchased)  ======
    if (isAlreadyPurchasedItem) {
      if (side == "left") {
        leftCurrentEquippedWeaponIndex = leftCurrentSelectedWeaponIndex;
        playerDataManager.localPlayerLeftWeaponIndex = leftCurrentEquippedWeaponIndex;
      }
      if (side == "right") {
        rightCurrentEquippedWeaponIndex = rightCurrentSelectedWeaponIndex;
        playerDataManager.localPlayerRightWeaponIndex = rightCurrentEquippedWeaponIndex;
      }

      txtWeaponStatus.text = EQUIPPED.text;
      txtWeaponStatus.color = EQUIPPED.color;
      txtWeaponCost.text = EQUIPPED.text;
    }


    UpdateWeaponStatus(side);
    UpdateProjectilesStatus(side);
  }


  /// <summary>
  /// Buy or eqquip the selected Weapectile
  /// </summary>
  /// <param name="side"></param>
  public void BuySelectedProjectile(string side) {
    Text[] txtProjectileStatus = null;
    Text txtProjectileCost = null;
    Text txtBtnBuyProj = null;
    int currentSelectedWeaponIndex = 0, currentSelectedProjectileIndex = 0;
    bool isAlreadyPurchasedItem = false;
    if (side == "left") {
      txtProjectileStatus = leftTxtProjectileStatus;
      txtProjectileCost = leftTxtProjectilePrice;
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      currentSelectedProjectileIndex = leftCurrentSelectedProjectileIndex;
      txtBtnBuyProj = leftTextBtnBuyProj;
      isAlreadyPurchasedItem = playerDataManager.purchasedLeftProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex];
    }

    if (side == "right") {
      txtProjectileStatus = rightTxtProjectileStatus;
      txtProjectileCost = rightTxtProjectilePrice;
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      currentSelectedProjectileIndex = rightCurrentSelectedProjectileIndex;
      txtBtnBuyProj = rightTextBtnBuyProj;
      isAlreadyPurchasedItem = playerDataManager.purchasedRightProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex];
    }

    // buying the projectile ===== 
    int projectilePrice = availablePlayerWeapons[currentSelectedWeaponIndex].availableProjectiles[currentSelectedProjectileIndex].shopPrice;

    if (!isAlreadyPurchasedItem) {
      if ((playerDataManager.localPlayerCredits - projectilePrice) >= 0) {

        if (side == "left") {
          playerDataManager.purchasedLeftProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex] = true;
        }
        if (side == "right") {
          playerDataManager.purchasedRightProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex] = true;
        }
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
      // if we equip this projectile, we make sur that the selected weapon is equipped too.
      if (side == "left") {
        leftCurrentEquippedWeaponIndex = leftCurrentSelectedWeaponIndex;
        playerDataManager.localPlayerLeftWeaponIndex = leftCurrentEquippedWeaponIndex;
        leftCurrentEquippedProjectileIndex = leftCurrentSelectedProjectileIndex;
        playerDataManager.localPlayerLeftProjectilesIndex = leftCurrentEquippedProjectileIndex;
      }
      if (side == "right") {
        rightCurrentEquippedWeaponIndex = rightCurrentSelectedWeaponIndex;
        playerDataManager.localPlayerRightWeaponIndex = rightCurrentEquippedWeaponIndex;
        rightCurrentEquippedProjectileIndex = rightCurrentSelectedProjectileIndex;
        playerDataManager.localPlayerRightProjectilesIndex = rightCurrentEquippedProjectileIndex;
      }

      txtProjectileStatus[currentSelectedProjectileIndex].text = EQUIPPED.text;
      txtProjectileStatus[currentSelectedProjectileIndex].color = EQUIPPED.color;
      txtProjectileCost.text = EQUIPPED.text;
    }

    UpdateWeaponStatus(side);
    UpdateProjectilesStatus(side);
  }

  public ItemShopStatus GetCurrentSelectedItemStatus(string side, string type) {
    ItemShopStatus status = null;
    bool isPurchasedItem = false;
    bool isPurchasedWeap = false;
    int currentSelectedWeaponIndex = 0, currentEquippedWeaponIndex = 0, currentSelectedProjectileIndex = 0, currentEquippedProjectileIndex = 0;

    if (side == "left") {
      currentSelectedWeaponIndex = leftCurrentSelectedWeaponIndex;
      currentEquippedWeaponIndex = leftCurrentEquippedWeaponIndex;
      currentSelectedProjectileIndex = leftCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = leftCurrentEquippedProjectileIndex;
      isPurchasedItem = playerDataManager.purchasedLeftProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex];
      isPurchasedWeap = System.Array.IndexOf<int>(playerDataManager.purchasedLeftWeapons.ToArray(), currentSelectedWeaponIndex) >= 0;
    }

    if (side == "right") {
      currentSelectedWeaponIndex = rightCurrentSelectedWeaponIndex;
      currentEquippedWeaponIndex = rightCurrentEquippedWeaponIndex;
      currentSelectedProjectileIndex = rightCurrentSelectedProjectileIndex;
      currentEquippedProjectileIndex = rightCurrentEquippedProjectileIndex;
      isPurchasedItem = playerDataManager.purchasedRightProjectiles[currentSelectedWeaponIndex, currentSelectedProjectileIndex];
      isPurchasedWeap = System.Array.IndexOf<int>(playerDataManager.purchasedRightWeapons.ToArray(), currentSelectedWeaponIndex) >= 0;
    }

    if (type == "weapon") {
      if (isPurchasedWeap) {
        status = UNLOCKED;
        if (currentEquippedWeaponIndex == currentSelectedWeaponIndex) {
          status = EQUIPPED;
        }
      } else {
        status = LOCKED;
        if (currentSelectedWeaponIndex == 0) {
          status = DEFAULT;
        }
      }
    }

    if (type == "projectile") {
      if (isPurchasedItem) {
        status = UNLOCKED;
        // FYI : the equipped projectile is always the default of the selected weapon OR one of the selected weapon.
        if (currentEquippedProjectileIndex == currentSelectedProjectileIndex) {
          status = EQUIPPED;
        }
      } else {
        status = LOCKED;
        if (currentSelectedProjectileIndex == 0) {
          status = DEFAULT;
        }
      }
    }

    if (type != "projectile" && type != "weapon") {
      throw new System.Exception("GetCurrentSelectedItemStatus : Bad Argument 'type'");
    }

    return status;
  }

  public void UpdateBuyWeaponBtnText(string side) {
    Text textBuyWeapBtn = null;
    if (side == "left") {
      textBuyWeapBtn = leftTextBtnBuyWeap;
    }

    if (side == "right") {
      textBuyWeapBtn = rightTextBtnBuyWeap;
    }

    ItemShopStatus weaponStatus = GetCurrentSelectedItemStatus(side, "weapon");

    textBuyWeapBtn.text = weaponStatus.weaponBtnText;

  }

  public void UpdateBuyProjectileBtnText(string side) {
    Text textBuyProjBtn = null;
    if (side == "left") {
      textBuyProjBtn = leftTextBtnBuyProj;
    }
    if (side == "right") {
      textBuyProjBtn = rightTextBtnBuyProj;
    }

    ItemShopStatus projectileStatus = GetCurrentSelectedItemStatus(side, "projectile");
    textBuyProjBtn.text
      = projectileStatus.projectileBtnText;
  }


  public void UpdatePlayerCreditText() {
    playerCreditsText.text = playerDataManager.localPlayerCredits.ToString();
  }

  public bool CheckReadyToAccept() {

    bool isLeftSelectedWeaponEquipped = leftCurrentEquippedWeaponIndex == leftCurrentSelectedWeaponIndex
      && playerDataManager.localPlayerLeftWeaponIndex == leftCurrentEquippedWeaponIndex;

    bool isRightSelectedWeaponEquipped = rightCurrentEquippedWeaponIndex == rightCurrentSelectedWeaponIndex
      && playerDataManager.localPlayerRightWeaponIndex == rightCurrentEquippedWeaponIndex;

    bool areSelectedWeaponsEquipped = isLeftSelectedWeaponEquipped && isRightSelectedWeaponEquipped;

    bool isLeftProjectileEquipped = (leftCurrentEquippedProjectileIndex == leftCurrentSelectedProjectileIndex
      && playerDataManager.localPlayerLeftProjectilesIndex == leftCurrentEquippedProjectileIndex)
      && isLeftSelectedWeaponEquipped;

    bool isRightProjectileEquipped = (rightCurrentEquippedProjectileIndex == rightCurrentSelectedProjectileIndex
      && playerDataManager.localPlayerRightProjectilesIndex == rightCurrentEquippedProjectileIndex)
      && isRightSelectedWeaponEquipped;

    bool areSelectedProjectilesOfTheSelectedWeaponEquipped = isLeftProjectileEquipped && isRightProjectileEquipped;

    return areSelectedWeaponsEquipped && areSelectedProjectilesOfTheSelectedWeaponEquipped;
  }

  public void AcceptChanges() {
    if (playerDataManager) {

      Hashtable data = new Hashtable();

      // left 
      data["LeftWeapon"] = leftCurrentSelectedWeaponIndex;
      if (!playerDataManager.IsWeaponPurchased(leftCurrentSelectedWeaponIndex)) {
        playerDataManager.purchasedLeftWeapons.Add(leftCurrentSelectedWeaponIndex);
      }

      data["LeftProjectiles"] = leftCurrentSelectedProjectileIndex;
      if (!playerDataManager.IsProjectilePurchased(leftCurrentSelectedProjectileIndex)) {
        playerDataManager.purchasedLeftProjectiles[leftCurrentSelectedWeaponIndex, leftCurrentSelectedProjectileIndex] = true;
      }

      // right
      data["RightWeapon"] = rightCurrentSelectedWeaponIndex;
      if (!playerDataManager.IsWeaponPurchased(rightCurrentSelectedWeaponIndex)) {
        playerDataManager.purchasedRightWeapons.Add(rightCurrentSelectedWeaponIndex);
      }

      data["RightProjectiles"] = rightCurrentSelectedProjectileIndex;
      if (!playerDataManager.IsProjectilePurchased(rightCurrentSelectedProjectileIndex)) {
        playerDataManager.purchasedRightProjectiles[rightCurrentSelectedWeaponIndex, rightCurrentSelectedProjectileIndex] = true;
      }

      //Debug.Log("leftCurrentSelectedProjectileIndex : " + leftCurrentSelectedProjectileIndex);
      //Debug.Log("rightCurrentSelectedProjectileIndex : " + rightCurrentSelectedProjectileIndex);
      playerDataManager.SetPlayerData(data);
      FindObjectOfType<GameManager>().StartHost();
    }
  }

}
