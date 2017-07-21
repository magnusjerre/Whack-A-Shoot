using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameState : NetworkBehaviour {

	[SyncVar(hook = "SyncTimeLeft")]
	private int timeLeft;
    public int TimeLeft { get { return timeLeft; } }

	public bool IsGamePlaying = false;

    [SyncVar(hook = "SyncGameState")]
    private GameStateEnum state;
    public GameStateEnum State { get { return state; } }

	private Text timeLeftText;
	private GameOverCanvas gameOverCanvas;

	void Start () {
		timeLeftText = GameObject.FindGameObjectWithTag ("TimeLeftText").GetComponent<Text> ();
		var gameOverGameObject = GameObject.FindGameObjectWithTag ("GameOverCanvas");
		if (gameOverGameObject != null)
        {
			gameOverCanvas = gameOverGameObject.GetComponent<GameOverCanvas> ();
            gameOverCanvas.Hide();
        }
	}

    [Command]
    public void CmdSetTimeLeft(int newTime) {
        timeLeft = newTime;
		timeLeftText.text = newTime.ToString ();
    }
	
	private void SyncTimeLeft(int newTimeLeft) {
		timeLeft = newTimeLeft;
	}

    private void SyncGameState(GameStateEnum newState) {
		if (newState == GameStateEnum.GAME_OVER) {
			var scoreManager = GameObject.FindObjectOfType<ScoreManager> ();
			if (gameOverCanvas != null)
            {
                gameOverCanvas.AddScores(scoreManager.PlayerScores);
                gameOverCanvas.Show();
            }
		}
        this.state = newState;
    }

    [Command]
    public void CmdSetGameState(GameStateEnum newState) {
        this.state = newState;
    }

	public bool IsGameOver() {
		return state == GameStateEnum.GAME_OVER;
	}
}
