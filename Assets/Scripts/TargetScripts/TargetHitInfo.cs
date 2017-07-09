using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct TargetHitInfo {
    public float elapsedTime;
    public float lifetime;
    public float precision;
    public int maxPoints;
    public NetworkInstanceId targetId;

    public NetworkInstanceId ownerId;

    public TargetHitInfo(float elapsedTime, float lifetime, float precision, int maxPoints, NetworkInstanceId targetId, NetworkInstanceId ownerId) {
        this.elapsedTime = elapsedTime;
        this.lifetime = lifetime;
        this.precision = precision;
        this.maxPoints = maxPoints;
        this.targetId = targetId;
        this.ownerId = ownerId;
    }
}
