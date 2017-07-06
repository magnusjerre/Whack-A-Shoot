using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (LineRenderer))]
public class PlayerVR : NetworkBehaviour {

	private LineRenderer lineRenderer;
	private PlayerHand playerHand;

	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;

		// GameObject.FindObjectOfType<PlayerHand>().SetPlayerVR(this);
		playerHand = GameObject.FindObjectOfType<PlayerHand>();
	}

	void Update() {
		if (!hasAuthority) {
			return;
		}

		if (playerHand.ReleasedTrigger()) {
			CmdFire(playerHand.Muzzle().position, playerHand.Muzzle().forward * 10);
		}
	}

	[Command]
	public void CmdFire(Vector3 start, Vector3 end) {
		//Do some checks for hit info
		RpcShowFire(start, end);
	}

	[ClientRpc]
	public void RpcShowFire(Vector3 start, Vector3 end) {
		lineRenderer.SetPositions(new Vector3[]{
			start, end
		});
		lineRenderer.enabled = true;
	}
}
