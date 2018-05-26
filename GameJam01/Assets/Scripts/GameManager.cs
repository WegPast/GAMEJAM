﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
  public Text textAddress;
  public GameObject theLocalPlayer;
  public enum GameStatus { startMenu, gameStarted, gameShop, gameLost };
  public GameStatus currentGameState;

  private static int nbEnnemiesKilled;
  private bool isGameStarted = false;
  private LevelManager levelManager;
  private NetworkManager netManager;
  public PlayerDataManager playerDataManager;

  void OnEnable() {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  // Use this for initialization
  void Start() {
    levelManager = GetComponent<LevelManager>();
    netManager = FindObjectOfType<NetworkManager>();
    playerDataManager = FindObjectOfType<PlayerDataManager>();
    textAddress.text = PlayerPrefs.GetString("ConnectionIP", "localhost");
    DontDestroyOnLoad(gameObject);
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    if (SceneManager.GetActiveScene().name == "00A StartMenu") {
      CurrentGameState = GameStatus.startMenu;
    }

    if (SceneManager.GetActiveScene().name == "02A Lost") {
      CurrentGameState = GameStatus.gameLost;
      Destroy(GameObject.Find("Main Camera"));
      string textScore = "You killed " + NbEnnemiesKilled.ToString() + " ennemies !";
      GameObject.Find("ScoreText").GetComponent<Text>().text = textScore;
    }

    if (SceneManager.GetActiveScene().name == "01B Game") {
      CurrentGameState = GameStatus.gameStarted;
    }

    if (SceneManager.GetActiveScene().name == "01C Shop") {
      CurrentGameState = GameStatus.gameShop;
    }
  }

  // Update is called once per frame
  void Update() {
    if (GetPlayers().Count <= 0 && IsGameStarted) {
      GameLost();
    }

    // if game is started
    if (CurrentGameState == GameStatus.gameStarted) {

      //// initialize the local player (to be done once)
      //if (theLocalPlayer && !playerDataManager.isLocalPlayerInitialized) {
      //  playerDataManager.InitPlayerDataManager();
      //}


    }
  }

  public void GameLost() {
    IsGameStarted = false;
    if (netManager && netManager.IsClientConnected()) {
      netManager.StopHost();
    }
    //levelManager.ChangeScene("02A Lost");
    levelManager.ChangeScene("01C Shop");
  }

  public List<GameObject> GetPlayers() {
    List<GameObject> allPlayers = new List<GameObject>();
    PlayerControl[] allPlayerControl = FindObjectsOfType<PlayerControl>();
    foreach (var item in allPlayerControl) {
      allPlayers.Add(item.gameObject);
    }
    return allPlayers;
  }

  public void StartHost() {
    if (netManager) {
      netManager.StartHost();
    }
  }

  public void ConnectTo() {

    string ip;
    if (PlayerPrefs.GetString("ConnectionIP") == "") {
      PlayerPrefs.SetString("ConnectionIP", textAddress.text);
      ip = textAddress.text;
    } else {
      ip = PlayerPrefs.GetString("ConnectionIP");
    }

    if (netManager) {
      netManager.networkAddress = ip;
      netManager.StartClient();
    }
  }

  void OnDisable() {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  public int DifficultyLvl { get; set; }
  public GameStatus CurrentGameState { get; set; }
  public bool IsGameStarted { get; set; }
  public static int NbEnnemiesKilled { get; set; }
}
