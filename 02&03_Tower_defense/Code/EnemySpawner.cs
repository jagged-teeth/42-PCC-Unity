using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;
	public float initialSpawnInterval = 5f;
	public float acceleratedSpawnInterval = 2f;
	public float acceleratedSpawnInterval2 = 1f;
	public int accelerationPoint = 10;
	public int accelerationPoint2 = 20;
	public int maxSpawnCount = 40;

	private float currentSpawnInterval;
	private int currentSpawnCount = 0;

	private void Start()
	{
		currentSpawnInterval = initialSpawnInterval;
		StartCoroutine(SpawnEnemy());
	}

	IEnumerator SpawnEnemy()
	{
		while (currentSpawnCount < maxSpawnCount && !BaseScript.isGameOver)
		{
			yield return new WaitForSeconds(currentSpawnInterval);
			Instantiate(enemyPrefab, transform.position, Quaternion.identity);
			currentSpawnCount++;

			if (currentSpawnCount == accelerationPoint)
			{
				currentSpawnInterval = acceleratedSpawnInterval;
			}
			if (currentSpawnCount == accelerationPoint2)
			{
				currentSpawnInterval = acceleratedSpawnInterval2;
			}
		}
	}
}
