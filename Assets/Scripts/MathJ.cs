using UnityEngine;
using System.Collections;

public class MathJ
{
	public static float SLerp(float progress) 
	{
		progress = Mathf.Clamp01 (progress);
		Vector2 p0 = Vector2.zero;
		Vector3 p1 = new Vector2 (0.5f, 0);
		Vector3 p2 = new Vector2 (0.5f, 1);
		Vector2 p3 = Vector2.one;

		float inv = 1 - progress;
		float inv2 = Mathf.Pow (inv, 2);
		float inv3 = Mathf.Pow (inv, 3);
		float progress2 = Mathf.Pow (progress, 2);
		float progress3 = Mathf.Pow (progress, 3);

		Vector2 pt0 = p0 * inv3;
		Vector2 pt1 = 3 * inv2 * progress * p1;
		Vector2 pt2 = 3 * inv * progress2 * p2;
		Vector2 pt3 = progress3 * p3;

		return (pt0 + pt1 + pt2 + pt3).y;
	}
}

