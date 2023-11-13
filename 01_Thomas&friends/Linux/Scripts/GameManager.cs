using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public ExitAlign[] exitAligns;

	void Update()
	{
		CheckAllCharactersAligned();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}

	void CheckAllCharactersAligned()
	{
		foreach (ExitAlign exitAlign in exitAligns)
		{
			if (!exitAlign.IsAligned)
				return;
		}
		Debug.Log("All characters aligned! Advancing to next stage...");
		AdvanceStage();
	}

	void AdvanceStage()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;
		if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
			nextSceneIndex = 0;
		SceneManager.LoadScene(nextSceneIndex);
	}
}
