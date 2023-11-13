using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Material glowMaterial;
	public Material originalMaterial;
	public Image buttonImage;
	public HoverGlowText hoverGlowText;

	private void Start()
	{
		if (originalMaterial == null)
			originalMaterial = new Material(Shader.Find("UI/Default"));
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (buttonImage != null) buttonImage.material = glowMaterial;
		if (hoverGlowText != null) hoverGlowText.OnPointerEnter(eventData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (buttonImage != null) buttonImage.material = originalMaterial;
		if (hoverGlowText != null) hoverGlowText.OnPointerExit(eventData);
	}
}
