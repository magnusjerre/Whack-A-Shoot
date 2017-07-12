using System;
using UnityEngine.Networking;

public class PlayerScore : IComparable<PlayerScore>
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

    public int CompareTo(PlayerScore other) {
		return TotalScore - other.TotalScore; //More points means a better position
    }

	public void SetTotalScore(int totalScore) {
		this.totalScore = totalScore;
	}

}
