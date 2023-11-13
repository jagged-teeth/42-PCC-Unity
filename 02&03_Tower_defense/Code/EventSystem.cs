using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
	public static EventSystem current;

	private void Awake()
	{
		if (current == null)
			current = this;
		else
			Destroy(gameObject);
	}

	public event Action EnemyDestroyed;

	public void OnEnemyDestroyed()
	{
		EnemyDestroyed?.Invoke();
	}
}
