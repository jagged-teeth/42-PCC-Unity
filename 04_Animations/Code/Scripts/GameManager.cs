using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance; // Singleton instance
	private int collectedItems = 0;
	private int statsPoints = 0;
	private HashSet<string> collectedItemsIDs = new HashSet<string>();
	private int playerHealth = 5;
	private int playerLives = 0;
	internal string lastStage = "Stage1";


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	void Update()
	{
		KeyManage();
	}

	public void KeyManage()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (SceneManager.GetActiveScene().name == "MenuScene")
				Application.Quit();
			else
			{
				SaveGameState();
				SceneManager.LoadScene("MenuScene");
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
			SaveGameState();

		if (Input.GetKeyDown(KeyCode.R))
			LoadGameState();
	}

	// Game State

	public void SaveGameState()
	{
		if (collectedItemsIDs != null && collectedItemsIDs.Count > 0)
		{
			string collectedItemsIDString = string.Join(",", collectedItemsIDs);
			PlayerPrefs.SetString("CollectedItemsIDs", collectedItemsIDString);
		}
		else
			PlayerPrefs.SetString("CollectedItemsIDs", "");

		PlayerPrefs.SetInt("StatsPoints", statsPoints);
		PlayerPrefs.SetInt("CollectedItems", collectedItems);
		PlayerPrefs.SetInt("PlayerHealth", playerHealth);
		PlayerPrefs.SetInt("PlayerLives", playerLives);

		lastStage = SceneManager.GetActiveScene().name;
		PlayerPrefs.SetString("LastStage", lastStage);

		PlayerPrefs.Save();

		Debug.Log("Collected items IDs: " + PlayerPrefs.GetString("CollectedItemsIDs", ""));
		Debug.Log("Collected items: " + collectedItems);
		Debug.Log("Stats points: " + statsPoints);
		Debug.Log("Player health: " + playerHealth);
		Debug.Log("Player lives: " + playerLives);
		Debug.Log("Last stage: " + lastStage);
	}

	public void LoadGameState()
	{
		collectedItemsIDs = GetCollectedItemsIDs();
		collectedItems = GetCollectedItems();
		statsPoints = GetStatsPoints();
		playerHealth = GetPlayerHealth();
		playerLives = GetPlayerLives();
		lastStage = GetLastStage();

		Debug.Log("Collected items IDs: " + PlayerPrefs.GetString("CollectedItemsIDs", ""));
		Debug.Log("Collected items: " + collectedItems);
		Debug.Log("Stats points: " + statsPoints);
		Debug.Log("Player health: " + playerHealth);
		Debug.Log("Player lives: " + playerLives);
		Debug.Log("Last stage: " + lastStage);
	}

	public void ResetGame()
	{
		ResetCollectibles();
		ResetPlayerHealth();
		ResetPlayerLives();
		ResetLastStage();

		Debug.Log("Collected items: " + collectedItems);
		Debug.Log("Stats points: " + statsPoints);
		Debug.Log("Player health: " + playerHealth);
		Debug.Log("Player lives: " + playerLives);
		Debug.Log("Last stage: " + lastStage);
	}

	public void CheckLevelCompletion()
	{
		if (collectedItems >= 25)
		{
			Debug.Log("Level completed !");
			GoToNextStage();
		}
		else
		{
			ExitController exitController = FindObjectOfType<ExitController>();

			if (exitController != null)
			{
				exitController.ExitText();
				Debug.Log("Exit text displayed.");
			}
			else
				Debug.LogError("ExitController instance not found.");
		}
	}

	// Collectibles

	public void Collect(string uniqueID, int value)
	{
		collectedItemsIDs.Add(uniqueID);
		collectedItems += value;
		statsPoints += value;
		PlayerPrefs.SetInt("StatsPoints", statsPoints);
		PlayerPrefs.SetInt("CollectedItems", collectedItems);
		PlayerPrefs.SetString(uniqueID, "collected");
	}

	public void ResetCollectibles()
	{
		collectedItems = 0;
		PlayerPrefs.SetInt("CollectedItems", collectedItems);
		PlayerPrefs.SetString("CollectedItemsIDs", "");
	}

	public int GetCollectedItems()
	{
		return PlayerPrefs.GetInt("CollectedItems", collectedItems);
	}

	public int GetStatsPoints()
	{
		return PlayerPrefs.GetInt("StatsPoints", statsPoints);
	}

	public HashSet<string> GetCollectedItemsIDs()
	{
		string collectedItemsIDString = PlayerPrefs.GetString("CollectedItemsIDs", "");
		if (!string.IsNullOrEmpty(collectedItemsIDString))
			collectedItemsIDs = new HashSet<string>(collectedItemsIDString.Split(','));
		else
			collectedItemsIDs.Clear();
		return collectedItemsIDs;
	}

	public void DestroyCollectedItems()
	{
		foreach (string uniqueID in collectedItemsIDs)
		{
			GameObject item = GameObject.Find(uniqueID);
			if (item != null)
				Destroy(item);
		}
	}

	// Player Health

	public void SetPlayerHealth(int health)
	{
		playerHealth = health;
		PlayerPrefs.SetInt("PlayerHealth", playerHealth);
		Debug.Log("Player took " + playerHealth + " damage. Health is now " + health);
	}

	public int GetPlayerHealth()
	{
		return PlayerPrefs.GetInt("PlayerHealth", playerHealth);
	}

	public void ResetPlayerHealth()
	{
		playerHealth = 5;
		PlayerPrefs.SetInt("PlayerHealth", playerHealth);
	}

	// Player Lives

	public void SetPlayerLives(int lives)
	{
		playerLives = lives;
		PlayerPrefs.SetInt("PlayerLives", playerLives);
		Debug.Log("Player lives: " + playerLives);
	}

	public int GetPlayerLives()
	{
		return PlayerPrefs.GetInt("PlayerLives", playerLives);
	}

	public void ResetPlayerLives()
	{
		playerLives = 3;
		PlayerPrefs.SetInt("PlayerLives", playerLives);
	}

	// Stage

	public void SetLastStage(string stage)
	{
		lastStage = stage;
		PlayerPrefs.SetString("LastStage", lastStage);
		Debug.Log("Last stage: " + lastStage);
	}

	public string GetLastStage()
	{
		return PlayerPrefs.GetString("LastStage", lastStage);
	}

	public void ResetLastStage()
	{
		lastStage = "Stage1";
		PlayerPrefs.SetString("LastStage", lastStage);
	}

	public void GoToNextStage()
	{
		if (SceneManager.GetActiveScene().name == "Stage2")
		{
			Debug.Log("Game completed !");
			SceneManager.LoadScene("Ending");
			return;
		}
		else
		{
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
		PlayerPrefs.SetString("LastStage", SceneManager.GetActiveScene().name);
		ResetCollectibles();
		SceneManager.LoadScene(nextSceneIndex);
		}
	}
}
