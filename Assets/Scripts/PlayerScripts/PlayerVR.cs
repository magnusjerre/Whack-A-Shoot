using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (LineRenderer))]
public class PlayerVR : NetworkBehaviour {

	private LineRenderer lineRenderer;
	private PlayerHand playerHand;

	private LayerMask shootingMask;

	private Target[] allTargets;

	private ParticleSystemPool particlePool;

	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;

		// GameObject.FindObjectOfType<PlayerHand>().SetPlayerVR(this);
		playerHand = GameObject.FindObjectOfType<PlayerHand>();
		shootingMask = LayerMask.NameToLayer("Shootable");
		allTargets = GameObject.FindObjectsOfType<Target>();
		particlePool = GameObject.FindObjectOfType<ParticleSystemPool>();
	}

	void Update() {
		if (!hasAuthority) {
			return;
		}
		Debug.DrawRay(playerHand.Muzzle().position, playerHand.Muzzle().position + playerHand.Muzzle().forward * 10);
		if (playerHand.ReleasedTrigger()) {
			CmdFire(playerHand.Muzzle().position, playerHand.Muzzle().position + playerHand.Muzzle().forward * 10);
		}
	}

	[Command]
    public void CmdFire(Vector3 start, Vector3 end)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(start, end - start, out hitInfo, 10f, ~shootingMask.value))
        {
			Debug.Log("Raycasthit, woho");
            Target hitTarget = hitInfo.collider.GetComponentInParent<Target>();
            if (hitTarget != null)
            {
                Vector3 offset = hitInfo.point - hitTarget.Center.position;
                RpcShowHitFire(start, hitTarget.GetNetId(), offset);
				hitTarget.CmdSetIsUp(false);
            }
        }
        else
        {
            RpcShowFire(start, end);
        }

		Debug.Log("hitInfo.collider: " + hitInfo.collider);
    }

	[ClientRpc]
	public void RpcShowFire(Vector3 start, Vector3 end) {
		lineRenderer.SetPositions(new Vector3[]{
			start, end
		});
		lineRenderer.enabled = true;
		Debug.Log("RpcShowFire");
	}

	[ClientRpc]
	public void RpcShowHitFire(Vector3 start, NetworkInstanceId netId, Vector3 centerOffset) {
		Debug.Log("RpcShowHitFire");
		Target target = GetTargetById(netId);
		lineRenderer.SetPositions(new Vector3[]{
			start, target.Center.position + centerOffset
		});
		lineRenderer.enabled = true;
		ParticleSystem ps = particlePool.GetParticleSystem();
		if (ps != null) {
			ps.transform.position = target.Center.position + centerOffset;
			ps.Play();
		}
	}

	private Target GetTargetById(NetworkInstanceId netId) {
		foreach (Target target in allTargets) {			
			if (target.GetNetId().Equals(netId)) {
				return target;
			}
		}
		return null;
	}
}
