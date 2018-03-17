﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPool : Pool<PlayerControl> {
	protected override void Execute() {
		if (pool != null && pool.Count == 0) {
			SceneManager.LoadScene ("00 StartMenu");
		}
	}
}