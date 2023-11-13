using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IPlayerState
{
	public void EnterState(PlayerStateMachine player)
	{
		//Debug.Log("Entering Run State");
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.audioSources[0].clip = player.runningSoundClip;
		player.audioSources[0].loop = true;
		player.audioSources[0].Play();
	}

	public void Update(PlayerStateMachine player)
	{
		float moveX = Input.GetAxis("Horizontal");

		if (moveX != 0)
		{
			player.lastMoveX = moveX;
			player.spriteRenderer.flipX = moveX < 0.0f;
		}

		if (player.isGrounded)
		{
			player.rb.velocity = new Vector2(moveX * player.speed, player.rb.velocity.y);
			player.animator.SetFloat("moveX", Mathf.Abs(moveX));
		}

		if (Mathf.Abs(moveX) <= 0.1f)
		{
			player.TransitionToState(new IdleState());
			return;
		}

		if (Input.GetButtonDown("Jump") && !player.isJumping && player.shouldJump)
		{
			player.TransitionToState(new JumpState());
			return;
		}

		if (Mathf.Abs(player.rb.velocity.y) > player.yVelocityThreshold)
		{
			player.TransitionToState(new FallState());
			return;
		}

		// Dash
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.TransitionToState(new DashState());
			return;
		}

		// Attack
		if (Input.GetMouseButtonDown(0) && player.currentAttack == 0)
		{
			player.TransitionToState(new AttackState());
		}
	}

	public void OnCollisionEnter2D(PlayerStateMachine playerStateMachine, Collision2D collision) { }

	public void ExitState(PlayerStateMachine player)
	{
		//Debug.Log("Exiting Run State");
	}
}
