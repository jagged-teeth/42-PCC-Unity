using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
	private float attackCooldown = 0.5f;
	private float timeSinceLastAttack = 0;

	public void EnterState(PlayerStateMachine player)
	{
		//Debug.Log("Entering Attack State");

		player.canChainAttack = false;
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.audioSources[0].clip = player.attackSoundClip;
		player.audioSources[0].loop = false;
		player.audioSources[0].Play();

		if (player.currentAttack >= 2)
			player.currentAttack = 0;

		player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
		player.rb.velocity = Vector2.zero;

		player.currentAttack++;
		timeSinceLastAttack = 0;
		player.animator.SetInteger("attackState", player.currentAttack);

		//Debug.Log("attackState: " + player.currentAttack);
	}

	public void Update(PlayerStateMachine player)
	{
		timeSinceLastAttack += Time.deltaTime;

		if (timeSinceLastAttack >= 0 && timeSinceLastAttack < attackCooldown)
			player.canChainAttack = true;
		else
		{
			player.currentAttack = 0;
			player.animator.SetInteger("attackState", 0);
			player.TransitionToState(new IdleState());
		}

		if (player.canChainAttack && Input.GetMouseButtonDown(0))
		{
			//Debug.Log("Chaining Attack");
			player.TransitionToState(new AttackState());
			return;
		}
	}

	public void OnCollisionEnter2D(PlayerStateMachine playerStateMachine, Collision2D collision) { }

	public void ExitState(PlayerStateMachine player)
	{
		//Debug.Log("Exiting Attack State");
		player.rb.constraints = RigidbodyConstraints2D.None;
		player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
