using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IPlayerState
{
	private float dashSpeed = 15.0f;
	private float dashDistance = 3.0f;
	private Vector2 startPosition;
	private Vector2 endPosition;
	private Vector2 dashDirection;
	private LayerMask wallLayer = 1 << 7;

	public void EnterState(PlayerStateMachine player)
	{
		Debug.Log("Entering Dash State");
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.audioSources[0].clip = player.dashSoundClip;
		player.audioSources[0].loop = false;
		player.audioSources[0].Play();

		player.rb.velocity = Vector2.zero;
		startPosition = player.transform.position;

		dashDirection = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
		endPosition = startPosition + dashDirection * dashDistance;

		player.animator.SetBool("isDashing", true);
	}

	public void Update(PlayerStateMachine player)
	{
		Collider2D wallCheck = Physics2D.OverlapCircle(player.transform.position, 0.1f, wallLayer);

		if (Vector2.Distance(player.transform.position, endPosition) <= 0.1f || wallCheck)
		{
			if (player.isGrounded)
				player.TransitionToState(new IdleState());
			else
			{
				player.rb.velocity = new Vector2(dashSpeed * (dashDirection.x > 0 ? 1 : -1), player.rb.velocity.y);
				player.TransitionToState(new FallState());
			}
			return;
		}
		else if (player.isGrounded)
		{
			player.transform.position = Vector2.MoveTowards(player.transform.position, endPosition, dashSpeed * Time.deltaTime);
		}
		else
		{
			player.canAirDash = false;
			player.transform.position = Vector2.MoveTowards(player.transform.position, endPosition, dashSpeed * Time.deltaTime);
		}
	}

	public void OnCollisionEnter2D(PlayerStateMachine playerStateMachine, Collision2D collision) { }

	public void ExitState(PlayerStateMachine player)
	{
		Debug.Log("Exiting Dash State");
		player.animator.SetBool("isDashing", false);
	}
}
