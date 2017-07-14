﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNVR : NetworkBehaviour
{

    public float amount;
    private Camera camera;
    Ray lastRay;

    LayerMask targetGroupLayerMask;
    private NetworkIdentity networkIdentity;

	public float minTimeBetweenTargetPlacements, maxTimeBetweenTargetPlacements;
	private float elapsedTimeSinceLastPlacement;

	public ProgressBar minTimeProgressBar, maxTimeProgressBar;

    // Use this for initialization
    void Start()
    {
        lastRay = new Ray(Vector3.zero, Vector3.up);
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        targetGroupLayerMask = LayerMask.NameToLayer("TargetGroup");
		minTimeProgressBar = GameObject.FindGameObjectWithTag ("MinTimeProgress").GetComponent<ProgressBar> ();
		maxTimeProgressBar = GameObject.FindGameObjectWithTag ("MaxTimeProgress").GetComponent<ProgressBar> ();
    }

    public override void OnStartLocalPlayer()
    {
        var scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.Log("Score doesn't exist at this time");
            return;
        }

        networkIdentity = GetComponent<NetworkIdentity>();
        scoreManager.playerId = networkIdentity.netId;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) {
            return;
        }

        if (Input.GetMouseButtonUp(0)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, ~targetGroupLayerMask.value)) {
                TargetGroup targetGroup = hit.collider.GetComponent<TargetGroup>();
                if (targetGroup != null) {
                    SpawnTarget(targetGroup.TargetGroupId, hit.point, networkIdentity.netId);        
                }
            }   
        }

		elapsedTimeSinceLastPlacement += Time.deltaTime;
		if (elapsedTimeSinceLastPlacement >= maxTimeBetweenTargetPlacements) {
			TargetGroup[] groups = GameObject.FindObjectsOfType<TargetGroup> ();
			int index = Random.Range (0, groups.Length);
			TargetGroup randomGroup = groups [index];
			Vector3 randomPosition = randomGroup.GetRandomPositionOnMesh ();
			SpawnTarget (randomGroup.TargetGroupId, randomPosition, networkIdentity.netId);

			elapsedTimeSinceLastPlacement = 0f;
		}

		minTimeProgressBar.SetProgress (elapsedTimeSinceLastPlacement / minTimeBetweenTargetPlacements);
		maxTimeProgressBar.SetProgress (elapsedTimeSinceLastPlacement / maxTimeBetweenTargetPlacements);

    }

	private void SpawnTarget(int targetGroupId, Vector3 position, NetworkInstanceId ownerId) {
		if (elapsedTimeSinceLastPlacement < minTimeBetweenTargetPlacements) {
			return;
		}
		elapsedTimeSinceLastPlacement = 0f;
		CmdSpawnTarget (targetGroupId, position, ownerId);
	}

    [Command]
    public void CmdSpawnTarget(int targetGroupId, Vector3 position, NetworkInstanceId ownerId) {
        var targetGroup = GetTargetGroupById(targetGroupId);
        var newTarget = Instantiate(targetGroup.targetPrefab);
        newTarget.transform.position = position;
        newTarget.transform.LookAt(Vector3.zero, Vector3.up);
        newTarget.lifetime = targetGroup.lifetime;
        newTarget.ownerId = ownerId;
        newTarget.MaxPoints = targetGroup.maxScorePerTarget;
        NetworkServer.Spawn(newTarget.gameObject);
    }

    public TargetGroup GetTargetGroupById(int id) {
        var groups = GameObject.FindObjectsOfType<TargetGroup>();
        foreach (TargetGroup targetGroup in groups) {
            if (targetGroup.TargetGroupId == id) {
                return targetGroup;
            }
        }
        return null;
    }

}
