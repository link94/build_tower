using System;
using UnityEngine;

[ExecuteInEditMode]
public class EnableCameraDepthTexture : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}
}
