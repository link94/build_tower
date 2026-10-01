using System;
using UnityEngine;

[ExecuteInEditMode]
public class EditModeGridSnap : MonoBehaviour
{
	public float snapValue = 0.5f;

	private bool snapX = true;

	private bool snapY = true;

	private bool snapZ = true;

	private void Update()
	{
		if (!Application.isPlaying)
		{
			float num = 1f / this.snapValue;
			float x;
			if (this.snapX)
			{
				x = Mathf.Round(base.transform.localPosition.x * num) / num;
			}
			else
			{
				x = base.transform.localPosition.x;
			}
			float y;
			if (this.snapY)
			{
				y = Mathf.Round(base.transform.localPosition.y * num) / num;
			}
			else
			{
				y = base.transform.localPosition.y;
			}
			float z;
			if (this.snapZ)
			{
				z = Mathf.Round(base.transform.localPosition.z * num) / num;
			}
			else
			{
				z = base.transform.localPosition.z;
			}
			base.transform.localPosition = new Vector3(x, y, z);
		}
	}
}
