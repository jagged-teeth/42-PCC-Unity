using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
	public float detectionRadius = 10f;
	public float fireRate = 1.0f;
	public float baseDamage = 1.0f;
	public GameObject projectilePrefab;
	public Transform firePoint;

	private float nextFireTime = 0f;

	void FixedUpdate()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
		float closestDistance = detectionRadius;
		Transform closestEnemy = null;

		foreach (Collider2D hit in hits)
		{
			if (hit.CompareTag("Enemy"))
			{
				float distance = Vector3.Distance(transform.position, hit.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestEnemy = hit.transform;
				}
			}
		}

		if (closestEnemy != null && Time.time > nextFireTime)
		{
			nextFireTime = Time.time + 1f / fireRate;
			Fire(closestEnemy);
		}
	}

	void Fire(Transform target)
	{
		if (projectilePrefab != null && firePoint != null)
		{
			GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
			Projectile projectile = projectileObject.GetComponent<Projectile>();
			if (projectile != null)
			{
				projectile.SetDamage(baseDamage);
				projectile.SetTarget(target);
			}
			else
				Debug.LogWarning("Projectile component missing on the projectile prefab.");
		}
	}
}
