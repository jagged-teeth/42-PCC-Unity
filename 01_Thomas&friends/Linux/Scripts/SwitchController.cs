using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
	public GameObject door;
	public GameObject button;
	public Vector3 destination;
	public Vector3 buttonPressed;
	public float speed = 1.0f;

	private bool isSwitchActive = false;

	void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("Claire") || other.CompareTag("John") ||
			other.CompareTag("Thomas")) && !isSwitchActive)
			isSwitchActive = true;
	}

	void Update()
	{
		if (isSwitchActive)
		{
			button.transform.localPosition = Vector3.MoveTowards(button.transform.localPosition, buttonPressed, speed * Time.deltaTime);
			door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, destination, speed * Time.deltaTime);
			if (door.transform.localPosition == destination)
				isSwitchActive = false;
		}
	}
}

