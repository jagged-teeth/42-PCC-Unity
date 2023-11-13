using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject GetTurretInstance() { return turretInstance; }
	public Image turretButton;
	public GameObject turretPrefab;
	public CanvasGroup tileMapCanvasGroup;
	public EnergyManager energyManager;
	public float turretEnergyCost = 0.4f;
	public Color greyedOutColor = new Color(0f, 0f, 0f, 0.5f);
	public GameObject tooltipPrefab;
	public Vector3 tooltipOffset = new Vector2(50f, 50f);

	private GameObject tooltipInstance;
	private GameObject turretInstance;
	private Renderer turretShadowRenderer;
	private CanvasGroup canvasGroup;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		energyManager.OnEnergyChanged += UpdateButtonInteractable;
		UpdateButtonInteractable();
	}

	private void OnDestroy()
	{
		energyManager.OnEnergyChanged -= UpdateButtonInteractable;
	}

	private void UpdateButtonInteractable()
	{
		bool hasEnergy = energyManager.HasEnergy(turretEnergyCost);
		turretButton.color = hasEnergy ? Color.white : greyedOutColor;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (tileMapCanvasGroup != null) tileMapCanvasGroup.alpha = 0.3f;

		turretInstance = Instantiate(turretPrefab, eventData.position, Quaternion.identity);
		Transform shadowTransform = turretInstance.transform.Find("T_Shadow");
		canvasGroup.blocksRaycasts = false;

		if (shadowTransform != null)
		{
			turretShadowRenderer = shadowTransform.GetComponent<Renderer>();
			if (turretShadowRenderer != null)
				turretShadowRenderer.enabled = true;
			else
				Debug.LogWarning("Renderer component missing on the TurretShadow.");
		}
		else
		{
			Debug.LogWarning("TurretShadow sub-object not found.");
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (turretInstance != null)
		{
			Vector3 worldPosition;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(
				GetComponent<RectTransform>(),
				eventData.position,
				eventData.pressEventCamera,
				out worldPosition);
			turretInstance.transform.position = worldPosition;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (turretShadowRenderer != null)
		{
			turretShadowRenderer.enabled = false;
		}

		if (turretInstance != null)
		{
			Vector3 finalPosition = turretInstance.transform.position;
			turretInstance.transform.position = finalPosition;

			RaycastHit2D hit = Physics2D.Raycast(finalPosition, Vector2.zero);
			if (hit.collider != null && hit.collider.CompareTag("GridSlot"))
			{
				GridSlot gridSlot = hit.collider.GetComponent<GridSlot>();
				if (gridSlot != null && gridSlot.IsEmpty)
				{
					if (energyManager.HasEnergy(turretEnergyCost))
					{
						energyManager.ConsumeEnergy(turretEnergyCost);
						turretInstance.transform.position = hit.collider.transform.position;
						gridSlot.IsEmpty = false;

						finalPosition = turretInstance.transform.position;
						finalPosition.z = 0;
						turretInstance.transform.position = finalPosition;
					}
					else
					{
						Destroy(turretInstance);
						Debug.LogWarning("Not enough energy");
					}
				}
				else
				{
					Destroy(turretInstance);
				}
			}
			else Destroy(turretInstance);
		}
		canvasGroup.blocksRaycasts = true;
		turretInstance = null;
		if (tileMapCanvasGroup != null) tileMapCanvasGroup.alpha = 0f;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (tooltipPrefab != null)
		{
			Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position) + (Vector3)tooltipOffset;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			worldPosition.z = 0;

			tooltipInstance = Instantiate(tooltipPrefab, worldPosition, Quaternion.identity, transform);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (tooltipInstance != null)
		{
			Destroy(tooltipInstance);
		}
	}
}
