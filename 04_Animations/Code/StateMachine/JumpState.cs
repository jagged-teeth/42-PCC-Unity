using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerState
{
	private Coroutine jumpRoutine;
	private float currentJumpVelocity;
	public void EnterState(PlayerStateMachine player)
	{
		//Debug.Log("Entering Jump State");
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.audioSources[0].clip = player.jumpSoundClip;
		player.audioSources[0].loop = false;
		player.audioSources[0].Play();

		player.isJumping = true;
		player.shouldJump = false;
		player.animator.SetBool("isJumping", true);

		jumpRoutine = player.StartCoroutine(JumpRoutine(player));
	}

	public void Update(PlayerStateMachine player)
	{
		float moveX = Input.GetAxis("Horizontal");

		if (moveX != 0)
			player.spriteRenderer.flipX = moveX < 0.0f;


		// Air control
		float targetVelocityX = moveX * player.speed;
		float newVelocityX = Mathf.Lerp(player.rb.velocity.x, targetVelocityX, player.airControlFactor * Time.deltaTime);

		player.rb.velocity = new Vector2(newVelocityX, player.rb.velocity.y);

		if (player.rb.velocity.y < 0)
			player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.fallMultiplier - 1) * Time.deltaTime;
		else if (player.rb.velocity.y > 0 && !Input.GetButton("Jump"))
			player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.floatVelocity - 1) * Time.deltaTime;

		// Dash
		if (Input.GetKeyDown(KeyCode.LeftShift) && player.canAirDash)
		{
			player.TransitionToState(new DashState());
			return;
		}
	}

	public void OnCollisionEnter2D(PlayerStateMachine player, Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			player.audioSources[1].clip = player.landSoundClip;
			player.audioSources[1].loop = false;
			player.audioSources[1].Play();
			ExitState(player);
			player.TransitionToState(new IdleState());
		}
	}

	private IEnumerator JumpRoutine(PlayerStateMachine player)
	{
		currentJumpVelocity = player.minJumpVelocity;
		player.rb.velocity = new Vector2(player.rb.velocity.x, currentJumpVelocity);

		while (Input.GetButton("Jump") && currentJumpVelocity < player.maxJumpVelocity)
		{
			currentJumpVelocity = Mathf.MoveTowards(currentJumpVelocity, player.maxJumpVelocity, Time.deltaTime * 10);
			player.rb.velocity = new Vector2(player.rb.velocity.x, currentJumpVelocity);
			yield return null;
		}
	}

	public void ExitState(PlayerStateMachine player)
	{
		if (jumpRoutine != null)
			player.StopCoroutine(jumpRoutine);
		player.animator.SetBool("isJumping", false);
		player.isJumping = false;
		player.shouldJump = true;
		//Debug.Log("Exiting Jump State");
	}
}
