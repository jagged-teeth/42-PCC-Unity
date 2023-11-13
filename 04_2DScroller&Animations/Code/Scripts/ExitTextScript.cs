using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitTextScript : MonoBehaviour
{
	public float timeToFade = 3.0f;
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

		timeElapsed += Time.deltaTime;

		if (timeElapsed < timeToFade)
			text.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
		else
			Destroy(gameObject);
	}
}

