using System;
using UnityEngine;

public class BackgroundTube : MonoBehaviour
{
	[SerializeField]
	private Renderer rend;

	[SerializeField]
	private GameObject parent;

	private void Start()
	{
	}

	private void Update()
	{
		if (Camera.main.transform.position.y - base.transform.position.y > 20f)
		{
			BackgroundGenerator.instance.CreateTube();
			UnityEngine.Object.Destroy(this.parent);
		}
	}
}
