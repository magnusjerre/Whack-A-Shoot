using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (Rotator))]
public class Target : NetworkBehaviour {


	[SerializeField]
	private Transform center;
	public Transform Center { get { return center; } }

	[SyncVar(hook = "ChangeIsUpTo")]
	public bool IsUp;

	private ParticleSystem ps;
	[SerializeField]
	private GameObject targetModel;

	private Rotator rotator;

	private NetworkIdentity networkIdentity;

	void Start () {
		ps = GetComponentInChildren<ParticleSystem>();
		networkIdentity = GetComponent<NetworkIdentity>();
		
		rotator = GetComponent<Rotator>();
		//Want to set the target to be lying down at first
		transform.rotation = rotator.TargetRotation;	
		rotator.rotationAmount = -rotator.rotationAmount;
		rotator.SetOriginRotation();

		CmdSetIsUp(true);
	}

	public void RegisterHit() {
		IsUp = false;
		targetModel.SetActive(false);
		ps.Play();
	}

	[Command]
	public void CmdSetIsUp(bool newValue) {
		Debug.Log("SetIsUp");
		this.IsUp = newValue;
		// ChangeIsUpTo(newValue);
	}

	[ClientRpc]
	public void RpcAnimateHit() {
		ps.Play();
	}

	public void ChangeIsUpTo(bool newValue) {
		Debug.Log("ChangeIsUpTo");
		if (newValue) {
			rotator.RotateToTarget();
		} else {
			rotator.RotateToOrigin();
		}
		this.IsUp = newValue;
	}

	public NetworkInstanceId GetNetId() {
		return networkIdentity.netId;
	}
}
