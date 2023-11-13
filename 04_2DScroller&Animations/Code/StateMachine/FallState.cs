using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IPlayerState
{
	public void EnterState(PlayerStateMachine player)
	{
		//Debug.Log("Entering Fall State");
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.animator.SetBool("isFalling", true);

		if (player.previousState is DashState)
		{
			float retainedVelocityX = player.rb.velocity.x / 2.0f;
			player.rb.velocity = new Vector2(retainedVelocityX, player.rb.velocity.y);

		}
		else
			player.rb.velocity = new Vector2(0, player.rb.velocity.y);
	}

	public void Update(PlayerStateMachine player)
	{
		float moveX = Input.GetAxis("Horizontal");

		float targetVelocityX = moveX * player.speed;
		float newVelocityX = Mathf.Lerp(player.rb.velocity.x, targetVelocityX, player.airControlFactor * Time.deltaTime);

		player.rb.velocity = new Vector2(newVelocityX, player.rb.velocity.y);

		if (Mathf.Abs(player.rb.velocity.y) < player.yVelocityThreshold)
		{
			if (player.lastMoveX != 0f)
				player.TransitionToState(new RunState());
			else
				player.TransitionToState(new IdleState());
		}

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
			if (player.lastMoveX != 0f)
				player.TransitionToState(new RunState());
			else
				player.TransitionToState(new IdleState());
		}
	}

	public void ExitState(PlayerStateMachine player)
	{
		//Debug.Log("Exiting Fall State");
		player.animator.SetBool("isFalling", false);
	}

}
