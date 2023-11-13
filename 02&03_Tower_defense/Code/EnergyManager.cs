using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
	public Slider energyBar;
	public delegate void EnergyChangedEventHandler();
	public event EnergyChangedEventHandler OnEnergyChanged;
	public float energy = 0.0f;

	private float fillRate = 0.05f;

	void Update()
	{
		if (energy < 1)
		{
			energy += fillRate * Time.deltaTime;
			energyBar.value = energy;
			OnEnergyChanged?.Invoke();
		}
	}

	public bool ConsumeEnergy(float amount)
	{
		if (energy >= amount)
		{
			energy = Mathf.Clamp(energy - amount, 0, 1);
			energyBar.value = energy;
			OnEnergyChanged?.Invoke();
			return true;
		}
		return false;
	}

	public bool HasEnergy(float amount)
	{
		return energy >= amount;
	}
}
