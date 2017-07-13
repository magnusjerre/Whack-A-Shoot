using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

	public float GameTime;
	private float timeLeftInGame;

	public bool IsGamePlaying = false;

	private GameState gameState;

	void Start () {
		gameState = GameObject.FindObjectOfType<GameState> ();
		StartGame ();
	}
	
	void Update () {
		if (IsGamePlaying) {
			timeLeftInGame -= Time.deltaTime;
			int intCast = (int) Mathf.Ceil(timeLeftInGame);
			if (intCast != gameState.TimeLeft) {
				gameState.CmdSetTimeLeft(intCast);
			}
			if (timeLeftInGame <= 0f) {
				gameState.CmdSetGameState (GameStateEnum.GAME_OVER);
				IsGamePlaying = false;
			}
		}
	}

	public void StartGame() {
		timeLeftInGame = GameTime;
		gameState.CmdSetTimeLeft((int) GameTime);
		gameState.CmdSetGameState (GameStateEnum.PLAYING);
		IsGamePlaying = true;
	}
}
