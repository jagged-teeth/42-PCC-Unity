using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverGlowText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public TextMeshProUGUI buttonText;
	public Material glowMaterial;
	public Material originalMaterial;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (buttonText != null && glowMaterial != null)
		{
			buttonText.fontMaterial = glowMaterial;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (buttonText != null && originalMaterial != null)
		{
			buttonText.fontMaterial = originalMaterial;
		}
	}
}
