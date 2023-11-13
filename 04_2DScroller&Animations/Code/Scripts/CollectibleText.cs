using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleText : MonoBehaviour
{
	public Vector3 moveSpeed = new Vector3(0, 75, 0);
	public float timeToFade = 1.0f;
	RectTransform textTransform;
	TextMeshProUGUI text;
	private float timeElapsed = 0.0f;
	private Color startColor;

	private void Awake()
	{
		textTransform = GetComponent<RectTransform>();
		text = GetComponent<TextMeshProUGUI>();
		startColor = text.color;
	}

	private void Update()
	{
		float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));

		textTransform.position += moveSpeed * Time.deltaTime;
		timeElapsed += Time.deltaTime;

		if (timeElapsed < timeToFade)
			text.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
		else
			Destroy(gameObject);
	}
}
