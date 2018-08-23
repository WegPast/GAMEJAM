using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script intend to keep LOCAL player data across scenes
 * 
 **/
public class PlayerDataManager : MonoBehaviour
{
  private PlayerControl localPlayerControl;
  private GameObject localPlayerGameObject;
  private LifeManager localPlayerLifeManager;
  private bool playerDataManagerReady = false;

  public bool isLocalPlayerInitialized = false;
  public GameManager gameManager;

  // player data =============================
  [Header("Local Player Weapons and Projectiles")]
  public int localPlayerLeftWeaponIndex = 0;
  public int localPlayerLeftProjectilesIndex = 0;
  public int localPlayerRightWeaponIndex = 0;
  public int localPlayerRightProjectilesIndex = 0;

  [Header("Local Player Bonus (Kept by this Player Data Manager)")]
  public float localPlayerLifeMaxBonus;
  public int localPlayerSpeedBonus;
  public int localPlayerCredits;
  // end player data -------------------------

  [Header("Weapons"), Header("--- Available weapons ---"), Space(20f)]
  public Weapon[] availableWeapons;
  public List<int> purchasedLeftWeapons;
  public List<int> purchasedRightWeapons;
  public bool[,] purchasedLeftProjectiles; // [purchasedWeaponIndex,purchasedProjectileIndex]
  public bool[,] purchasedRightProjectiles; // [purchasedWeaponIndex,purchasedProjectileIndex]

  public bool hasBeenInit = false;

  private void Start() {
    purchasedLeftProjectiles = new bool[availableWeapons.Length, 5];
    for (int i = 0; i < availableWeapons.Length; i++) {
      purchasedLeftProjectiles[i, 0] = true;
      purchasedLeftProjectiles[i, 1] = false;
      purchasedLeftProjectiles[i, 2] = false;
      purchasedLeftProjectiles[i, 3] = false;
      purchasedLeftProjectiles[i, 4] = false;
    }

    purchasedRightProjectiles = new bool[availableWeapons.Length, 5];
    for (int i = 0; i < availableWeapons.Length; i++) {
      purchasedRightProjectiles[i, 0] = true;
      purchasedRightProjectiles[i, 1] = false;
      purchasedRightProjectiles[i, 2] = false;
      purchasedRightProjectiles[i, 3] = false;
      purchasedRightProjectiles[i, 4] = false;
    }

    purchasedLeftWeapons.Add(0);
    purchasedRightWeapons.Add(0);

    //gameManager = FindObjectOfType<GameManager>();
    if (gameManager.playerDataManager) {
      if (gameManager.playerDataManager != this) {
        Destroy(gameObject);
      } else {
        DontDestroyOnLoad(gameObject);
        playerDataManagerReady = true;
        //Debug.Log("player data manager ready");
      }
    }
  }

  // on application Start.
  public void InitPlayerDataManager() {
    localPlayerControl = gameManager.theLocalPlayer.GetComponent<PlayerControl>();
    localPlayerGameObject = localPlayerControl.gameObject;
    localPlayerLifeManager = localPlayerGameObject.GetComponent<LifeManager>();
    //SavePlayerData();
    hasBeenInit = true;
    //Debug.Log("Init,  WeaponLeft : " + localPlayerControl.WeaponLeft);
    //Debug.Log("Init,  WeaponRight : " + localPlayerControl.WeaponRight);
  }

  public void Update() {
    if (gameManager.CurrentGameState == GameManager.GameStatus.gameStarted
        && gameManager.theLocalPlayer
        && !isLocalPlayerInitialized
        && playerDataManagerReady) {
      InitPlayerDataManager();
      isLocalPlayerInitialized = true;
    }

  }

  /// <summary>
  /// Will save the player data from de Local Player if it's still alive
  /// </summary>
  public void SavePlayerData() {
    localPlayerLeftWeaponIndex = GetWeaponIndexByShopName(localPlayerControl.WeaponLeft.shopName);
    localPlayerLeftProjectilesIndex = GetProjectileIndexByShopName(localPlayerControl.WeaponLeft.projectileType.shopName, availableWeapons[localPlayerLeftWeaponIndex]);
    localPlayerRightWeaponIndex = GetWeaponIndexByShopName(localPlayerControl.WeaponRight.shopName);
    localPlayerRightProjectilesIndex = GetProjectileIndexByShopName(localPlayerControl.WeaponRight.projectileType.shopName, availableWeapons[localPlayerRightWeaponIndex]);
    //Debug.Log("Saving : lw " + localPlayerLeftWeaponIndex + " | lp " + localPlayerLeftProjectilesIndex + " | rw " + localPlayerRightWeaponIndex + " | rp " + localPlayerRightProjectilesIndex);
  }

  public void SetPlayerData(Hashtable data) {
    //Debug.Log("SetPlayerDAta : lw " + data["LeftWeapon"] + " | lp " + data["LeftProjectiles"] + " | rw " + data["RightWeapon"] + " | rp " + data["RightProjectiles"]);
    localPlayerLeftWeaponIndex = (int)data["LeftWeapon"];
    localPlayerLeftProjectilesIndex = (int)data["LeftProjectiles"];
    localPlayerRightWeaponIndex = (int)data["RightWeapon"];
    localPlayerRightProjectilesIndex = (int)data["RightProjectiles"];
  }

  /**
   * Apply the bonus to the player stat
   * 
   **/
  public void ApplyLifeMaxBonus(float bonus) {
    localPlayerLifeManager.lifeMax += bonus;
    localPlayerLifeManager.Heal(bonus);
  }

  public void ApplySpeedBonus(int bonus) {
    localPlayerControl.speed += bonus;
  }



  // ================= ARMORY ===============================

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

  public bool IsWeaponPurchased(int index) {
    //Debug.Log("weapon " + index + " is purchased :" + (purchasedLeftWeapons.IndexOf(index) >= 0));
    return purchasedLeftWeapons.IndexOf(index) >= 0;
  }

  public bool IsProjectilePurchased(int index) {
    for (int weapIndex = 0; weapIndex < availableWeapons.Length; weapIndex++) {
      for (int projectIndex = 0; projectIndex < 6; projectIndex++) {
        if (projectIndex == index) {
          //Debug.Log("projectile " + index + " for weapon "+ weapIndex+" is purchased :" + purchasedLeftProjectiles[weapIndex, projectIndex]);
          return purchasedLeftProjectiles[weapIndex, projectIndex];
        }
      }
    }
    //Debug.Log("projectile " + index + " is purchased :" + false);
    return false;
  }


  public void AddCredit(int amount) {
    localPlayerCredits += amount;
  }

  public void SubstractCredit(int amount) {
    if(localPlayerCredits - amount < 0) {
      localPlayerCredits = 0;
    } else {
      localPlayerCredits -= amount;
    }
  }
}
