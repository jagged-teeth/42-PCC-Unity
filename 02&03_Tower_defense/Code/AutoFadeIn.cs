using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFadeIn : MonoBehaviour
{
	public Animator animator;
	void Start()
	{
		animator.SetTrigger("FadeIn");
	}

}
