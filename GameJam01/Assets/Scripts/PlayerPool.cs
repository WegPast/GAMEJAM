using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPool : MonoBehaviour {

	private List<PlayerControl> remainingPlayers;
	private bool gameOver = false;
		
	public void Update() {
		if (remainingPlayers != null && remainingPlayers.Count == 0) {
			gameOver = true;
		}
	}

	public PlayerPool addPlayer(PlayerControl newPlayer) {
		if (remainingPlayers == null) {
			remainingPlayers = new List<PlayerControl>();
		}
		remainingPlayers.Add (newPlayer);
		return this;
	}

	public bool GameOver {
		get {
			return this.gameOver;
		}
	}
	
}
