using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
	public GameObject otherTeleporter;
	public bool canTeleport = true;

	void OnTriggerEnter(Collider other)
	{
		if (canTeleport && (other.CompareTag("Claire") || other.CompareTag("John") || other.CompareTag("Thomas")))
		{
			otherTeleporter.GetComponent<Teleporter>().canTeleport = false;
			other.transform.position = otherTeleporter.transform.position;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Claire") || other.CompareTag("John") || other.CompareTag("Thomas"))
		{
			canTeleport = true;
		}
	}
}
