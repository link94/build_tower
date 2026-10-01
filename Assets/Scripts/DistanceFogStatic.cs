using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DistanceFogStatic : MonoBehaviour
{
	private List<Material> materials = new List<Material>();

	private void Start()
	{
		this.findMaterials();
		this.updateMaterials();
	}

	private void findMaterials()
	{
		this.materials.Clear();
		Renderer[] array = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
		Renderer[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Renderer renderer = array2[i];
			if (renderer != null)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					Material material = sharedMaterials[j];
					if (material && !this.materials.Contains(material) && material.name.Contains("DistanceFogStatic"))
					{
						this.materials.Add(material);
					}
				}
			}
		}
	}

	private void updateMaterials()
	{
		foreach (Material current in this.materials)
		{
			if (current)
			{
				current.SetVector("_FogStaticStartPos", new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 0f));
			}
		}
	}

	private void Update()
	{
		this.updateMaterials();
	}
}
