using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRenderer2D : MonoBehaviour {

  public Vector2 offest = new Vector2(0f, -0.2f);
  public Material shadowMaterial;
  public Color shadowColor;

  private SpriteRenderer spriteRndCaster;
  private SpriteRenderer spriteRndShadow;

  private Transform transCaster;
  private Transform transShadow;

  private void Start() {
    transCaster = transform;
    transShadow = new GameObject("shadow").transform;
    transShadow.parent = transCaster;
    transShadow.localRotation = Quaternion.identity;

    spriteRndCaster = GetComponent<SpriteRenderer>();
    spriteRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

    spriteRndShadow.sortingLayerName = spriteRndCaster.sortingLayerName;
    spriteRndShadow.sortingOrder = spriteRndCaster.sortingOrder - 1;

    spriteRndShadow.material = shadowMaterial;
    spriteRndShadow.color = shadowColor;

  }

  private void LateUpdate() {
    transShadow.position = new Vector2(transCaster.position.x + offest.x, transCaster.position.y + offest.y);
    spriteRndShadow.sprite = spriteRndCaster.sprite;
  }

}
