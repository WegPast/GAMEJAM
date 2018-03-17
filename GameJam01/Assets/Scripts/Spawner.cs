using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] entitiesToSpawn;

    private int cpt;


    // Use this for initialization
    void Start() {
        if (entitiesToSpawn.Length == 0) {
            Debug.LogError("No entities to spawn for " + name);
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (GameObject attacker in entitiesToSpawn) {
            if (IsTimeToSpawn(attacker)) {
                Spawn(attacker);
            }
        }
    }

    public void Spawn(GameObject entity) {
        GameObject spawnedEntity = Instantiate(entity, gameObject.transform.position, Quaternion.identity);
        spawnedEntity.transform.parent = transform;
    }

    bool IsTimeToSpawn(GameObject attackerGameObject) {
		Ennemy attacker = attackerGameObject.GetComponent<Ennemy>();

        float meanSpawnDelay = 1;
        float spawnsPerSecond = 1 / meanSpawnDelay;

        if (Time.deltaTime > meanSpawnDelay) {
            Debug.LogWarning("Spawn rate capped by frame rate");
        }

        float threshold = spawnsPerSecond * Time.deltaTime / 5;

        return (Random.value < threshold);

    }
}
