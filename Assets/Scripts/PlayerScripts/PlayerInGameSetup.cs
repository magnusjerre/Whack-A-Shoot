using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class PlayerInGameSetup : MonoBehaviour {

	[SerializeField]
	private GameObject nvrCamera, vrCamera, nvrMenu, vrMenu;

	void Start () {
		if (VRSettings.enabled && VRSettings.isDeviceActive) {
			Instantiate(vrCamera);
			Instantiate(vrMenu);
		} else {
			Instantiate(nvrCamera);
			Instantiate(nvrMenu);	
		}
	}
	
}
