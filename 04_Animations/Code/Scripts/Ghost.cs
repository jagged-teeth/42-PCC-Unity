using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	public GameObject spikeballPrefab;
	public AudioClip ghostThrowSoundClip;
	public float throwForce = 20.0f;
	public float throwCooldown = 5.0f;
	public float detectionRadius = 5.0f;
	public Vector2 facingDirection = Vector2.left;


	void Start()
	{
		StartCoroutine(ThrowSpikeballs());
	}

	IEnumerator ThrowSpikeballs()
	{
		while (true)
		{
			yield return new WaitForSeconds(throwCooldown);
			GameObject player = GameObject.FindWithTag("Character");
			if (player)
			{
				Vector2 toPlayer = (Vector2)(player.transform.position - transform.position);
				float angle = Vector2.Angle(facingDirection, toPlayer);
				if (angle <= 90.0f)
				{
					float distanceToPlayer = toPlayer.magnitude;

					if (distanceToPlayer <= detectionRadius)
					{
						Vector2 throwDirection = toPlayer.normalized;
						GameObject spikeball = Instantiate(spikeballPrefab, transform.position, Quaternion.identity);
						GetComponent<AudioSource>().PlayOneShot(ghostThrowSoundClip);
						Destroy(spikeball, 4f);
						Rigidbody2D rb = spikeball.GetComponent<Rigidbody2D>();
						rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
					}
				}
			}
		}
	}
}
