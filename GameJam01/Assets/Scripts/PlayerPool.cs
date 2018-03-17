using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPool : MonoBehaviour {

	private List<Character> remainingPlayers;
	private bool gameOver = false;
		
	public void Update() {
		if (remainingPlayers != null && remainingPlayers.Count == 0) {
			gameOver = true;
		}
	}

	public PlayerPool addPlayer(Character newPlayer) {
		if (remainingPlayers == null) {
			remainingPlayers = new List<Character>();
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
