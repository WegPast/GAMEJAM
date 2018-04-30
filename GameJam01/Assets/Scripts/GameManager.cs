using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
  public Text textAddress;
  public GameObject theLocalPlayer;

  private static int nbEnnemiesKilled;
  private bool isGameStarted = false;
  private GameObject stageManager;
  private enum GameStatus { startMenu, gameStarted, gameLost };
  private GameStatus currentGameState;
  private LevelManager levelManager;
  private NetworkManager netManager;
  private GameObject waveCounter;

  void OnEnable() {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  // Use this for initialization
  void Start() {
    levelManager = GetComponent<LevelManager>();
    netManager = FindObjectOfType<NetworkManager>();
    textAddress.text = PlayerPrefs.GetString("ConnectionIP", "localhost");
    DontDestroyOnLoad(gameObject);
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    if (SceneManager.GetActiveScene().name == "00A StartMenu") CurrentGameState = GameStatus.startMenu;

    if (SceneManager.GetActiveScene().name == "02A Lost") {
      CurrentGameState = GameStatus.gameLost;
      Destroy(GameObject.Find("Main Camera"));
      string textScore = "You killed " + NbEnnemiesKilled.ToString() + " ennemies !";
      GameObject.Find("ScoreText").GetComponent<Text>().text = textScore;
    }

    if (SceneManager.GetActiveScene().name == "01B Game") {
      CurrentGameState = GameStatus.gameStarted;
      waveCounter = GameObject.Find("WaveCounter");
    }
  }

  // Update is called once per frame
  void Update() {
    if (GetPlayers().Count <= 0 && IsGameStarted) {
      GameLost();
    }
    if (CurrentGameState == GameStatus.gameStarted) {
      if (!stageManager) {
        stageManager = GameObject.Find("StageManager");
      } else {
        waveCounter.GetComponent<Text>().text = "Stage #" + stageManager.GetComponent<StageManager>().stageIndex.ToString() + " | Wave #" + stageManager.GetComponent<StageManager>().waveIndex.ToString();
      }
    }
  }

  public void GameLost() {
    IsGameStarted = false;
    if (netManager && netManager.IsClientConnected()) {
      netManager.StopHost();
    }
    levelManager.ChangeScene("02A Lost");
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
  private GameStatus CurrentGameState { get; set; }
  public bool IsGameStarted { get; set; }
  public static int NbEnnemiesKilled { get; set; }
}
