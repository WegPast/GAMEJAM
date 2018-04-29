using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageManager : NetworkBehaviour
{
  [SyncVar] int syncWaveIndex;
  [SyncVar] int syncStageIndex;
  public int waveIndex;
  public int stageIndex;

  private enum State
  {
    initialize = 0,
    wave,
    stage,
  };

  private int gameState = (int)State.initialize;

  [Header("multiplicateur de difficulté")]
  public float difficulty;

  [Header("Nombre max de vague du niveau")]
  public int nbWaveMaxForStage;

  [Header("Nombre d'ennemis à la première vague du stage")]
  public int nbEnemyStartType1;
  public int nbEnemyStartType2;
  public int nbEnemyStartType3;

  //Ratio
  [Header("Ratio d'augmentation d'ennemi en fonction de la vague")]
  public float ratioWaveEnemyType1;
  public float ratioWaveEnemyType2;
  public float ratioWaveEnemyType3;

  [Header("Ratio d'augmentation d'ennemi en fonction du niveau")]
  public float ratioStageEnemyType1;
  public float ratioStageEnemyType2;
  public float ratioStageEnemyType3;

  [Header("Ratio d'augmentation du nombre de vague en fonction du niveau")]
  public float ratioWave;

  [Header("Ratio d'augmentation de la difficulté en fonction du niveau")]
  public float difficulyRatio;

  public int nbEnemyType1;
  public int nbEnemyType2;
  public int nbEnemyType3;


  // Use this for initialization
  void Start()
  {
    nbEnemyType1 = nbEnemyStartType1;
    nbEnemyType2 = nbEnemyStartType2;
    nbEnemyType3 = nbEnemyStartType3;
  }

  // Update is called once per frame
  void Update()
  {
    if (isServer)
    {
      switch(this.gameState)
      {
        case (int)State.initialize:
          gameObject.GetComponent<WaveHandler>().InitializeWave();
          this.gameState = (int)State.wave;
          break;
        case (int)State.wave:
          if(gameObject.GetComponent<WaveHandler>().WaveState())
          {

            this.NextWave();
            gameObject.GetComponent<WaveHandler>().InitializeWave();
            if(waveIndex>= nbWaveMaxForStage) // nombre de vagues atteint
            {
              this.gameState = (int)State.stage;
            }   
          }
        break;
        case (int)State.stage:
          if (gameObject.GetComponent<WaveHandler>().WaveState())
          {
            this.NextStage();
            this.gameState = (int)State.wave;
          }
        break;
      }
      SendInfoStage();
    }
    else
    {
      //Synchronisation niveau avec serveur
      waveIndex = syncWaveIndex;
      stageIndex = syncStageIndex;
    }
  }

  //Increment Stage difficulty
  private void IncrementStageDifficulty()
  {
    //Augmentation ennemi à la première vague
    nbEnemyStartType1 += (int)(stageIndex * ratioStageEnemyType1);
    nbEnemyStartType2 += (int)(stageIndex * ratioStageEnemyType2);
    nbEnemyStartType3 += (int)(stageIndex * ratioStageEnemyType3);

    //Augmentation du ratio d'ennemis par vague en fonction du niveau
    ratioWaveEnemyType1 += (stageIndex * ratioStageEnemyType1);
    ratioWaveEnemyType2 += (stageIndex * ratioStageEnemyType2);
    ratioWaveEnemyType3 += (stageIndex * ratioStageEnemyType3);

    //Augmentation ennemi à la première vague
    difficulty += stageIndex * difficulyRatio;

    //Augmentation nombre de vague par stage
    nbWaveMaxForStage += (int)(stageIndex * ratioWave);

  }

  //Increment Wave difficulty
  private void IncrementWaveDifficulty()
  {
    nbEnemyType1 = nbEnemyStartType1 + (int)(waveIndex * ratioWaveEnemyType1);
    nbEnemyType2 = nbEnemyStartType2 + (int)(waveIndex * ratioWaveEnemyType2);
    nbEnemyType3 = nbEnemyStartType3 + (int)(waveIndex * ratioWaveEnemyType3);
  }
  //Reset Wave difficulty
  private void ResetWaveDifficulty()
  {
    nbEnemyType1 = nbEnemyStartType1;
    nbEnemyType2 = nbEnemyStartType2;
    nbEnemyType3 = nbEnemyStartType3;
  }


  //Fonction de passage au niveau suivant
  public void NextStage()
  {
    stageIndex++;
    this.IncrementStageDifficulty();
    this.ResetWaveDifficulty();
  }


  //Fonction de passage au niveau suivant
  public void NextWave()
  {
    waveIndex++;
    this.IncrementWaveDifficulty();

  }


  //Network function

  //Client
  [Client]
  void SendInfoStage()
  {
    CmdSendInfoStageToServer(this.waveIndex, this.stageIndex);
  }


  //Command
  [Command]
  void CmdSendInfoStageToServer(int waveIndex, int stageIndex)
  {
    this.syncWaveIndex = waveIndex;
    this.syncStageIndex = stageIndex;
  }

}

