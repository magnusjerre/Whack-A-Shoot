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

    // Use this for initialization
    void Start()
    {
        lastRay = new Ray(Vector3.zero, Vector3.up);
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        targetGroupLayerMask = LayerMask.NameToLayer("TargetGroup");
        networkIdentity = GetComponent<NetworkIdentity>();
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
                    CmdSpawnTarget(targetGroup.TargetGroupId, hit.point, networkIdentity.netId);        
                }
            }   
        }
    }

    [Command]
    public void CmdSpawnTarget(int targetGroupId, Vector3 position, NetworkInstanceId owner) {
        var targetGroup = GetTargetGroupById(targetGroupId);
        var newTarget = Instantiate(targetGroup.targetPrefab);
        newTarget.transform.position = position;
        newTarget.transform.LookAt(Vector3.zero, Vector3.up);
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
