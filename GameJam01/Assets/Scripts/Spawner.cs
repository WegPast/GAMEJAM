using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject entityToSpawn;
	public float spawnPerSecond = 5f;
	private float nextUpdate;

    // Use this for initialization
    void Start() {
		if (entityToSpawn == null) {
			Debug.LogError ("No entities to spawn for " + name);
		}
    }

    // Update is called once per frame
    void Update() {
		if (Time.time >= nextUpdate) {
			nextUpdate = Mathf.FloorToInt (Time.time) + spawnPerSecond;
			Spawn (entityToSpawn);
		}
    }

	void Spawn(GameObject toSpawn) {
		GameObject spawnedEntity = Instantiate(toSpawn, gameObject.transform.position, Quaternion.identity);
        spawnedEntity.transform.parent = transform;
    }

}
