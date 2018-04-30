using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameHudController : NetworkBehaviour
{

  public GameObject gunLeftInfo;
  public GameObject gunRightInfo;

  // Use this for initialization
  void Start() {
    //gunLeftSprite = transform.Find("GunLeftInfo")
    //  .GetComponent<SpriteRenderer>();
    //gunRightSprite = transform
    //  .Find("GunRightInfo")
    //  .GetComponent<SpriteRenderer>();
    //SetWeaponIcon();
  }

  // Update is called once per frame
  void Update() {


  }

  public void SetWeaponIcon(Weapon instantiatedWeapon) {
    gunLeftInfo
      .GetComponent<Image>().sprite = instantiatedWeapon
      .projectileType
      .GetComponent<Projectiles>()
      .iconSprite;

    //gunRightInfo.GetComponent<Image>().sprite = localPlayer.WeaponRight.projectileType.GetComponent<Projectiles>().iconSprite;

  }

}
