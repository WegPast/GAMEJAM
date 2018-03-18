using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int difficultyLvl = 0;

    public int maxDifficultyLvl = 3;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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
}
