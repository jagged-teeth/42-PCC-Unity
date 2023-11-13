using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
	public float delay = 2f;
	public string targetSceneName = "Stage1";

	void Start()
	{
		Debug.Log("Transition started");
		StartCoroutine(LoadSceneAfterDelay());
	}

	IEnumerator LoadSceneAfterDelay()
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(targetSceneName);
		Debug.Log("Stage 1 loaded");
	}
}
