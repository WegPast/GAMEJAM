using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInnerBody : MonoBehaviour {

  public Projectiles parentProjectile;

	// Use this for initialization
	void Start () {
		
	}

  private void OnTriggerEnter2D(Collider2D collision) {
    parentProjectile.Impact(collision);
  }


  // Update is called once per frame
  void Update () {
		
	}
}
