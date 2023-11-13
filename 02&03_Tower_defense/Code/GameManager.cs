using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public GameObject pauseMenuUI;
	public GameObject confirmationUI;
	public TextMeshProUGUI scoreScreen;
	public GameObject scoreScreenObject;

	private int totalEnemies;
	private int defeatedEnemies = 0;
	private BaseScript baseScript;
	private EnergyManager energyManager;
	private bool isPaused = false;

	void Start()
	{
		Time.timeScale = 1f;
		baseScript = FindObjectOfType<BaseScript>();
		energyManager = FindObjectOfType<EnergyManager>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
				ResumeGame();
			else
				PauseGame();
		}

		CheckForScoreScreen();
	}

	private void OnEnable()
	{
		EventSystem.current.EnemyDestroyed += IncrementDefeatedEnemiesCount;
	}

	private void OnDisable()
	{
		EventSystem.current.EnemyDestroyed -= IncrementDefeatedEnemiesCount;
	}

	private void IncrementDefeatedEnemiesCount()
	{
		defeatedEnemies++;
		Debug.Log("Defeated enemies: " + defeatedEnemies);
		CheckForScoreScreen();
	}

	void CheckForScoreScreen()
	{
		EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
		totalEnemies = 0;

		foreach (EnemySpawner spawner in spawners)
		{
			totalEnemies += spawner.maxSpawnCount;
		}

		//if (spawner != null)
		//{
		//	totalEnemies = spawner.maxSpawnCount;
		//}

		if (totalEnemies > 0 && defeatedEnemies >= totalEnemies)
			DisplayScoreScreen();
	}

	void DisplayScoreScreen()
	{
		float healthNormalized = (float)baseScript.health / 5;
		float scorePercentage = (healthNormalized + energyManager.energy) / 2;
		char rank = CalculateRank(scorePercentage);

		if (scoreScreen != null && scoreScreenObject != null)
		{
			scoreScreen.text = "Rank: " + rank;
			scoreScreenObject.SetActive(true);
		}
	}

	char CalculateRank(float scorePercentage)
	{
		if (scorePercentage >= 0.92f) return 'S';
		else if (scorePercentage >= 0.82f) return 'A';
		else if (scorePercentage >= 0.74f) return 'B';
		else if (scorePercentage >= 0.68f) return 'C';
		else if (scorePercentage >= 0.6f) return 'D';
		else return 'F';
	}

	public void PauseGame()
	{
		if (pauseMenuUI != null && confirmationUI != null)
		{
			pauseMenuUI.SetActive(true);
			confirmationUI.SetActive(false);
			Time.timeScale = 0f;
			isPaused = true;
		}
		else
		{
			Debug.LogError("UI GameObjects are not assigned in the GameManager.");
		}
	}

	public void ResumeGame()
	{
		pauseMenuUI.SetActive(false);
		confirmationUI.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
	}

	public void RequestReturnToMainMenu()
	{
		confirmationUI.SetActive(true);
	}

	public void ReturnToMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}

	public void CancelReturnToMainMenu()
	{
		confirmationUI.SetActive(false);
	}

	public void loadNextStage()
	{
		SceneManager.LoadScene("Stage2");
	}

	public void reloadStage()
	{
		BaseScript.isGameOver = false;
		SceneManager.LoadScene("Stage1");
	}
}
