using UnityEngine;
using System.Collections;

public class Waving : MonoBehaviour
{
	public Axis localAxis;
	public float totalAngle = 90f;

	//Live changeable
	public float totalTime = 2f;
	public bool smoothLerp = true;

	private float elapsedTime;

	private Quaternion initialRotation, endRotation1, endRotation2;
	private Quaternion fromRotation, toRotation;

	void Awake() 
	{
		LookAt (transform.position + transform.forward);
	}

	void Update ()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= totalTime) 
		{
			Quaternion temp = fromRotation;
			fromRotation = toRotation;
			toRotation = temp;
			elapsedTime = 0f;
		}

		if (smoothLerp)
		{
			transform.localRotation = Quaternion.Slerp (fromRotation, toRotation, MathJ.SLerp(elapsedTime / totalTime));
		}
		else
		{
			transform.localRotation = Quaternion.Slerp (fromRotation, toRotation, elapsedTime / totalTime);
		}
	}

	public void LookAt(Vector3 position) {
		transform.LookAt (position, Vector3.up);

		initialRotation = transform.localRotation;
		var offset = Vector3.zero;
		if (localAxis == Axis.FORWARD) 
		{
			offset = transform.InverseTransformVector(transform.forward) * (totalAngle / 2);
		} 
		else if (localAxis == Axis.RIGHT) 
		{
			offset = transform.InverseTransformVector(transform.right) * (totalAngle / 2);
		}
		else 
		{
			offset = transform.InverseTransformVector(transform.up) * (totalAngle / 2);
		}
		endRotation1 = Quaternion.Euler (initialRotation.eulerAngles + offset);
		endRotation2 = Quaternion.Euler (initialRotation.eulerAngles - offset);

		transform.localRotation = endRotation1;

		fromRotation = endRotation1;
		toRotation = endRotation2;
	}
}

