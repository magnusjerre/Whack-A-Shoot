using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour 
{
	public Axis localAxis;
	public float totalTime;
	
	void Update () 
	{
		float dr = (360f / totalTime) * Time.deltaTime;
		Space space = Space.World;
		if (localAxis == Axis.FORWARD) 
		{
			transform.Rotate (transform.forward, dr, space);
		}
		else if (localAxis == Axis.RIGHT) 
		{
			transform.Rotate (transform.right, dr, space);
		}
		else 
		{
			transform.Rotate(transform.up, dr, space);
		}
	}
}
