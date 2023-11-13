using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFadeOut : MonoBehaviour
{
	public Animator animator;
	void Start()
	{
		animator.SetTrigger("FadeOut");
	}
}
