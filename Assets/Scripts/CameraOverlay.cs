using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraOverlay : MonoBehaviour
{
	private List<Transform> quads;

	private void Start()
	{
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		if (componentsInChildren.Length > 0)
		{
			Transform[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = array[i];
				if (transform != base.transform)
				{
					if (this.quads == null)
					{
						this.quads = new List<Transform>();
					}
					this.quads.Add(transform);
				}
			}
		}
		if (this.quads == null || !base.GetComponent<Camera>())
		{
			UnityEngine.Debug.Log("This script must be attached to Camera. Camera must have at least one Quad Transform as a child");
			return;
		}
	}

	private void Update()
	{
		if (base.GetComponent<Camera>() && this.quads != null)
		{
			if (base.GetComponent<Camera>().orthographic)
			{
				float num = (float)Screen.width / (float)Screen.height;
				float num2 = base.GetComponent<Camera>().orthographicSize * 2f;
				Vector3 vector = new Vector3(num2 * num, num2, 0f);
				foreach (Transform current in this.quads)
				{
					current.localScale = new Vector3(vector.x, vector.y, 1f);
				}
			}
			else
			{
				foreach (Transform current2 in this.quads)
				{
					float num3 = Vector3.Distance(current2.position, base.GetComponent<Camera>().transform.position);
					current2.LookAt(base.GetComponent<Camera>().transform);
					if (num3 < base.GetComponent<Camera>().nearClipPlane)
					{
						Vector3 translation = 1.1f * base.GetComponent<Camera>().nearClipPlane * current2.forward;
						current2.Translate(translation);
						num3 = Vector3.Distance(current2.position, base.GetComponent<Camera>().transform.position);
					}
					float num4 = Mathf.Tan(base.GetComponent<Camera>().fieldOfView * 0.0174532924f * 0.5f) * num3 * 2f;
					float x = num4 * base.GetComponent<Camera>().aspect;
					current2.localScale = new Vector3(x, num4, 1f);
				}
			}
		}
	}
}
