using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public interface IPlayerState
{
	void EnterState(PlayerStateMachine player);
	void Update(PlayerStateMachine player);
	void ExitState(PlayerStateMachine player);
	void OnCollisionEnter2D(PlayerStateMachine player, Collision2D collision);
}

public class PlayerStateMachine : MonoBehaviour
{
	public AudioSource[] audioSources;
	public AudioClip runningSoundClip;
	public AudioClip jumpSoundClip;
	public AudioClip dashSoundClip;
	public AudioClip landSoundClip;
	public AudioClip clingSoundClip;
	public AudioClip damageSoundClip;
	public AudioClip deathSoundClip;
	public AudioClip attackSoundClip;
	public AudioClip respawnSoundClip;

	public CanvasGroup gameOverCanvasGroup;
	public TextMeshProUGUI gameOverText;
	public PolygonCollider2D polygonColliderLeft;
	public PolygonCollider2D polygonColliderRight;
	public IPlayerState currentState { get; private set; }
	public IPlayerState previousState { get; private set; }
	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }
	public SpriteRenderer spriteRenderer { get; private set; }
	public Vector2 respawnPoint;

	public int playerLives;
	public int health;
	public float damageCooldown = 2.0f;
	public float lastDamageTime = 0.0f;
	public float stopTime = 0.0f;
	public bool isJumping = false;
	public bool shouldJump = true;
	public bool isGrounded = true;
	public bool canAirDash = true;
	public bool canChainAttack = false;
	public int currentAttack = 0;
	public float speed = 5.0f;
	public float maxJumpVelocity = 8.0f;
	public float minJumpVelocity = 4.0f;
	public float floatVelocity = 2.0f;
	public float fallMultiplier = 1.5f;
	public float airControlFactor = 0.5f;
	public float yVelocityThreshold = 0.3f;
	internal float lastMoveX = 0f;

	private bool wasFlipped;
	private float currentJumpVelocity;

	private void Awake()
	{
		playerLives = GameManager.Instance.GetPlayerLives();
		health = GameManager.Instance.GetPlayerHealth();

		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateCollider();

		wasFlipped = spriteRenderer.flipX;

		currentState = new IdleState();
	}

	private void Start()
	{
		currentState.EnterState(this);
	}

	private void Update()
	{
		currentState.Update(this);

		if (wasFlipped != spriteRenderer.flipX)
		{
			UpdateCollider();
			wasFlipped = spriteRenderer.flipX;
		}
	}

	private void UpdateCollider()
	{
		polygonColliderLeft.enabled = !spriteRenderer.flipX;
		polygonColliderRight.enabled = spriteRenderer.flipX;
	}

	public void TransitionToState(IPlayerState newState)
	{
		if (currentState != null)
			currentState.ExitState(this);

		previousState = currentState;
		currentState = newState;
		currentState.EnterState(this);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		currentState.OnCollisionEnter2D(this, collision);

		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
			canAirDash = true;
		}

		if (collision.gameObject.CompareTag("Cliff"))
			TransitionToState(new ClingState());
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
			isGrounded = false;
	}

	public bool IsDashState()
	{
		return currentState is DashState;
	}

	public bool CanTakeDamage()
	{
		if (IsDashState())
			return false;
		if (Time.time - lastDamageTime >= damageCooldown)
		{
			lastDamageTime = Time.time;
			return true;
		}
		return false;
	}

	public void TakeDamage(int amount)
	{
		audioSources[1].clip = damageSoundClip;
		audioSources[1].loop = false;
		audioSources[1].Play();

		health -= amount;
		GameManager.Instance.SetPlayerHealth(health);

		if (health <= 0)
		{
			audioSources[1].clip = deathSoundClip;
			audioSources[1].loop = false;
			audioSources[1].Play();

			Debug.Log("Player died");
			playerLives--;
			GameManager.Instance.SetPlayerLives(playerLives);
			if (playerLives == 0)
				StartCoroutine(FadeToGameOver());
		}
	}

	private IEnumerator FadeToGameOver()
	{
		audioSources[0].Stop();
		audioSources[1].Stop();
		audioSources[2].Stop();

		float fadeDuration = 1.0f;
		float startTime = Time.time;

		while (Time.time < startTime + fadeDuration)
		{
			float t = (Time.time - startTime) / fadeDuration;
			gameOverCanvasGroup.alpha = t;
			yield return null;
		}

		gameOverCanvasGroup.alpha = 1.0f;
		gameOverText.alpha = 1.0f;

		yield return new WaitForEndOfFrame();

		while (!Input.anyKeyDown)
		{
			yield return null;
		}

		GameManager.Instance.ResetGame();
		SceneManager.LoadScene("Menu");
	}
}
