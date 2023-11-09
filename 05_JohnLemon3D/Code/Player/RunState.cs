using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RunState : IPlayerState
{
	public Vector3 moveDirection;

	public void EnterState(PlayerController player)
	{
		player.audioSource.loop = true;
		player.audioSource.clip = player.footstepSound;
		player.audioSource.Play();
	}

	public void FixedUpdate(PlayerController player)
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");

		moveDirection = player.orientation.forward * moveZ + player.orientation.right * moveX;

		if (moveDirection.magnitude > 0.1f)
		{
			moveDirection.Normalize();

			Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
			player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 7);

			player.rb.AddForce(moveDirection * player.speed * 10f, ForceMode.Force);

			player.animator.SetFloat("moveX", moveX);
			player.animator.SetFloat("moveZ", moveZ);
		}

		if (moveDirection == Vector3.zero)
		{
			player.TransitionToState(player.idleState);
		}
	}

	public void OnCollisionEnter(PlayerController player, Collision collision) { }
	public void ExitState(PlayerController player)
	{
		player.audioSource.Stop();
	}
}
