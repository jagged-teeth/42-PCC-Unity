using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{
	public string uniqueID;
	public int collectibleValue = 5;
	public TMP_Text collectTextPrefab;
	public Canvas canvas;
	public Vector3 spinRotationSpeed = new Vector3(0, 100, 0);

	void Start() { }
	void Awake()
	{
		uniqueID = gameObject.name;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Character"))
		{
			GameManager.Instance.Collect(uniqueID, collectibleValue);
			CollectText(collectibleValue);
			Destroy(gameObject);
		}
	}

	public void CollectText(int value)
	{
		Vector3 worldPosition = transform.position;

		Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

		Vector3 screenPosition = new Vector3(
			((viewportPosition.x * canvas.GetComponent<RectTransform>().rect.size.x) - (canvas.GetComponent<RectTransform>().rect.size.x * 0.5f)),
			((viewportPosition.y * canvas.GetComponent<RectTransform>().rect.size.y) - (canvas.GetComponent<RectTransform>().rect.size.y * 0.5f))
		);

		TMP_Text text = Instantiate(collectTextPrefab, canvas.transform);
		text.rectTransform.anchoredPosition = screenPosition;

		text.text = "+" + value.ToString();
	}

	private void Update()
	{
		transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
	}
}
