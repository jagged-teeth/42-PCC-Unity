using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
	public Transform cameraTransform;
	public float parallaxFactor;  // Range (0,1), where 0 means no movement and 1 means moves with the camera
	private Vector3 lastCameraPosition;

	void Start()
	{
		lastCameraPosition = cameraTransform.position;
	}

	void LateUpdate()
	{
		Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

		// We only care about X-axis movement for a side scroller
		transform.position += new Vector3(deltaMovement.x * parallaxFactor, 0, 0);

		lastCameraPosition = cameraTransform.position;
	}
}
