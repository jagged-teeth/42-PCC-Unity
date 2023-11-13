using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseScript : MonoBehaviour
{
	public int health = 5;
	public static bool isGameOver = false;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI gameOverText;
	public GameObject gameOverUI;

	private void Start()
	{
		UpdateUI();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Trigger entered by: " + other.gameObject.tag);
		if (other.CompareTag("Enemy") && !isGameOver)
		{
			Destroy(other.gameObject);
			EventSystem.current.OnEnemyDestroyed();
			health--;
			Debug.Log("Remaining HP: " + health);

			UpdateUI();

			if (health <= 0)
			{
				health = 0;
				isGameOver = true;
				Debug.Log("Game Over");
				EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
				if (spawner != null)
					spawner.CancelInvoke("SpawnEnemy");
				foreach (var enemy in FindObjectsOfType<NavigationScript>())
				{
					Destroy(enemy.gameObject);
				}

				if (gameOverText != null && gameOverUI != null)
				{
					gameOverUI.SetActive(true);
					gameOverText.text = "GAME OVER";
				}
			}
		}
	}

	private void UpdateUI()
	{
		if (healthText != null)
			healthText.text = health.ToString();
	}
}
