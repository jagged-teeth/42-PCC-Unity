using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
	public static GameUIManager Instance; // Singleton instance
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI pointsText;
	public CanvasGroup gameUICanvasGroup;

	private void Awake()
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

	private void Start()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneLoaded += OnSceneLoaded;
		UpdateUI();
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void Update()
	{
		UpdateUI();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.StartsWith("Stage"))
			gameUICanvasGroup.alpha = 1;
		else
			gameUICanvasGroup.alpha = 0;
	}

	public void UpdateUI()
	{
		healthText.text = GameManager.Instance.GetPlayerHealth().ToString();
		pointsText.text = GameManager.Instance.GetCollectedItems().ToString();
	}
}
