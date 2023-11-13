using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 1.0f;
	public float jumpForce = 5.0f;
	public float rotationTorque = 0.1f;
	public bool canMove = false;
	public bool isJumping = false;
	public LayerMask layerMasks;
	public delegate void AlignementHandler();
	public event AlignementHandler OnAligned;
	public string Tag;


	private Rigidbody rb;
	private int numContacts = 0;
	private Vector3 initialPosition;
	private Vector3 newPosition;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		Tag = gameObject.tag;
	}

	void Update()
	{
		if (canMove)
		{
			float moveHorizontal = Input.GetAxis("Horizontal");
			Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
			rb.AddForce(movement * speed);

			if (Input.GetButtonDown("Jump") && CanJump())
			{
				rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
				isJumping = true;
			}

			if (isJumping == true && Input.GetKey(KeyCode.LeftArrow))
			{
				rb.AddTorque(0, 0, rotationTorque, ForceMode.Force);
			}
			if (isJumping == true && Input.GetKey(KeyCode.RightArrow))
			{
				rb.AddTorque(0, 0, -rotationTorque, ForceMode.Force);
			}
			// isJumping boolean has no action here and I have no idea why
		}
	}

	//void OnCollisionEnter(Collision collision)
	//{
	//	string platformLayer = LayerMask.LayerToName(collision.gameObject.layer);

	//	if (collision.gameObject.tag == "Wall")
	//		Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), false);
	//	else if (IsValidJumpSurface(collision.gameObject.layer) || platformLayer.Contains(Tag))
	//	{
	//		Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), false);
	//		numContacts++;
	//		isJumping = false;
	//	}
	//	else
	//		Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
	//}

	void OnCollisionEnter(Collision collision)
	{
		if (IsValidJumpSurface(collision.gameObject.layer))
		{
			numContacts++;
			isJumping = false;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (IsValidJumpSurface(collision.gameObject.layer))
		{
			numContacts = Mathf.Max(0, numContacts - 1);
		}
	}

	bool IsValidJumpSurface(int layer)
	{
		bool isValid = layer == LayerMask.NameToLayer("Ground") ||
					   layer == LayerMask.NameToLayer("Claire") ||
					   layer == LayerMask.NameToLayer("John") ||
					   layer == LayerMask.NameToLayer("Thomas") ||
					   layer == LayerMask.NameToLayer("Platform");

		if (Tag == "Claire")
			isValid = isValid || (layer == LayerMask.NameToLayer("PlatformClaire"));
		else if (Tag == "John")
			isValid = isValid || (layer == LayerMask.NameToLayer("PlatformJohn"));
		else if (Tag == "Thomas")
			isValid = isValid || (layer == LayerMask.NameToLayer("PlatformThomas"));

		return isValid;
	}

	bool CanJump()
	{
		return numContacts > 0;
	}

	public void ToggleMovement(bool canMove)
	{
		this.canMove = canMove;
	}

	public void Align()
	{
		if (OnAligned != null)
			OnAligned();
	}
}
