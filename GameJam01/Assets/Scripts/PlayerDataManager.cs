using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script intend to keep LOCAL player data across scenes
 * 
 **/
public class PlayerDataManager : MonoBehaviour
{
  private GameManager gameManager;
  private PlayerControl localPlayerControl;
  private GameObject localPlayerGameObject;
  private LifeManager localPlayerLifeManager;

  // player data =============================
  [Header("Local Player Weapons and Projectiles")]
  public Weapon localPlayerLeftWeapon;
  public Projectiles localPlayerLeftProjectiles;
  public Weapon localPlayerRightWeapon;
  public Projectiles localPlayerRightProjectiles;

  [Header("Local Player Bonus (Kept by this Player Data Manager)")]
  public int localPlayerLifeMaxBonus;
  public int localPlayerSpeedBonus;
  // end player data -------------------------


  public bool hasBeenInit = false;

  private void Start() {

    gameManager = FindObjectOfType<GameManager>();
    if (gameManager.playerDataManager) {
      if (gameManager.playerDataManager != this) {
        Destroy(gameObject);
      } else {
        DontDestroyOnLoad(gameObject);
      }
    }
  }

  public void InitPlayerDataManager() {
    localPlayerControl = gameManager.theLocalPlayer.GetComponent<PlayerControl>();
    localPlayerGameObject = localPlayerControl.gameObject;
    localPlayerLifeManager = localPlayerGameObject.GetComponent<LifeManager>();
    SavePlayerData();
    hasBeenInit = true;
    Debug.Log("localPlayerControl.WeaponLeft : " + localPlayerControl.WeaponLeft);
  }

  public void Update() {
    if (gameManager.CurrentGameState == GameManager.GameStatus.gameStarted && gameManager.theLocalPlayer) {
      InitPlayerDataManager();
    }
  }

  /// <summary>
  /// Will save the player data from de Local Player if it's still alive
  /// </summary>
  public void SavePlayerData() {
    localPlayerLeftWeapon = localPlayerControl.WeaponLeft;
    localPlayerLeftProjectiles = localPlayerControl.WeaponLeft.projectileType;
    localPlayerRightWeapon = localPlayerControl.WeaponRight;
    localPlayerRightProjectiles = localPlayerControl.WeaponRight.projectileType;
  }

  public void SetPlayerData(Hashtable data) {
    localPlayerLeftWeapon = (Weapon)data["LeftWeapon"];
    localPlayerLeftProjectiles = (Projectiles)data["LeftProjectiles"];
    localPlayerRightWeapon = (Weapon)data["RightWeapon"];
    localPlayerRightProjectiles = (Projectiles)data["RightProjectiles"];
  }

  /**
   * Apply the bonus to the player stat
   * 
   **/
  public void ApplyLifeMaxBonus(int bonus) {
    localPlayerLifeManager.lifeMax += bonus;
    localPlayerLifeManager.Heal(bonus);
  }

  public void ApplySpeedBonus(int bonus) {
    localPlayerControl.speed += bonus;
  }


}
