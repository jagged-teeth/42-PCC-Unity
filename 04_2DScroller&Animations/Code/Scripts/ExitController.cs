using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitController : MonoBehaviour
{
	public TMP_Text exitTextPrefab;
	public Canvas canvas;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Character"))
			GameManager.Instance.CheckLevelCompletion();
	}

	public void ExitText()
	{
		Vector3 worldPosition = transform.position;

		Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

		Vector3 screenPosition = new Vector3(
			((viewportPosition.x * canvas.GetComponent<RectTransform>().rect.size.x) - (canvas.GetComponent<RectTransform>().rect.size.x * 0.5f)),
			((viewportPosition.y * canvas.GetComponent<RectTransform>().rect.size.y) - (canvas.GetComponent<RectTransform>().rect.size.y * 0.5f))
		);

		TMP_Text text = Instantiate(exitTextPrefab, canvas.transform);
		text.rectTransform.anchoredPosition = screenPosition;

		text.text = "I can't leave yet !";
	}
}
