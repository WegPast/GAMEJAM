using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Spawner : NetworkBehaviour
{
  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  void Spawn(GameObject toSpawn)
  {
    GameObject spawnedEntity = Instantiate(toSpawn, gameObject.transform.position, Quaternion.identity);
    spawnedEntity.transform.parent = transform;
    NetworkServer.Spawn(spawnedEntity);
  }
}
