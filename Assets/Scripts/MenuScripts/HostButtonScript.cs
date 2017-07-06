using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class HostButtonScript : MonoBehaviour {

	private MainMenuScript mainMenuScript;
	[SerializeField]
	private Color highlightColor;
	private Color normalColor;

	private Collider thisCollider;

	// Use this for initialization
	void Start () {
		mainMenuScript = GameObject.FindObjectOfType<MainMenuScript>();
		normalColor = GetComponent<Renderer>().material.color;
		thisCollider = GetComponent<Collider>();
		thisCollider.isTrigger = true;
	}
	
	public void Click() {
		mainMenuScript.Host();
	}

	public void Highlight() {
		GetComponent<Renderer>().material.color = highlightColor;
	}

	public void UnHighlight() {
		GetComponent<Renderer>().material.color = normalColor;
	}

}
