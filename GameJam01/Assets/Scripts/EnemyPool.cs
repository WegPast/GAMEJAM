using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPool : Pool<Ennemy> {
	protected override void Execute() {
		if (pool != null && pool.Count == 0) {
			SceneManager.LoadScene ("00 StartMenu");
		}
	}
}
