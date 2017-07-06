using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class PlayerInGameSetup : MonoBehaviour {

	[SerializeField]
	private GameObject nvrCamera, vrCamera;

	void Start () {
		if (VRSettings.enabled && VRSettings.isDeviceActive) {
			Instantiate(vrCamera);
		} else {
			Instantiate(nvrCamera);
		}
	}
	
}
