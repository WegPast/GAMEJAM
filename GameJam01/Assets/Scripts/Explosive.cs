using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour {

	// Use this for initialization

	void Start () {
	}

  public void DestroyMe() {
    Destroy(gameObject);
  }
	

}

