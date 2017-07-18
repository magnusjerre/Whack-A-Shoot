using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
    private List<PlayerScore> playerScores;
	public List<PlayerScore> PlayerScores { get { return playerScores; } }

    private List<TargetScoreCanvas> scoreCanvases;

    public TargetScoreCanvas scoreCanvasPrefab;

    public NetworkInstanceId playerId;
    public bool IsVrPlayer { get; set; }

    private Text playerTotalScoreText;
	private GameState gameState;

    void Start()
    {
        playerScores = new List<PlayerScore>();
        scoreCanvases = new List<TargetScoreCanvas>();
        for (int i = 0; i < 10; i++)
        {
            var canvas = Instantiate(scoreCanvasPrefab);
            canvas.transform.SetParent(transform, false);
            canvas.IsAvailable = true;
            canvas.Hide();
            scoreCanvases.Add(canvas);
        }

        playerTotalScoreText = GameObject.FindGameObjectWithTag("PlayerTotalScoreText").GetComponent<Text>();
        playerTotalScoreText.text = "0";
		gameState = GameObject.FindObjectOfType<GameState> ();
    }

    [Command]
    public void CmdNotifyTargetLifetimeExpired(NetworkInstanceId ownerId, NetworkInstanceId targetId, int targetMaxPoints)
    {
		if (gameState.IsGameOver()) {
			return;
		}

        var playerScore = GetPlayerScore(ownerId);
        playerScore.Increase(targetMaxPoints);
        RpcUpdatePlayerTotalScore(ownerId, playerScore.TotalScore);
        RpcShowScoreForPlayer(ownerId, targetId, targetMaxPoints, targetMaxPoints);
    }

    [Command]
    public void CmdNotifyTargetDestroyed(TargetHitInfo hitInfo, NetworkInstanceId shooterId)
    {
		if (gameState.IsGameOver()) {
			return;
		}

        int shooterPoints = (int)Mathf.Clamp((int)(hitInfo.maxPoints * (1 - hitInfo.elapsedTime / hitInfo.lifetime) * hitInfo.precision), 1, hitInfo.maxPoints);
        int ownerPoints = hitInfo.maxPoints - shooterPoints;

        var shooterScore = GetPlayerScore(shooterId);
        shooterScore.Increase(shooterPoints);
        RpcUpdatePlayerTotalScore(shooterId, shooterScore.TotalScore);

        var ownerScore = GetPlayerScore(hitInfo.ownerId);
        ownerScore.Increase(ownerPoints);
        RpcUpdatePlayerTotalScore(hitInfo.ownerId, ownerScore.TotalScore);

        RpcShowScoreForPlayer(shooterId, hitInfo.targetId, shooterPoints, hitInfo.maxPoints);
        RpcShowScoreForPlayer(hitInfo.ownerId, hitInfo.targetId, ownerPoints, hitInfo.maxPoints);
    }

	[Command]
	public void CmdIncreaseScoreBy(int amount, NetworkInstanceId playerId) {
		if (gameState.IsGameOver()) {
			return;
		}

		var playerScore = GetPlayerScore (playerId);
		playerScore.Increase (amount);
		RpcUpdatePlayerTotalScore (playerId, playerScore.TotalScore);
		RpcShowOnlyScoreForPlayer(playerId, amount, new Vector3(0, 1, 0));
	}

    private PlayerScore GetPlayerScore(NetworkInstanceId playerId)
    {
        foreach (PlayerScore playerScore in playerScores)
        {
            if (playerScore.PlayerId.Equals(playerId))
            {
                return playerScore;
            }
        }

        var newPlayerScore = new PlayerScore(playerId);
        playerScores.Add(newPlayerScore);
        return newPlayerScore;
    }


    [ClientRpc]
    public void RpcShowScoreForPlayer(NetworkInstanceId playerId, NetworkInstanceId targetId, int score, int maxScore)
    {
        Debug.Log("ScoreManager.RpcShowScoreForPlayer: scoreManager.playerId: " + this.playerId.ToString() + ", inputPlayerId: " + playerId.ToString());
        if (this.playerId.Equals(playerId))
        {
            var scoreCanvas = GetScoreCanvas();
            scoreCanvas.ShowScore(targetId, score, maxScore, IsVrPlayer);
        }
    }

    private TargetScoreCanvas GetScoreCanvas()
    {
        foreach (var canvas in scoreCanvases)
        {
            if (canvas.IsAvailable)
            {
                return canvas;
            }
        }

        var newCanvas = Instantiate(scoreCanvasPrefab);
        newCanvas.transform.SetParent(transform, false);
        scoreCanvases.Add(newCanvas);
        return newCanvas;
    }

    [ClientRpc]
    public void RpcUpdatePlayerTotalScore(NetworkInstanceId playerId, int newScore)
    {
		GetPlayerScore (playerId).SetTotalScore (newScore);
		Debug.Log ("RpcUpdatePlayerScore, set the score for " + playerId + " to " + newScore);
		if (this.playerId.Equals(playerId))
        {
            playerTotalScoreText.text = newScore.ToString();
        }
    }

	[ClientRpc]
	public void RpcShowOnlyScoreForPlayer(NetworkInstanceId playerId, int score, Vector3 position) {
		if (this.playerId.Equals(playerId))
		{
			var scoreCanvas = GetScoreCanvas ();
			scoreCanvas.Show (score.ToString (), position, IsVrPlayer);
		}
	}
}
