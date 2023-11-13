using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed = 10f;
	public float maxDistance = 50f;

	private Transform target;
	private Vector3 direction;
	private Vector3 startPosition;
	private float damage;

	public void Start()
	{
		startPosition = transform.position;
	}

	public void SetDamage(float damageValue)
	{
		damage = damageValue;
	}

	public void SetTarget(Transform newTarget)
	{
		if (newTarget != null)
			target = newTarget;
		else
			Debug.LogWarning("Target is null. Projectile will not move.");
	}

	void Update()
	{
		if (target != null)
			direction = (target.position - transform.position).normalized;
		transform.position += direction * speed * Time.deltaTime;

		float distanceTraveled = Vector3.Distance(startPosition, transform.position);
		if (distanceTraveled > maxDistance)
			Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		NavigationScript enemy = collision.gameObject.GetComponent<NavigationScript>();
		if (enemy != null)
		{
			enemy.ReceiveDamage(damage);
			Destroy(gameObject);
		}
	}
}
