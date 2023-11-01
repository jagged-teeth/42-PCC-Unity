using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
	private Rigidbody2D rb;
	public float rotationSpeed = 400.0f;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.angularVelocity = 100.0f;
	}

	void Update()
	{
		transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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
