﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroup : MonoBehaviour {

	public Target targetPrefab;
	[SerializeField]
	private int targetGroupId;
	public int TargetGroupId { get { return targetGroupId; } }
	public float lifetime;

	public int maxScorePerTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
