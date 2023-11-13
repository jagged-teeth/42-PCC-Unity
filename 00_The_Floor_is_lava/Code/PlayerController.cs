using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float speed = 1.0f;
	public float jumpForce = 5.0f;
	public bool isJumping = false;
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		float moveHorizontal = Input.GetAxis("Horizontal") * -1;
		float moveVertical = Input.GetAxis("Vertical") * -1;

		Vector3 movementVector = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.AddForce(movementVector * speed);

		if (Input.GetButtonDown("Jump") && !isJumping)
		{
			rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			isJumping = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			#if UNITY_EDITOR
               		 UnityEditor.EditorApplication.isPlaying = false;
            		#endif
		}
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Plane")
		{
			Debug.Log("Game Over");
			Destroy(gameObject);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		isJumping = false;
	}
}
