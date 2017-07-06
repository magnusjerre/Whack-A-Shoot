using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private Weapon weapon;
    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        weapon = GetComponentInChildren<Weapon>();
    }

    public bool ReleasedTrigger()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        return device.GetPressUp(triggerButton);
    }

    public Transform Muzzle() {
        return weapon.muzzle;
    }
}
