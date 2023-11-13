using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public Animator animator;

	public void PlayGame()
	{
		Debug.Log("Play button pressed");
		animator.ResetTrigger("FadeIn");
		StartCoroutine(PlayGameSequence());
	}

	IEnumerator PlayGameSequence()
	{
		Debug.Log("Play game sequence started");
		animator.SetTrigger("FadeIn");

		yield return new WaitForSeconds(1.5f);
		Debug.Log("Loading transition");
		SceneManager.LoadScene("Stage1_transition");
	}

	public void ExitGame()
	{
		Debug.LogWarning("Exiting game");
		Application.Quit();
	}

	public void Instruction()
	{
		Debug.LogWarning("Loading instruction");
		SceneManager.LoadScene("Menu_Help");
	}

	public void MenuScene()
	{
		Debug.LogWarning("Loading menu");
		SceneManager.LoadScene("Menu");
	}
}
