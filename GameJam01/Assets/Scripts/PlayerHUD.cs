using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHUD : NetworkBehaviour
{
  public GameObject gunLeftInfo;
  public GameObject gunRightInfo;
  public GameObject lifeBarBckgd;

  private float lifeBarMaxWidth;
  private float lifeBarMaxHeight;
  private float playerPercentLife; // must be between 0.0f and 1.0f
  private PlayerControl localPlayer;

  public bool hasBeenInit = false;

  // Use this for initialization
  void Start() {
    //gunLeftInfo = GameObject.Find("GunLeftInfo").gameObject;
    //gunRightInfo = GameObject.Find("GunRightInfo").gameObject;
    //lifeBarBckgd = GameObject.Find("LifeBar_bckgd").gameObject;

  }

  public void Init() {
    localPlayer = FindObjectOfType<GameManager>().theLocalPlayer.GetComponent<PlayerControl>();
    lifeBarMaxWidth = lifeBarBckgd.GetComponent<RectTransform>().rect.width;
    lifeBarMaxHeight = lifeBarBckgd.GetComponent<RectTransform>().rect.height;
    hasBeenInit = true;
  }

  // Update is called once per frame
  void Update() {
    if (isLocalPlayer) {
      SetWeaponIcon(localPlayer.WeaponLeft, gunLeftInfo);
      SetWeaponIcon(localPlayer.WeaponRight, gunRightInfo);
      playerPercentLife = localPlayer.gameObject.GetComponent<LifeManager>().lifePercent;
      UpdateLifeBarSize();
    }
    if (FindObjectOfType<GameManager>().theLocalPlayer != null && !hasBeenInit) {
      Init();
    }
  }

  public void UpdateLifeBarSize() {
    lifeBarBckgd.GetComponent<RectTransform>().sizeDelta = new Vector2(playerPercentLife * lifeBarMaxWidth / 100f, lifeBarMaxHeight);
  }

  public void SetWeaponIcon(Weapon instantiatedWeapon, GameObject gunInfo) {
    gunInfo.GetComponent<Image>().sprite = instantiatedWeapon.projectileType.GetComponent<Projectiles>().iconSprite;
  }

}
