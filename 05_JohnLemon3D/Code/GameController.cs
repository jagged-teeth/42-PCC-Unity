using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{
	public static GameController Instance;
	public GameObject FPScam;
	public CinemachineFreeLook TPScam;
	public FPScamera fpsCameraScript;
	public TPScamera tpsCameraScript;
	public Animator levelCompleteAnimator;
	public Animator dieAnimator;
	public CanvasGroup respawnCanvasGroup;
	public Animator respawnAnimator;
	public AudioSource audioSource;
	public AudioClip dieAudioClip;
	public AudioClip levelCompleteAudioClip;
	public AudioClip ambientSound;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	void Start()
	{
		audioSource.Stop();
		audioSource.loop = true;
		audioSource.clip = ambientSound;
		audioSource.Play();

		respawnCanvasGroup.gameObject.SetActive(false);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		ReacquireReferences();

		FPScam.gameObject.SetActive(true);
		fpsCameraScript.enabled = true;

		TPScam.gameObject.SetActive(false);
		TPScam.Priority = 9;
		tpsCameraScript.enabled = false;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != "LevelComplete")
		{
			ReacquireReferences();
			if (respawnCanvasGroup != null)
			{
				respawnCanvasGroup.gameObject.SetActive(true);
				respawnCanvasGroup.alpha = 1f;
			}
			if (respawnAnimator != null)
				respawnAnimator.SetTrigger("Respawn");
		}
	}

	void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void ReacquireReferences()
	{
		GameObject camerasContainer = GameObject.Find("Cameras");
		GameObject uiCanvas = GameObject.Find("UICanvas");

		if (camerasContainer != null)
		{
			FPScam = camerasContainer.transform.Find("FPSCamera").gameObject;
			TPScam = camerasContainer.transform.Find("TPSCamera/FreeLook Camera").GetComponent<CinemachineFreeLook>();

			if (FPScam != null)
				fpsCameraScript = FPScam.GetComponent<FPScamera>();
			else
				Debug.LogError("FPS Camera Script Not Found");

			if (TPScam != null)
				tpsCameraScript = TPScam.transform.parent.GetComponent<TPScamera>();
			else
				Debug.LogError("TPS Camera Script Not Found");
		}
		else
			Debug.LogError("Cameras Container Not Found");

		if (uiCanvas != null)
		{
			levelCompleteAnimator = uiCanvas.transform.Find("FadeToBlack").GetComponent<Animator>();
			if (levelCompleteAnimator == null)
				Debug.LogError("Level Complete Animator Not Found");

			dieAnimator = uiCanvas.transform.Find("Die").GetComponent<Animator>();
			if (dieAnimator == null)
				Debug.LogError("Die Animator Not Found");

			respawnAnimator = uiCanvas.transform.Find("Respawn").GetComponent<Animator>();
			if (respawnAnimator == null)
				Debug.LogError("Respawn Animator Not Found");

			respawnCanvasGroup = uiCanvas.transform.Find("Respawn").GetComponent<CanvasGroup>();
			if (respawnCanvasGroup == null)
				Debug.LogError("Respawn Canvas Group Not Found");
		}
		else
			Debug.LogError("UI Canvas Not Found");
	}

	public void ShowCursor()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void Update()
	{
		// placeholder for now
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		if (Input.GetKeyDown(KeyCode.Tab))
			SwitchCamera();
	}

	private void SwitchCamera()
	{
		if (FPScam.gameObject.activeInHierarchy)
			SwitchToTPS();
		else
			SwitchToFPS();
	}

	private void SwitchToFPS()
	{
		Vector3 playerForward = tpsCameraScript.playerObj.forward;
		float newYRotation = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;
		fpsCameraScript.yRotation = newYRotation;

		FPScam.gameObject.SetActive(true);
		fpsCameraScript.enabled = true;

		TPScam.gameObject.SetActive(false);
		TPScam.Priority = 9;
		tpsCameraScript.enabled = false;
	}

	private void SwitchToTPS()
	{
		FPScam.gameObject.SetActive(false);
		fpsCameraScript.enabled = false;

		TPScam.gameObject.SetActive(true);
		TPScam.Priority = 11;
		tpsCameraScript.enabled = true;
	}

	public void LevelComplete()
	{
		audioSource.PlayOneShot(levelCompleteAudioClip);

		if (FPScam != null && FPScam.activeInHierarchy)
			fpsCameraScript.enabled = false;
		ShowCursor();
		levelCompleteAnimator.SetTrigger("LevelComplete");
		Invoke("LoadLevelCompleteScene", 1f);
		Debug.Log("Congratulations! You've completed the level!");
	}

	private void LoadLevelCompleteScene() => SceneManager.LoadScene("LevelComplete");

	public void PlayerDied()
	{
		audioSource.PlayOneShot(dieAudioClip);

		dieAnimator.SetTrigger("Die");
		Invoke("ReloadScene", 1f);
		Debug.Log("You died!");
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene("Stage1");
	}
}
