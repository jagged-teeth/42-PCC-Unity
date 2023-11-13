using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5.0f;
	public float fixedDistance = 10.0f;
	private float zoomSpeed = 50.0f;
	private float minZoom = 5.0f;
	private float maxZoom = 20.0f;

	private Vector3 initialForward;

	private void Start()
	{
		initialForward = transform.forward;
	}

	private void Update()
	{
		float scrollData = Input.GetAxis("Mouse ScrollWheel");

		if (scrollData > 0f)
		{
			fixedDistance = Mathf.Clamp(fixedDistance * (1 - zoomSpeed * Time.deltaTime), minZoom, maxZoom);
		}
		else if (scrollData < 0f)
		{
			fixedDistance = Mathf.Clamp(fixedDistance * (1 + zoomSpeed * Time.deltaTime), minZoom, maxZoom);
		}
	}

	private void FixedUpdate()
	{
		if (target != null)
		{
			Vector3 desiredPosition = target.position - (initialForward * fixedDistance);
			transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);
		}
	}
}


//public class CameraFollow : MonoBehaviour
//{
//	public Transform target;
//	public float smoothing = 5.0f;
//	public float zoomSpeed = 1.0f;
//	public float minZoom = 5.0f;
//	public float maxZoom = 20.0f;
//	public float rotationSpeed = 50.0f;

//	private Vector3 offset;

//	private void Start()
//	{
//		offset = transform.position - target.position;
//	}

//	private void Update()
//	{
//		float scrollData = Input.GetAxis("Mouse ScrollWheel");
//		Vector3 newOffset = offset;

//		if (scrollData > 0f)
//		{
//			newOffset = Vector3.Lerp(offset, offset * (1 - zoomSpeed * Time.deltaTime), smoothing * Time.deltaTime);
//		}
//		else if (scrollData < 0f)
//		{
//			newOffset = Vector3.Lerp(offset, offset * (1 + zoomSpeed * Time.deltaTime), smoothing * Time.deltaTime);
//		}

//		if (newOffset.magnitude >= minZoom && newOffset.magnitude <= maxZoom)
//		{
//			offset = newOffset;
//		}

//		if (Input.GetMouseButton(1))
//		{
//			float horizontalInput = Input.GetAxis("Mouse X");
//			offset = Quaternion.AngleAxis(horizontalInput * rotationSpeed * Time.deltaTime, Vector3.up) * offset;
//		}
//	}

//	private void FixedUpdate()
//	{
//		if (target != null)
//		{
//			Vector3 targetCamPos = target.position + offset;
//			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
//			transform.LookAt(target);
//		}
//	}
//}
