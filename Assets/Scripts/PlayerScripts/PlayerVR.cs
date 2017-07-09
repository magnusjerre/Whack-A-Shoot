using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerVR : NetworkBehaviour {

	private ShotRendererPool shotRendererPool;
	private PlayerHand playerHand;

	private LayerMask shootingMask;

	private ParticleSystemPool particlePool;

	private ScoreManager scoreManager;

	void Start () {
		shotRendererPool = GameObject.FindObjectOfType<ShotRendererPool>();

		playerHand = GameObject.FindObjectOfType<PlayerHand>();
		shootingMask = LayerMask.NameToLayer("Shootable");
		particlePool = GameObject.FindObjectOfType<ParticleSystemPool>();
	}

	public override void OnStartLocalPlayer()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.Log("Score doesn't exist at this time");
            return;
        }

        scoreManager.playerId = GetComponent<NetworkIdentity>().netId;
		scoreManager.IsVrPlayer = true;
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
				TargetHitInfo targetHitInfo = hitTarget.CreateHitInfo(offset);
                RpcShowHitFire(start, hitTarget.GetNetId(), offset);
				hitTarget.RpcHideTargetForDestruction();
				hitTarget.CmdRegisterHitDestroyAfterTime();

				scoreManager.CmdNotifyTargetDestroyed(
					targetHitInfo,
					GetComponent<NetworkIdentity>().netId
				);
            }
			else 
			{
				RpcShowFire(start, end);
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
		shotRendererPool.GetShotRenderer().Show(start, end);
	}

	[ClientRpc]
	public void RpcShowHitFire(Vector3 start, NetworkInstanceId netId, Vector3 centerOffset) {
		Target target = GetTargetById(netId);
		shotRendererPool.GetShotRenderer().Show(start, target.Center.position + centerOffset);
		ParticleSystem ps = particlePool.GetParticleSystem();
		if (ps != null) {
			ps.transform.position = target.Center.position + centerOffset;
			ps.Play();
		}
	}

	private Target GetTargetById(NetworkInstanceId netId) {
		Target[] allTargets = GameObject.FindObjectsOfType<Target>();
		foreach (Target target in allTargets) {			
			if (target.GetNetId().Equals(netId)) {
				return target;
			}
		}
		return null;
	}
}
