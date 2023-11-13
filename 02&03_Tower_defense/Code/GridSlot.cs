using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler
{
	public bool IsEmpty { get; internal set; } = true;

	public void OnDrop(PointerEventData eventData)
	{
		if (IsEmpty)
		{
			TurretDragDrop turretDragDrop = eventData.pointerDrag.GetComponent<TurretDragDrop>();
			if (turretDragDrop != null)
			{
				GameObject turretInstance = turretDragDrop.GetTurretInstance();

				if (turretInstance != null)
				{
					turretInstance.transform.position = transform.position;
					//IsEmpty = false;
				}
			}
		}
		else
		{
			Debug.Log("Slot is occupied.");
		}
	}

	public void ResetSlot()
	{
		IsEmpty = true;
	}
}
