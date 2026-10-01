using System;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
	public float speed = 10f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, Time.deltaTime * this.speed, 0f), Space.World);
	}
}
