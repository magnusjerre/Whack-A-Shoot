using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour
{
    private List<PlayerScore> playerScores;
    private List<ScoreCanvas> scoreCanvases;

    public ScoreCanvas scoreCanvasPrefab;

    public NetworkInstanceId playerId;
    public bool IsVrPlayer { get; set; }

    void Start()
    {
        playerScores = new List<PlayerScore>();
        scoreCanvases = new List<ScoreCanvas>();
        for (int i = 0; i < 10; i++)
        {
            var canvas = Instantiate(scoreCanvasPrefab);
            canvas.transform.SetParent(transform, false);
            canvas.IsAvailable = true;
            canvas.Hide();
            scoreCanvases.Add(canvas);
        }
    }

    [Command]
    public void CmdNotifyTargetLifetimeExpired(NetworkInstanceId ownerId, NetworkInstanceId targetId, int targetMaxPoints)
    {
        GetPlayerScore(ownerId).Increase(targetMaxPoints);
        RpcShowScoreForPlayer(ownerId, targetId, targetMaxPoints, targetMaxPoints);
    }

    [Command]
    public void CmdNotifyTargetDestroyed(TargetHitInfo hitInfo, NetworkInstanceId shooterId)
    {
        int shooterPoints = (int)Mathf.Clamp((int)(hitInfo.maxPoints * (1 - hitInfo.elapsedTime / hitInfo.lifetime) * hitInfo.precision), 0, hitInfo.maxPoints);
        int ownerPoints = hitInfo.maxPoints - shooterPoints;

        GetPlayerScore(shooterId).Increase(shooterPoints);
        GetPlayerScore(hitInfo.ownerId).Increase(ownerPoints);

        RpcShowScoreForPlayer(shooterId, hitInfo.targetId, shooterPoints, hitInfo.maxPoints);
        RpcShowScoreForPlayer(hitInfo.ownerId, hitInfo.targetId, ownerPoints, hitInfo.maxPoints);
    }

    private PlayerScore GetPlayerScore(NetworkInstanceId playerId)
    {
        foreach (PlayerScore playerScore in playerScores)
        {
            if (playerScore.Equals(playerId))
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

    private ScoreCanvas GetScoreCanvas()
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
}
