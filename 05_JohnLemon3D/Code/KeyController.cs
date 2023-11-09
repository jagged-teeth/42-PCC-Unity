using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
	public float rotationSpeed = 100.0f;

	private void Update()
	{
		transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().hasKey += 1;
			Debug.Log("Player collected a key!");
			Destroy(gameObject);
		}
	}
}
