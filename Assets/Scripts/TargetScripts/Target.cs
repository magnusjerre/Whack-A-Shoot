using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Target : NetworkBehaviour {


	[SerializeField]
	private Transform center;
	public Transform Center { get { return center; } }

	private ParticleSystem ps;
	[SerializeField]
	private GameObject targetModel;

	private NetworkIdentity networkIdentity;
	public NetworkInstanceId ownerId;
	public float lifetime;
	private float elapsedTime;
	private bool hitRegistered = false;

	public int MaxPoints;

	void Start () {
		ps = GetComponentInChildren<ParticleSystem>();
		networkIdentity = GetComponent<NetworkIdentity>();
	}

	void Update() {
		if (!isServer || hitRegistered) {
			return;
		}

		elapsedTime += Time.deltaTime;
		if (elapsedTime >= lifetime) {
			var scoreManager = GameObject.FindObjectOfType<ScoreManager>();
			scoreManager.CmdNotifyTargetLifetimeExpired(ownerId, GetNetId(), MaxPoints);
			RpcHideTargetForDestruction();
			CmdRegisterHitDestroyAfterTime();
		}
	}

	[Command]
	public void CmdRegisterHitDestroyAfterTime() {
		hitRegistered = true;
		Destroy(gameObject, 1f);
	}

	[ClientRpc]
	public void RpcHideTargetForDestruction() {
		var childMeshRenderers = GetComponentsInChildren<MeshRenderer> ();
		foreach (var renderer in childMeshRenderers) {
			renderer.enabled = false;
		}
		targetModel.SetActive(false);
	}

	public NetworkInstanceId GetNetId() {
		return networkIdentity.netId;
	}

	public TargetHitInfo CreateHitInfo(Vector3 offset) {
		return new TargetHitInfo(elapsedTime, lifetime, 1f, MaxPoints, GetNetId(), ownerId);
	}

	public void LookAt(Vector3 position) {
		var winkingComponent = GetComponent<Waving> ();
		if (winkingComponent != null)
		{
			winkingComponent.LookAt (position);
		}
		else
		{
			transform.LookAt (position, Vector3.up);
		}
	}
}
