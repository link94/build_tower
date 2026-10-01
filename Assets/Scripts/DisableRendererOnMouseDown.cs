using System;
using UnityEngine;

public class DisableRendererOnMouseDown : MonoBehaviour
{
	private void OnMouseDown()
	{
		base.GetComponent<Renderer>().enabled = !base.GetComponent<Renderer>().enabled;
	}
}
