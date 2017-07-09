using UnityEngine.Networking;

public class PlayerScore
{
    private NetworkInstanceId playerId;
    private int totalScore;

    public PlayerScore(NetworkInstanceId playerId)
    {
        this.playerId = playerId;
    }

    public void Increase(int amount)
    {
        totalScore += amount;
    }

}
