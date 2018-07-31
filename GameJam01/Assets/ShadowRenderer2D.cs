using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRenderer2D : MonoBehaviour {

  public float yOffset = 10f;
  public Sprite shadowSprite;

  public bool castShadow = false;
  public bool receiveShadow = true;

  private SpriteRenderer spriteRenderer;

  // Use this for initialization
  void Start() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer) {

      if (castShadow) {
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
      } else {
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      }

      spriteRenderer.receiveShadows = receiveShadow;

    } else {
      Debug.LogError("No SpriteRender on " + name);
    }
  }

  // Update is called once per frame
  void Update() {

  }
}
