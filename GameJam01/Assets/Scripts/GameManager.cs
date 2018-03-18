using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int difficultyLvl = 0;
    private LevelManager levelManager;
    private NetworkManager netManger;

    public int maxDifficultyLvl = 3;
    public bool isGameStarted = false;

    // Use this for initialization
    void Start() {
        levelManager = GetComponent<LevelManager>();
        netManger = GameObject.FindObjectOfType<NetworkManager>();
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
        netManger.StopHost();
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
        if (netManger) {
            netManger.StartHost();
        }
    }
}
