using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour {

  public int explosionSize = 0;

  private Animator animator;

  public void Explode() {
    animator = gameObject.GetComponent<Animator>();
    switch (explosionSize) {
      case 1:
        animator.SetTrigger("explodeSmall");
        break;
      case 2:
        animator.SetTrigger("explodeMed");
        break;
      case 3:
        animator.SetTrigger("explodeBig");
        break;
    }
  }

  public void DestroyMe() {
    Destroy(gameObject);
  }


}

