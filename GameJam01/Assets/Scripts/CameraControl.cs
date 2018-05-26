using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControl : NetworkBehaviour
{

  public GameObject player;
  public GameManager gameManager;

  // Use this for initialization
  void Start()
  {
    DontDestroyOnLoad(gameObject);
  }

  // Update is called once per frame
  void Update()
  {
    if(gameManager.currentGameState == GameManager.GameStatus.gameShop && gameObject.activeSelf) {
      gameObject.SetActive(false);
    }
    if ((gameManager.currentGameState == GameManager.GameStatus.gameStarted || gameManager.currentGameState == GameManager.GameStatus.startMenu) && !gameObject.activeSelf) {
      gameObject.SetActive(true);
    }


    if (gameManager.theLocalPlayer)
    {
      player = gameManager.theLocalPlayer;
      Vector3 playerPos = player.gameObject.transform.position;
      transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
    else
    {
      transform.position = new Vector3(0f, 0f, transform.position.z);
    }
  }
}
