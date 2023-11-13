using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
	public Animator animator { get; private set; }
	public AudioClip blobJumpSoundClip;
	public float jumpForce = 5.0f;
	public float yAxisBias = 1.0f;
	public float jumpCooldown = 4.0f;
	private Rigidbody2D rb;

	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine(Jump());
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Character"))
		{
			PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();
			if (player.CanTakeDamage())
				StartCoroutine(HandleDamageTrigger(player));
		}
	}

	IEnumerator Jump()
	{
		while (true)
		{
			yield return new WaitForSeconds(jumpCooldown);
			GameObject player = GameObject.FindWithTag("Character");
			if (player)
			{
				animator.SetTrigger("isJumping");
				Vector2 direction = (player.transform.position - transform.position).normalized;
				GetComponent<AudioSource>().PlayOneShot(blobJumpSoundClip);
				direction.y += yAxisBias;
				direction = direction.normalized;
				rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
			}
		}
	}

	IEnumerator HandleDamageTrigger(PlayerStateMachine player)
	{
		if (player.health > 1)
		{
			player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
			player.rb.velocity = Vector2.zero;
			player.animator.SetTrigger("isDamaged");
			player.TakeDamage(1);
			yield return new WaitForSeconds(0.4f);
		}
		else if (player.health == 1)
		{
			player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
			player.rb.velocity = Vector2.zero;
			player.animator.SetTrigger("isDead");
			player.TakeDamage(1);
			yield return new WaitForSeconds(2.4f);

			GameManager.Instance.SetPlayerHealth(5);
			//player.health = 5;

			player.transform.position = player.respawnPoint;
			player.audioSources[1].clip = player.respawnSoundClip;
			player.audioSources[1].loop = false;
			player.audioSources[1].Play();
		}

		player.rb.constraints = RigidbodyConstraints2D.None;
		player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		player.animator.ResetTrigger("isDamaged");
		player.animator.ResetTrigger("isDead");
		player.animator.ResetTrigger("isRespawn");
	}
}
