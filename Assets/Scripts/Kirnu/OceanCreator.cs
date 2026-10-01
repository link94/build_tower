using System;
using UnityEngine;

namespace Kirnu
{
	public class OceanCreator
	{
		private static Mesh createPlaneMesh(int widthSegments, int lengthSegments, float width, float length)
		{
			Mesh mesh = new Mesh();
			mesh.name = "OceanMesh";
			int num = widthSegments + 1;
			int num2 = lengthSegments + 1;
			int num3 = widthSegments * lengthSegments * 6;
			int num4 = num * num2;
			Vector3[] array = new Vector3[num4];
			Color[] array2 = new Color[num4];
			Vector2[] array3 = new Vector2[num4];
			int[] array4 = new int[num3];
			int num5 = 0;
			float num6 = 1f / (float)widthSegments;
			float num7 = 1f / (float)lengthSegments;
			float num8 = width / (float)widthSegments;
			float num9 = length / (float)lengthSegments;
			for (float num10 = 0f; num10 < (float)num2; num10 += 1f)
			{
				for (float num11 = 0f; num11 < (float)num; num11 += 1f)
				{
					float x = num11 * num8 - width / 2f;
					float z = num10 * num9 - length / 2f;
					float y = 0f;
					array2[num5] = new Color(UnityEngine.Random.Range(0f, 1f), 0f, 0f);
					array[num5] = new Vector3(x, y, z);
					array3[num5++] = new Vector2(num11 * num6, num10 * num7);
				}
			}
			num5 = 0;
			for (int i = 0; i < lengthSegments; i++)
			{
				for (int j = 0; j < widthSegments; j++)
				{
					array4[num5 + 2] = i * num + j;
					array4[num5] = (i + 1) * num + j;
					array4[num5 + 1] = i * num + j + 1;
					array4[num5 + 5] = (i + 1) * num + j;
					array4[num5 + 3] = (i + 1) * num + j + 1;
					array4[num5 + 4] = i * num + j + 1;
					num5 += 6;
				}
			}
			mesh.vertices = array;
			mesh.uv = array3;
			mesh.colors = array2;
			mesh.triangles = array4;
			mesh.RecalculateNormals();
			return mesh;
		}

		public static Mesh createOcean()
		{
			Mesh mesh = OceanCreator.createPlaneMesh(30, 30, 30f, 30f);
			Vector3[] array = new Vector3[mesh.triangles.Length];
			Color[] array2 = new Color[mesh.triangles.Length];
			Vector2[] array3 = new Vector2[array.Length];
			Vector3[] array4 = new Vector3[array.Length];
			int[] array5 = new int[mesh.triangles.Length];
			for (int i = 0; i < mesh.triangles.Length; i++)
			{
				int num = i;
				array[num] = mesh.vertices[mesh.triangles[num]];
				array3[num] = mesh.uv[mesh.triangles[num]];
				array2[num] = mesh.colors[mesh.triangles[num]];
				array4[num] = mesh.normals[mesh.triangles[num]];
				array5[num] = num;
			}
			mesh.vertices = array;
			mesh.colors = array2;
			mesh.normals = array4;
			mesh.triangles = array5;
			mesh.uv = array3;
			mesh.uv2 = array3;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return mesh;
		}
	}
}
