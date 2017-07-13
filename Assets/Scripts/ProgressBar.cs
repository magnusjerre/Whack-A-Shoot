using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	public Image progressImage;
	private RectTransform progressTransform;

	public Color startColor;
	public Color endColor;

	// Use this for initialization
	void Start ()
	{
		progressTransform = progressImage.GetComponent<RectTransform> ();
		progressImage.color = startColor;
	}
	
	public void SetProgress(float progress) {
		progress = Mathf.Clamp01 (progress);

		progressTransform.anchorMax = new Vector2 (progress, 1f);
		progressImage.color = Color.Lerp (startColor, endColor, progress);
	}
}

