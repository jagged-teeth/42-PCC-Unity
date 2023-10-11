using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController_char : MonoBehaviour
{
	public GameObject[] platforms;
	public GameObject button;
	public GameObject buttonBase;
	public Vector3 buttonPressed;
	public float speed = 1.0f;

	private bool isSwitchActive = false;
	private Renderer buttonRenderer;
	private Renderer platformRenderer;
	private Renderer buttonBaseRenderer;
	private string characterLayer;

	void Start()
	{
		buttonRenderer = button.GetComponent<Renderer>();
		buttonBaseRenderer = buttonBase.GetComponent<Renderer>();
		//platformRenderer = platform.GetComponent<Renderer>();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("Claire") || other.CompareTag("John") ||
			other.CompareTag("Thomas")) && !isSwitchActive)
		{
			isSwitchActive = true;
			Material characterMaterial = other.gameObject.GetComponent<Renderer>().material;
			characterLayer = "Platform" + other.tag;

			buttonRenderer.material = characterMaterial;
			buttonBaseRenderer.material = characterMaterial;
			button.layer = LayerMask.NameToLayer(characterLayer);

			foreach (GameObject platform in platforms)
			{
				Renderer platformRenderer = platform.GetComponent<Renderer>();
				platformRenderer.material = characterMaterial;
				platform.layer = LayerMask.NameToLayer(characterLayer);
			}
		}
	}

	void Update()
	{
		if (isSwitchActive)
		{
			button.transform.localPosition = Vector3.MoveTowards(button.transform.localPosition, buttonPressed, speed * Time.deltaTime);
			if (button.transform.localPosition == buttonPressed)
				isSwitchActive = false;
		}
	}
}

