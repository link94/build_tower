using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kirnu
{
	[ExecuteInEditMode]
	public class OceanWaver : MonoBehaviour
	{
		public List<Transform> floatingObjects = new List<Transform>();

		public float waveHeight = 0.5f;

		public float waveSpeed = 1f;

		public float yPower = 0.1f;

		public float xPower = 0.1f;

		public float zPower = 0.1f;

		private Vector3[] vertices;

		private Vector3[] normals;

		private Color[] speeds;

		private Vector3[] newVertices;

		public Mesh mesh;

		private Mesh newMesh;

		private List<FloatingObject> internalFloatingObjects = new List<FloatingObject>();

		private void preCalculateFloatingObjects()
		{
			foreach (Transform current in this.floatingObjects)
			{
				FloatingObject floatingObject = new FloatingObject();
				floatingObject.t = current;
				for (int i = 0; i < this.vertices.Length; i++)
				{
					if (Vector3.Distance(this.vertices[i], current.position) <= 1.1f)
					{
						floatingObject.vertices.Add(i);
					}
				}
				this.internalFloatingObjects.Add(floatingObject);
			}
		}

		private void calculateFloatingObjects()
		{
			foreach (FloatingObject current in this.internalFloatingObjects)
			{
				if (current.vertices.Count != 0)
				{
					float num = 0f;
					Vector3 a = Vector3.zero;
					for (int i = 0; i < current.vertices.Count; i++)
					{
						num += this.newVertices[current.vertices[i]].y;
						a += this.normals[current.vertices[i]];
					}
					Vector3 position = current.t.position;
					position.y = num / (float)current.vertices.Count;
					current.t.position = position;
					current.t.transform.up = a / (float)current.vertices.Count;
				}
			}
		}

		private void Start()
		{
			if (this.mesh)
			{
				this.newMesh = UnityEngine.Object.Instantiate<Mesh>(this.mesh, Vector3.zero, Quaternion.identity);
				base.GetComponent<MeshFilter>().mesh = this.newMesh;
				this.vertices = this.mesh.vertices;
				this.normals = this.mesh.normals;
				this.newVertices = new Vector3[this.vertices.Length];
				this.speeds = this.mesh.colors;
			}
			this.preCalculateFloatingObjects();
		}

		private void Update()
		{
			if (this.newMesh)
			{
				for (int i = 0; i < this.newVertices.Length; i++)
				{
					Vector3 vector = this.vertices[i];
					float num = (vector.x + 0.5f * vector.z) * this.xPower + vector.z * this.zPower;
					float num2 = this.speeds[i].r * this.yPower;
					vector.y += Mathf.Sin(Time.time * this.waveSpeed + num2 + num) * this.waveHeight;
					this.newVertices[i] = vector;
				}
				this.newMesh.vertices = this.newVertices;
				this.newMesh.RecalculateNormals();
				this.normals = this.newMesh.normals;
				this.calculateFloatingObjects();
			}
		}
	}
}
