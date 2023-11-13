using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5.0f;
	Vector3 offset;

	private void Start()
	{
		offset = transform.position - target.position;
	}

	private void FixedUpdate()
	{
		if (target != null)
		{
			Vector3 targetCamPos = target.position + offset;
			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
		}
	}
}

