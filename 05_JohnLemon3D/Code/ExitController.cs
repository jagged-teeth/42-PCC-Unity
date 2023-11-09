using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
	private GameController gameController;

	private void Start()
	{
		gameController = FindObjectOfType<GameController>();
		if (gameController == null)
			Debug.LogError("GameController not found in the scene!");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			gameController.LevelComplete();
		}
	}
}
