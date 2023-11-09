using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
	public Transform player;
	public Transform[] patrolPoints;
	public float chaseTime = 4f;
	public float detectionRadius = 10f;

	private GameController gameController;
	private NavMeshAgent agent;
	private int destPoint = 0;
	private bool chasing = false;
	private float chaseTimer;

	void Awake()
	{
		gameController = FindObjectOfType<GameController>();
		if (gameController == null)
			Debug.LogError("GameController not found in the scene!");
	}

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.autoBraking = false;

		GotoNextPoint();
	}

	void GotoNextPoint()
	{
		if (patrolPoints.Length == 0)
			return;

		agent.destination = patrolPoints[destPoint].position;
		destPoint = (destPoint + 1) % patrolPoints.Length;
	}

	void FixedUpdate()
	{
		if (!agent.pathPending && !chasing && agent.remainingDistance < 0.5f)
			GotoNextPoint();

		if (player != null)
		{
			Vector3 directionToPlayer = player.position - transform.position;
			float angle = Vector3.Angle(directionToPlayer, transform.forward);
			float distanceToPlayer = directionToPlayer.magnitude;

			if (distanceToPlayer < detectionRadius && angle < 45.0f)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRadius))
				{
					if (hit.transform == player)
					{
						chasing = true;
						chaseTimer = chaseTime;
						agent.destination = player.position;
					}
				}
			}
		}

		if (chasing)
		{
			if (chaseTimer > 0)
			{
				chaseTimer -= Time.deltaTime;
				agent.destination = player.position;
			}
			else
			{
				chasing = false;
				GotoNextPoint();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			gameController.PlayerDied();
		}
	}

	public void Alerted(Vector3 playerPosition)
	{
		chasing = true;
		chaseTimer = chaseTime;
		agent.destination = playerPosition;

		if (chaseTimer > 0)
		{
			chaseTimer -= Time.deltaTime;
			agent.destination = playerPosition;
		}
		else
		{
			chasing = false;
			GotoNextPoint();
		}
	}
}
