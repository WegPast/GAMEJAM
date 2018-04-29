using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHUD : NetworkBehaviour
{
  public GameObject gunLeftInfo;
  public GameObject gunRightInfo;

  // Use this for initialization
  void Start() {
    gunLeftInfo = GameObject.Find("GunLeftInfo").gameObject;
    gunRightInfo = GameObject.Find("GunRightInfo").gameObject;
  }

  // Update is called once per frame
  void Update() {
    if (isLocalPlayer) {
      SetWeaponIcon(GetComponent<PlayerControl>().WeaponLeft, gunLeftInfo);
      SetWeaponIcon(GetComponent<PlayerControl>().WeaponRight, gunRightInfo);
    }
  }

  public void SetWeaponIcon(Weapon instantiatedWeapon, GameObject gunInfo) {
    gunInfo.GetComponent<Image>().sprite = instantiatedWeapon.projectileType.GetComponent<Projectiles>().iconSprite;
  }

}
