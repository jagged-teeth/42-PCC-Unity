using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
	[SerializeField] Transform target;
	public float noiseScale = 1f;
	public float noiseStrength = 10f;
	public float health = 3.0f;

	private NavMeshAgent agent;
	private float time;
	private Vector3 originalDestination;

	private void Start()
	{
		if (target == null)
		{
			GameObject targetGameObject = GameObject.FindWithTag("Target");
			if (targetGameObject != null)
				target = targetGameObject.transform;
			else
				Debug.LogError("Target GameObject with tag 'Target' not found.");
		}
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		time = Random.Range(0f, 10f);

		if (target != null)
			originalDestination = target.position;
		else
			Debug.LogError("Target is null. Original destination cannot be set.");
	}

	private void Update()
	{
		float noiseX = Mathf.PerlinNoise(time, 0f) * 2f - 1f;
		float noiseZ = Mathf.PerlinNoise(0f, time) * 2f - 1f;
		Vector3 offset = new Vector3(noiseX, 0f, noiseZ) * noiseStrength;
		Vector3 newDestination = originalDestination + offset;
		agent.SetDestination(newDestination);
		time += Time.deltaTime * noiseScale;
	}

	public void ReceiveDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Destroy(gameObject);
			EventSystem.current.OnEnemyDestroyed();
		}
	}
}
