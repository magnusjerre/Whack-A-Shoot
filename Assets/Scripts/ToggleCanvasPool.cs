using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleCanvasPool : MonoBehaviour
{

	public ToggleCanvas prefab;
	public int initalCapacity = 3;
	private List<ToggleCanvas> pool;

	void Awake() {
		pool = new List<ToggleCanvas> ();
	}

	// Use this for initialization
	void Start ()
	{
		for (var i = 0; i < initalCapacity; i++) 
		{
			var tc = Instantiate<ToggleCanvas> (prefab, transform);
			tc.Hide ();
			pool.Add (tc);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public ToggleCanvas Get() {
		for (var i = 0; i < pool.Count; i++) 
		{
			var tc = pool [i];
			if (tc.IsAvailable()) 
			{
				return tc;
			}
		}

		var newTc = Instantiate(prefab, transform);
		newTc.Hide();
		pool.Add(newTc);
		return newTc;
	}
}

