using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {


	[SerializeField]
	private Transform center;
	public Transform Center { get { return center; } }

	public int id;

	public bool IsUp;

	private ParticleSystem ps;
	[SerializeField]
	private GameObject targetModel;

	// Use this for initialization
	void Start () {
		ps = GetComponentInChildren<ParticleSystem>();
		ShowTarget();
	}

	public void RegisterHit() {
		IsUp = false;
		targetModel.SetActive(false);
		ps.Play();
	}

	public void ShowTarget() {
		IsUp = true;
		targetModel.SetActive(true);
		ps.Stop();
	}
}
