using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform firePoint;
	public float fireRate = 1.0f;
	public float bulletSpeed = 10.0f;

	private float nextFireTime;

	void Update()
	{
		if (Time.time > nextFireTime)
		{
			FireBullet();
			nextFireTime = Time.time + fireRate;
		}
	}

	void FireBullet()
	{
		Quaternion bulletRotation = Quaternion.Euler(0, 0, 90);

		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

		Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
		bulletRigidbody.velocity = new Vector3(bulletSpeed * -1, 0, 0);
	}
}
