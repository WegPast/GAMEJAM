using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

  private enum GameStatus { startMenu, gameStarted, gameLost };
  private GameStatus currentGameState;

  private int difficultyLvl = 0;
  private LevelManager levelManager;
  private NetworkManager netManager;
  private GameObject myPlayer;
  private GameObject waveCounter;
  private WaveHandler waveHandler;


  private GameObject waveManager;

  public int maxDifficultyLvl = 3;
  public bool isGameStarted = false;
  public Text textAddress;
  public static int nbEnnemiesKilled;

  void OnEnable() {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  // Use this for initialization
  void Start() {
    levelManager = GetComponent<LevelManager>();
    netManager = FindObjectOfType<NetworkManager>();

    textAddress.text = PlayerPrefs.GetString("ConnectionIP","localhost");
  
    DontDestroyOnLoad(gameObject);
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    if (SceneManager.GetActiveScene().name == "00 StartMenu") {
      SetCurrentGameState(GameStatus.startMenu);
    }

    if (SceneManager.GetActiveScene().name == "02 Lost") {
      SetCurrentGameState(GameStatus.gameLost);
      GameObject.Find("ScoreText").GetComponent<Text>().text = "You killed " + nbEnnemiesKilled.ToString() + " ennemies !";
    }

    if (SceneManager.GetActiveScene().name == "01 Game") {
      SetCurrentGameState(GameStatus.gameStarted);
      waveCounter = GameObject.Find("WaveCounter");
    }
  }

  // Update is called once per frame
  void Update() {


    if (CountPlayer() <= 0 && isGameStarted) {
      GameLost();
    }
    if (GetCurrentGameState() == GameStatus.gameStarted) {
      if (!waveManager) {
        waveManager = GameObject.Find("WavesManager");
      } else {
        waveCounter.GetComponent<Text>().text = "Wave #" + waveManager.GetComponent<WaveHandler>().waveNumber.ToString();
      }
    }
  }

  public int GetDifficultyLvl() {
    return difficultyLvl;
  }

  public void SetDifficultyLvl(int diffLvl) {
    difficultyLvl = diffLvl;
  }

  public void IncreaseDifficultyLvl() {
    if ((difficultyLvl + 1) <= maxDifficultyLvl) {
      difficultyLvl++;
    }
  }

  public void DecreaseDifficultyLvl() {
    if ((difficultyLvl - 1) >= 0) {
      difficultyLvl--;
    }
  }

  public void GameLost() {
    isGameStarted = false;
    if (netManager && netManager.IsClientConnected()) {
      netManager.StopHost();
    }
    levelManager.ChangeScene("02 Lost");
  }

  public List<GameObject> GetPlayers() {
    List<GameObject> allPlayers = new List<GameObject>();

    PlayerControl[] allPlayerControl = FindObjectsOfType<PlayerControl>();
    foreach (var item in allPlayerControl) {
      allPlayers.Add(item.gameObject);
    }
    return allPlayers;
  }

  public int CountPlayer() {
    return GetPlayers().Count;
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

  private void SetCurrentGameState(GameStatus status) {
    currentGameState = status;
  }


  private GameStatus GetCurrentGameState() {
    return currentGameState;
  }


  void OnDisable() {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
}
