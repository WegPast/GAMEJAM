using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    private int difficultyLvl = 0;
    private LevelManager levelManager;
    private NetworkManager netManager;

    public int maxDifficultyLvl = 3;
    public bool isGameStarted = false;
    public Text textAddress;

    // Use this for initialization
    void Start() {
        levelManager = GetComponent<LevelManager>();
        netManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update() {
        if (CountPlayer() <= 0 && isGameStarted) {
            GameLost();
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

        PlayerControl[] allPlayerControl = GameObject.FindObjectsOfType<PlayerControl>();
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
        string ip = textAddress.text;
        if (netManager) {
            netManager.networkAddress = ip;
            netManager.StartClient();
        }
    }
}
