using UnityEngine;
using System.Collections;

public class ToggleCanvas : MonoBehaviour
{
	[SerializeField]
	private float lifetime;
	private float elapsedTime;

	private Canvas canvas;
	private bool show;

	void Awake() 
	{
		canvas = GetComponent<Canvas> ();
	}

	void Update ()
	{
		if (show) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= lifetime) 
			{
				Hide ();
			}
		}
	}

	public void Show(Vector3 position) {
		transform.position = position;
		elapsedTime = 0f;
		canvas.enabled = true;
		show = true;
	}

	public void Hide() {
		show = false;
		canvas.enabled = false;
	}

	public bool IsAvailable() {
		return !show;
	}

}

