using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (Rotator))]
public class Target : NetworkBehaviour {


	[SerializeField]
	private Transform center;
	public Transform Center { get { return center; } }

	private ParticleSystem ps;
	[SerializeField]
	private GameObject targetModel;

	private NetworkIdentity networkIdentity;
	public float lifetime;
	private float elapsedTime;

	void Start () {
		ps = GetComponentInChildren<ParticleSystem>();
		networkIdentity = GetComponent<NetworkIdentity>();
	}

	void Update() {
		if (!isServer) {
			return;
		}

		elapsedTime += Time.deltaTime;
		if (elapsedTime >= lifetime) {
			RpcHideTargetForDestruction();
			CmdRegisterHitDestroyAfterTime();
		}
	}

	[Command]
	public void CmdRegisterHitDestroyAfterTime() {
		Destroy(gameObject, 1f);
	}

	[ClientRpc]
	public void RpcHideTargetForDestruction() {
		targetModel.SetActive(false);
	}

	public NetworkInstanceId GetNetId() {
		return networkIdentity.netId;
	}
}
