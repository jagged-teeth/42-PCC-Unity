using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAlign : MonoBehaviour
{
	public GameObject character;
	public float alignmentTolerance = 0.5f;
	public Color alignedColor = Color.green;
	public Color unalignedColor = Color.red;
	public bool IsAligned { get { return isAligned; }}

	private bool isAligned = false;
	private SpriteRenderer spriteRenderer;

	void Start()
	{
		if (character != null)
		{
			PlayerController playerController = character.GetComponent<PlayerController>();
			if (playerController != null)
			{
				playerController.OnAligned += HandleAlignment;
			}
		}

		spriteRenderer = GetComponent<SpriteRenderer>();

		if (spriteRenderer != null)
		{
			spriteRenderer.color = unalignedColor;
		}
	}

	void Update()
	{
		if (character != null)
		{
			float distance = Vector3.Distance(character.transform.position, transform.position);
			if (distance <= alignmentTolerance && !isAligned)
			{
				isAligned = true;
				HandleAlignment();
			}
			else if (distance > alignmentTolerance && isAligned)
			{
				isAligned = false;
				RevertAlignment();
			}
		}
	}

	void HandleAlignment()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = alignedColor;
		}
		Debug.Log(character.name + " Aligned!");
	}

	void RevertAlignment()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = unalignedColor;
		}
	}
}
