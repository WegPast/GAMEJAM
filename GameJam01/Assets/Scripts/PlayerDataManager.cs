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

  private void Start() {
    DontDestroyOnLoad(gameObject);

    gameManager = FindObjectOfType<GameManager>();
  }

  public void InitPlayerDataManager() {
    if (gameManager.CurrentGameState == GameManager.GameStatus.gameStarted) {
      localPlayerControl = gameManager.theLocalPlayer.GetComponent<PlayerControl>();
      localPlayerGameObject = localPlayerControl.gameObject;
      localPlayerLifeManager = localPlayerGameObject.GetComponent<LifeManager>();
      SavePlayerData();
    }
  }

  public void SavePlayerData() {
    localPlayerLeftWeapon = localPlayerControl.WeaponLeft;
    localPlayerLeftProjectiles = localPlayerControl.WeaponLeft.projectileType;
    localPlayerRightWeapon = localPlayerControl.WeaponRight;
    localPlayerRightProjectiles = localPlayerControl.WeaponRight.projectileType;
  }

  public void LoadPlayerData() {

    ApplyLifeMaxBonus(localPlayerLifeMaxBonus);
    ApplySpeedBonus(localPlayerSpeedBonus);

    localPlayerControl.WeaponLeft = localPlayerLeftWeapon;
    localPlayerControl.WeaponLeft.projectileType = localPlayerLeftProjectiles;
    localPlayerControl.WeaponRight = localPlayerRightWeapon;
    localPlayerControl.WeaponRight.projectileType = localPlayerRightProjectiles;

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
