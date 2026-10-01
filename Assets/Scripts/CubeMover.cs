using System;
using UnityEngine;

[ExecuteInEditMode]
public class CubeMover : MonoBehaviour
{
	public float xSpeed;

	public float ySpeed;

	public float zSpeed;

	public float maxMovement = 10f;

	private Vector3 startPos = Vector3.zero;

	private bool forward = true;

	private void Start()
	{
		this.startPos = base.transform.position;
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		position.x += this.xSpeed * Time.deltaTime * (float)((!this.forward) ? (-1) : 1);
		position.y += this.ySpeed * Time.deltaTime * (float)((!this.forward) ? (-1) : 1);
		position.z += this.zSpeed * Time.deltaTime * (float)((!this.forward) ? (-1) : 1);
		base.transform.position = position;
		if (Vector3.Distance(this.startPos, position) >= this.maxMovement)
		{
			this.forward = !this.forward;
			this.startPos = base.transform.position;
		}
	}
}
