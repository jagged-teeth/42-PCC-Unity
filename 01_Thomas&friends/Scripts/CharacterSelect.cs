using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
	public GameObject[] characters;
	private int activeCharIndex = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ChangeCharacter(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ChangeCharacter(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ChangeCharacter(2);
		}

		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private void ChangeCharacter(int index)
	{
		if (index < 0 || index >= characters.Length)
		{
			Debug.LogError("Character index out of range");
			return;
		}

		characters[activeCharIndex].GetComponent<PlayerController>().ToggleMovement(false);
		characters[index].GetComponent<PlayerController>().ToggleMovement(true);

		activeCharIndex = index;

		CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
		cameraFollow.target = characters[index].transform;
	}
}
