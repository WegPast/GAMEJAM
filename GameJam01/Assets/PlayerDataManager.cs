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
  [Header("Local Player Weapons")]
  public Weapon localPlayerLeftWeapon;
  public Weapon localPlayerRightWeapon;

  [Header("Local Player Life and Bonus")]
  public float localPlayerLifeMax;
  public float localPlayerSpeedBonus;
  // end player data -------------------------

  private void Start() {
    DontDestroyOnLoad(gameObject);

    gameManager = FindObjectOfType<GameManager>();
    localPlayerControl = gameManager.theLocalPlayer.GetComponent<PlayerControl>();
    localPlayerGameObject = localPlayerControl.gameObject;
    localPlayerLifeManager = localPlayerGameObject.GetComponent<LifeManager>();
  }

  public void SavePlayerData() {
    localPlayerLeftWeapon = localPlayerControl.WeaponLeft;
    localPlayerRightWeapon = localPlayerControl.WeaponRight;
    localPlayerLifeMax = localPlayerLifeManager.lifeMax;

  }


}
