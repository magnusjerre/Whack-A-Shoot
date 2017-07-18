using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public interface IPowerup 
{
	void Use (GameObject user, GameObject[] allPlayers);
}

