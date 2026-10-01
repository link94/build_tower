using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class UVHeightGenerator : MonoBehaviour
{
	public bool makeAllMeshesUnique;

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
		if (this.makeAllMeshesUnique)
		{
			component.sharedMesh = UnityEngine.Object.Instantiate<Mesh>(component.sharedMesh);
			sharedMesh = component.sharedMesh;
		}
		Vector2[] array = new Vector2[sharedMesh.vertices.Length];
		float num = 3.40282347E+38f;
		float num2 = -3.40282347E+38f;
		for (int i = 0; i < sharedMesh.triangles.Length / 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				int num3 = sharedMesh.triangles[i * 3 + j];
				float y;
				if (t != null)
				{
					y = t.TransformPoint(sharedMesh.vertices[num3]).y;
				}
				else
				{
					y = sharedMesh.vertices[num3].y;
				}
				if (y < num)
				{
					num = y;
				}
				else if (y > num2)
				{
					num2 = y;
				}
			}
		}
		for (int k = 0; k < sharedMesh.triangles.Length / 3; k++)
		{
			for (int l = 0; l < 3; l++)
			{
				Vector3 vector = Vector3.zero;
				int num4 = sharedMesh.triangles[k * 3 + l];
				if (t != null)
				{
					vector = t.TransformPoint(sharedMesh.vertices[num4]);
				}
				else
				{
					vector = sharedMesh.vertices[num4];
				}
				array[num4].y = (vector.y - num) / Mathf.Abs(num2 - num);
			}
		}
		sharedMesh.uv3 = array;
	}

	public static Vector3 Vabs(Vector3 a)
	{
		return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
	}
}
