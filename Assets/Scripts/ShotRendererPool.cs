using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRendererPool : MonoBehaviour {

	public ShotRenderer shotRendererPrefab;
	public int initalPoolCapacity = 10;
	private List<ShotRenderer> pool;

	void Start () {
		pool = new List<ShotRenderer>();
		for (int i = 0; i < initalPoolCapacity; i++) {
			ShotRenderer sr = Instantiate(shotRendererPrefab);
			sr.transform.parent = transform;
			sr.isAvailable = true;
			pool.Add(sr);
		}
	}
	
	public ShotRenderer GetShotRenderer() {
		foreach (ShotRenderer renderer in pool) {
			if (renderer.isAvailable) {
				renderer.isAvailable = false;
				return renderer;
			}
		}

		Debug.Log("No renderer availbale, creating a new one");
		ShotRenderer newShotRenderer = Instantiate(shotRendererPrefab);
		pool.Add(newShotRenderer);
		newShotRenderer.isAvailable = false;
		return newShotRenderer;
	}
}
