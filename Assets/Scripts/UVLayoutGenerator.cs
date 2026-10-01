using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class UVLayoutGenerator : MonoBehaviour
{
	public float xScale = 5f;

	public float yScale = 5f;

	public void GenerateUVs()
	{
		this.calculateUVs(base.transform, base.gameObject);
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				GameObject gameObject = transform.gameObject;
				this.calculateUVs(transform, gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void calculateUVs(Transform t, GameObject go)
	{
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component == null)
		{
			return;
		}
		Mesh sharedMesh = component.sharedMesh;
		if (sharedMesh == null)
		{
			return;
		}
		Vector2[] array = new Vector2[sharedMesh.vertices.Length];
		for (int i = 0; i < sharedMesh.triangles.Length / 3; i++)
		{
			Vector3[] array2 = new Vector3[3];
			int[] array3 = new int[3];
			Vector3 vector = default(Vector3);
			for (int j = 0; j < 3; j++)
			{
				int num = sharedMesh.triangles[i * 3 + j];
				array3[j] = num;
				if (t != null)
				{
					array2[j] = t.TransformPoint(sharedMesh.vertices[num]);
				}
				else
				{
					array2[j] = sharedMesh.vertices[num];
				}
				if (t != null)
				{
					vector += t.TransformDirection(sharedMesh.normals[num]);
				}
				else
				{
					vector += sharedMesh.normals[num];
				}
			}
			vector /= 3f;
			Quaternion rotation = Quaternion.FromToRotation(vector, new Vector3(0f, 0f, 1f));
			Vector3 lhs = UVLayoutGenerator.Vabs(rotation * Vector3.up);
			Vector3 lhs2 = rotation * Vector3.right;
			for (int k = 0; k < array2.Length; k++)
			{
				array[array3[k]].x = Vector3.Dot(lhs2, array2[k]) / this.xScale;
				array[array3[k]].y = Vector3.Dot(lhs, array2[k]) / this.yScale;
			}
		}
		sharedMesh.uv4 = array;
	}

	public static Vector3 Vabs(Vector3 a)
	{
		return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
	}
}
