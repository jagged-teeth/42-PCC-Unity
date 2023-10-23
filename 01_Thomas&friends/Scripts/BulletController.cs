using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Thomas"))
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(currentSceneIndex);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
