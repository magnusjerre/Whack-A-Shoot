using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MJNetworkManager : NetworkManager {

    public GameObject playerVRPrefab, playerNVRPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnClientDisconnect(NetworkConnection conn)
    {
        MainMenuScript canvas = GameObject.FindObjectOfType<MainMenuScript>();
        if (canvas != null)
        {
            canvas.OnClientDisconnect();
        }
        base.OnClientDisconnect(conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        MainMenuScript canvas = GameObject.FindObjectOfType<MainMenuScript>();
        if (canvas != null)
        {
            canvas.OnClientError();
        }
        base.OnClientError(conn, errorCode);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        string hostAddress = "localClient";
        GameObject player;
        if (!conn.address.Equals(hostAddress)) {
            player = GameObject.Instantiate(playerNVRPrefab);
        } else {
            player = GameObject.Instantiate(playerNVRPrefab);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

}
