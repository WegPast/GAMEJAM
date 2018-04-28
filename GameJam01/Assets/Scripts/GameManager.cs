using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
  public int maxDifficultyLvl = 3;
  public Text textAddress;


  private static int nbEnnemiesKilled;
  private bool isGameStarted = false;
  private GameObject waveManager;
  private enum GameStatus { startMenu, gameStarted, gameLost };
  private GameStatus currentGameState;
  private int difficultyLvl = 0;
  private LevelManager levelManager;
  private NetworkManager netManager;
  private GameObject myPlayer;
  private GameObject waveCounter;
  private WaveHandler waveHandler;

  void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  // Use this for initialization
  void Start()
  {
    levelManager = GetComponent<LevelManager>();
    netManager = FindObjectOfType<NetworkManager>();
    textAddress.text = PlayerPrefs.GetString("ConnectionIP", "localhost");
    DontDestroyOnLoad(gameObject);
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (SceneManager.GetActiveScene().name == "00 StartMenu")  CurrentGameState = GameStatus.startMenu;

    if (SceneManager.GetActiveScene().name == "02 Lost")
    {
      CurrentGameState = GameStatus.gameLost;
      GameObject.Find("ScoreText").GetComponent<Text>().text = "You killed " + NbEnnemiesKilled.ToString() + " ennemies !";
    }

    if (SceneManager.GetActiveScene().name == "01 Game")
    {
      CurrentGameState = GameStatus.gameStarted;
      waveCounter = GameObject.Find("WaveCounter");
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (GetPlayers().Count <= 0 && IsGameStarted)
    {
      GameLost();
    }
    if (CurrentGameState == GameStatus.gameStarted)
    {
      if (!waveManager)
      {
        waveManager = GameObject.Find("WavesManager");
      }
      else
      {
        waveCounter.GetComponent<Text>().text = "Wave #" + waveManager.GetComponent<WaveHandler>().waveNumber.ToString();
      }
    }
  }

  public void IncreaseDifficultyLvl()
  {
    if ((DifficultyLvl + 1) <= maxDifficultyLvl) DifficultyLvl++;
  }

  public void GameLost()
  {
    IsGameStarted = false;
    if (netManager && netManager.IsClientConnected())
    {
      netManager.StopHost();
    }
    levelManager.ChangeScene("02 Lost");
  }

  public List<GameObject> GetPlayers()
  {
    List<GameObject> allPlayers = new List<GameObject>();
    PlayerControl[] allPlayerControl = FindObjectsOfType<PlayerControl>();
    foreach (var item in allPlayerControl)
    {
      allPlayers.Add(item.gameObject);
    }
    return allPlayers;
  }

  public void StartHost()
  {
    if (netManager)
    {
      netManager.StartHost();
    }
  }

  public void ConnectTo()
  {

    string ip;
    if (PlayerPrefs.GetString("ConnectionIP") == "")
    {
      PlayerPrefs.SetString("ConnectionIP", textAddress.text);
      ip = textAddress.text;
    }
    else
    {
      ip = PlayerPrefs.GetString("ConnectionIP");
    }

    if (netManager)
    {
      netManager.networkAddress = ip;
      netManager.StartClient();
    }
  }

  void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  public int DifficultyLvl { get; set; }
  private GameStatus CurrentGameState { get; set; }
  public bool IsGameStarted { get; set; }
  public static int NbEnnemiesKilled {get; set;}
}
