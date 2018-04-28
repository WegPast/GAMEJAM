using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControl : NetworkBehaviour
{

  public GameObject player;

  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (player)
    {
      Vector3 playerPos = player.gameObject.transform.position;
      transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
    else
    {
      transform.position = new Vector3(0f, 0f, transform.position.z);
    }
  }
}
