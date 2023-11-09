using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
	public void EnterState(PlayerController player)
	{
		player.animator.SetFloat("moveX", 0);
		player.animator.SetFloat("moveZ", 0);
		player.audioSource.Stop();
	}

	public void FixedUpdate(PlayerController player)
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");

		player.animator.SetFloat("moveX", Mathf.Abs(moveX));
		player.animator.SetFloat("moveZ", Mathf.Abs(moveZ));

		if (Mathf.Abs(moveX) >= 0.1f || Mathf.Abs(moveZ) >= 0.1f)
		{
			player.TransitionToState(player.runState);
		}
	}

	public void OnCollisionEnter(PlayerController player, Collision collision) { }
	public void ExitState(PlayerController player) { }
}
