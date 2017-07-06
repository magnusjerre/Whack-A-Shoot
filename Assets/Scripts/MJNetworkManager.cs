using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MJNetworkManager : NetworkManager {

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

}
