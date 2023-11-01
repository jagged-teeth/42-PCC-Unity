using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public Vector2 offset;
	public Vector2 mapBoundsMin;
	public Vector2 mapBoundsMax;

	void Update()
	{
		Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + (Vector3)offset;

		// Clamp within map bounds
		float clampedX = Mathf.Clamp(targetPosition.x, mapBoundsMin.x, mapBoundsMax.x);
		float clampedY = Mathf.Clamp(targetPosition.y, mapBoundsMin.y, mapBoundsMax.y);

		transform.position = new Vector3(clampedX, clampedY, transform.position.z);
	}
}
