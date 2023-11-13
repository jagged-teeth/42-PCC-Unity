using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public Vector3 origin;
	public Vector3 destination;
	public float speed = 1.0f;

	private bool toDestination = true;

	void Update()
	{
		if (toDestination)
		{
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed * Time.deltaTime);
			if (transform.localPosition == destination)
				toDestination = false;
		}
		else
		{
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, origin, speed * Time.deltaTime);
			if (transform.localPosition == origin)
				toDestination = true;
		}
	}
}
