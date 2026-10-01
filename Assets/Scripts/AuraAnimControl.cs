using System;
using System.Collections.Generic;
using UnityEngine;

public class AuraAnimControl : MonoBehaviour
{
	[SerializeField]
	public List<AnimationClip> anim;

	[SerializeField]
	private Animator animator;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void PlayAnim(int ID)
	{
		this.animator.Play(this.anim[ID].name);
	}
}
