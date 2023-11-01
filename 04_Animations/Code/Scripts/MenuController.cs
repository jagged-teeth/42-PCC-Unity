using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public void NewGame()
	{
		Debug.LogWarning("New Game started");
		GameManager.Instance.ResetGame();
		SceneManager.LoadScene("Stage1");
	}

	public void LoadStats()
	{
		Debug.LogWarning("Loading stats");
		SceneManager.LoadScene("Stats");
	}

	public void ReturnToMainMenu()
	{
		Debug.LogWarning("Returning to main menu");
		SceneManager.LoadScene("MenuScene");
	}

	public void ExitGame()
	{
		Debug.LogWarning("Exiting game");
		Application.Quit();
	}

	public void ResumeGame()
	{
		Debug.LogWarning("Resuming game");
		GameManager.Instance.LoadGameState();
		SceneManager.LoadScene(GameManager.Instance.lastStage);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == GameManager.Instance.lastStage)
		{
			GameManager.Instance.DestroyCollectedItems();
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}

