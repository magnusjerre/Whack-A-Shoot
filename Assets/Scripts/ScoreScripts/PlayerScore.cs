using UnityEngine.Networking;

public class PlayerScore
{
    private NetworkInstanceId playerId;
    public NetworkInstanceId PlayerId { get { return playerId; } }
    private int totalScore;
    public int TotalScore { get { return totalScore; } }

    public PlayerScore(NetworkInstanceId playerId)
    {
        this.playerId = playerId;
    }

    public void Increase(int amount)
    {
        totalScore += amount;
    }

}
