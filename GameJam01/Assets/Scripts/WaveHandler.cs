using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaveHandler : NetworkBehaviour
{
  public enum SpawnersAction
  {
    spawn,
    next,
    count
  }

  [System.Serializable]
  public class Wave
  {
    public List<int> enemyToSpawn = new List<int>();
    public List<GameObject> spawnedEnemy = new List<GameObject>();
  }

  [Header("Stage Manager")]
  public StageManager stageManager;

  [Header("Soldats")]
  public List<GameObject> enemyType1;
  [Header("Capitaines")]
  public List<GameObject> enemyType2;
  [Header("Généraux")]
  public List<GameObject> enemyType3;

  private Spawner[] spawnPoints;
  private float nextUpdate = 1f;
  private Wave wave = new Wave();

  void Start()
  {
    if (isServer)
    {
      spawnPoints = GameObject.FindObjectsOfType<Spawner>();
    }
  }

  void Update()
  {
    if (isServer)
    {
      if (Time.time >= nextUpdate)
      {
        nextUpdate = Mathf.FloorToInt(Time.time) + 1;
        Invoke("SpawnWave", 1f);
      }
    }
  }

  public void InitializeWave()
  {
    wave.enemyToSpawn.Clear();
    wave.spawnedEnemy.Clear();

    for (int i=0;i<stageManager.nbEnemyType1;i++)
    {
      wave.enemyToSpawn.Add(1);
    }
    for (int i = 0; i < stageManager.nbEnemyType2; i++)
    {
      wave.enemyToSpawn.Add(2);
    }
    for (int i = 0; i < stageManager.nbEnemyType3; i++)
    {
      wave.enemyToSpawn.Add(3);
    }
  }

  public bool WaveState()
  {
    for(int i=0;i< wave.spawnedEnemy.Count;i++)
    {
      if(wave.spawnedEnemy[i]==null)
      {
        wave.spawnedEnemy.RemoveAt(i);
      }
    }
    if (wave.spawnedEnemy.Count > 0 || wave.enemyToSpawn.Count > 0) { return false; }
    else { return true;  }
  }

  public void SpawnWave()
  {
    if (wave.enemyToSpawn.Count > 0)
    {
      GameObject o = null;
      switch (wave.enemyToSpawn[0])
      {
        case 1:
          o = Instantiate(enemyType1[0], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
          o.transform.parent = this.transform;
          break;
        case 2:
          o = Instantiate(enemyType2[0], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
          o.transform.parent = this.transform;
          break;
        case 3:
          o = Instantiate(enemyType3[0], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
          o.transform.parent = this.transform;
          break;
      }
      NetworkServer.Spawn(o);
      wave.spawnedEnemy.Add(o);
      wave.enemyToSpawn.RemoveAt(0);
      Invoke("SpawnWave", 1f);
    }
  }
}
