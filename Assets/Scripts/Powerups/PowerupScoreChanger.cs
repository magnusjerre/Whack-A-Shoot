using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PowerupScoreChanger : IPowerup
{

	public int scoreDelta = 10;

	#region IPowerup implementation

	public void Use (GameObject user, GameObject[] allPlayers)
	{
		var playerNVR = user.GetComponent<PlayerNVR> ();
		var playerNetId = user.GetComponent<NetworkInstanceId> ();

		throw new System.NotImplementedException ();
	}

	#endregion


}

