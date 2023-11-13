using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClingState : IPlayerState
{
	public void EnterState(PlayerStateMachine player)
	{
		Debug.Log("Entering Cling State");
		if (player.audioSources[0].isPlaying)
			player.audioSources[0].Stop();
		player.audioSources[0].clip = player.clingSoundClip;
		player.audioSources[0].loop = false;
		player.audioSources[0].Play();
		GameObject closestClingPoint = FindClosestClingPoint(player.transform.position);
		if (closestClingPoint != null)
			player.transform.position = closestClingPoint.transform.position;

		player.animator.SetBool("isCling", true);
		player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
		player.rb.velocity = Vector2.zero;
	}

	public void Update(PlayerStateMachine player)
	{
		if (Input.GetButtonDown("Jump"))
		{
			player.TransitionToState(new JumpState());
			return;
		}
		else
			return;
	}

	private GameObject FindClosestClingPoint(Vector2 playerPosition)
	{
		GameObject[] clingPoints = GameObject.FindGameObjectsWithTag("ClingPoint");
		GameObject closestPoint = null;
		float closestDistance = float.MaxValue;

		foreach (GameObject point in clingPoints)
		{
			float distance = Vector2.Distance(playerPosition, point.transform.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestPoint = point;
			}
		}

		return closestPoint;
	}
	public void OnCollisionEnter2D(PlayerStateMachine playerStateMachine, Collision2D collision) { }

	public void ExitState(PlayerStateMachine player)
	{
		Debug.Log("Exiting Cling State");
		player.rb.constraints = RigidbodyConstraints2D.None;
		player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		player.animator.SetBool("isCling", false);
	}
}
