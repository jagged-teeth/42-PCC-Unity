using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleController : MonoBehaviour
{
	public GameObject[] ghosts;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			AlertGhosts(other.transform.position);
	}

	private void AlertGhosts(Vector3 playerPosition)
	{
		foreach (GameObject ghost in ghosts)
			ghost.GetComponent<NavigationController>().Alerted(playerPosition);
	}
}
