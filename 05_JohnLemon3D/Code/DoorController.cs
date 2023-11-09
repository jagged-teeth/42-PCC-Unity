using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private Quaternion closedRotation;
	private bool isDoorOpen = false;
	private UnityEngine.AI.NavMeshObstacle obstacle;
	public int requiredKeys = 3;
	public AudioSource audioSource;
	public AudioClip doorOpenSound;
	public AudioClip doorLocked;

	void Start()
	{
		closedRotation = transform.rotation;
		obstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isDoorOpen)
		{
			int keys = other.GetComponent<PlayerController>().hasKey;
			if (keys >= requiredKeys)
			{
				StartCoroutine(RotateDoor());
				audioSource.PlayOneShot(doorOpenSound);
			}
			else
				audioSource.PlayOneShot(doorLocked);
		}
	}

	private IEnumerator RotateDoor()
	{
		isDoorOpen = true;

		Quaternion openRotation = closedRotation * Quaternion.Euler(0, -90, 0);

		float duration = 1.0f;
		float timeElapsed = 0;

		while (timeElapsed < duration)
		{
			transform.rotation = Quaternion.Lerp(closedRotation, openRotation, timeElapsed / duration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		transform.rotation = openRotation;
		obstacle.enabled = false;
	}
}
