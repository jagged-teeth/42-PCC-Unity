using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{

	public void EnterState(PlayerStateMachine player)
	{
		//Debug.Log("Entering Idle State");
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.animator.SetBool("isJumping", false);
		player.animator.SetBool("isFalling", false);
		player.animator.SetFloat("moveX", 0);
	}

	public void Update(PlayerStateMachine player)
	{
		float moveX = Input.GetAxis("Horizontal");


		player.animator.SetFloat("moveX", Mathf.Abs(moveX));

		// Flip Sprite
		if (moveX != 0)
		{
			player.lastMoveX = moveX;
			player.spriteRenderer.flipX = moveX < 0.0f;
		}
		else
			player.spriteRenderer.flipX = player.lastMoveX < 0.0f;

		// Run
		if (Mathf.Abs(moveX) >= 0.1f)
		{
			player.TransitionToState(new RunState());
			return;
		}

		// Jump
		if (Input.GetButtonDown("Jump") && !player.isJumping && player.shouldJump)
		{
			player.TransitionToState(new JumpState());
			return;
		}

		// Fall
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
		//Debug.Log("Exiting Idle State");
	}
}
