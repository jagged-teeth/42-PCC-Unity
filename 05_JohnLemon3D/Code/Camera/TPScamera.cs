using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPScamera : MonoBehaviour
{
	public Transform orientation;
	public Transform player;
	public Transform playerObj;
	public float rotationSpeed;

	public void Update()
	{
		Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
		orientation.forward = viewDir.normalized;

		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");
		Vector3 inputDir = orientation.forward * moveZ + orientation.right * moveX;

		if (inputDir != Vector3.zero)
			playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
	}
}
