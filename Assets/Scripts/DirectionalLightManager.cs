using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Transform))]
public class DirectionalLightManager : MonoBehaviour
{
	private List<Material> materials = new List<Material>();

	private Vector3 lastUpVector = Vector3.zero;

	private Vector3 lastRightVector = Vector3.zero;

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
					if (material && !this.materials.Contains(material) && material.shader != null && material.shader.name.Contains("Kirnu/Marvelous/") && material.shader.name.Contains("CustomLighting"))
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
			if (current && current.HasProperty("_LightDirT") && current.IsKeywordEnabled("USE_DIR_LIGHT") && (this.lastUpVector != base.transform.up || this.lastRightVector != base.transform.right))
			{
				current.SetVector("_LightDirF", base.transform.forward);
				current.SetVector("_LightDirR", -base.transform.right);
				current.SetVector("_LightDirT", -base.transform.up);
			}
		}
		this.lastUpVector = base.transform.up;
		this.lastRightVector = base.transform.right;
	}

	private void Update()
	{
		this.updateMaterials();
	}
}
