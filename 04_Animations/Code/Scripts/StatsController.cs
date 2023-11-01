using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsController : MonoBehaviour
{
	public TextMeshProUGUI remainingLivesText;
	public TextMeshProUGUI collectedItemsText;
	public TextMeshProUGUI unlockedStagesText;

	void Start()
	{
		UpdateStats();
	}

	public void UpdateStats()
	{
		int remainingLives = PlayerPrefs.GetInt("PlayerLives", 0);
		int collectedItems = PlayerPrefs.GetInt("StatsPoints", 0);
		string lastStage = PlayerPrefs.GetString("LastStage", "Stage1");

		remainingLivesText.text = $"Remaining Lives: {remainingLives}";
		collectedItemsText.text = $"Collectible Points: {collectedItems}";
		unlockedStagesText.text = $"Last Stage Reached: {lastStage}";
	}
}
