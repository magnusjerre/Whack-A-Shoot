using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class ControllerButtonInteractor : MonoBehaviour
{

    private HostButtonScript button;
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // Use this for initialization
    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (device.GetPressUp(triggerButton) && button != null)
        {
            button.Click();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (button == null)
        {
			HostButtonScript otherButton =  other.GetComponent<HostButtonScript>();
			if (otherButton != null) {
				button = otherButton;
            	button.Highlight();
			}
        }
    }

    void OnTriggerStay(Collider other)
    {
    }

    void OnTriggerExit(Collider other)
    {
        if (button != null && other.gameObject == button.gameObject)
        {
            button.UnHighlight();
            button = null;
        }
    }
}
