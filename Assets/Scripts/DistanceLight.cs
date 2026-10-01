using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DistanceLight : MonoBehaviour
{
	public float maxDistance = 5f;

	public bool additiveBlending;

	public Texture rampTexture;

	private List<Material> materials = new List<Material>();

	private Texture lastRampTexture;

	private Vector3 lastPosition = Vector3.zero;

	private float lastDistance = 3.40282347E+38f;

	private bool lastBlending;

	private void Start()
	{
		this.findMaterials();
	}

	public void distanceLightChanged()
	{
		this.findMaterials();
	}

	private void checkMaterial(Material mat)
	{
		if (mat && !this.materials.Contains(mat) && mat.shader != null && mat.shader.name.Contains("Kirnu/Marvelous/") && (mat.shader.name.Contains("DistanceLight") || mat.shader.name.Contains("CustomLightingMaster")))
		{
			this.materials.Add(mat);
		}
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
					Material mat = sharedMaterials[j];
					this.checkMaterial(mat);
				}
			}
		}
		Terrain[] activeTerrains = Terrain.activeTerrains;
		Terrain[] array3 = activeTerrains;
		for (int k = 0; k < array3.Length; k++)
		{
			Terrain terrain = array3[k];
			Material materialTemplate = terrain.materialTemplate;
			this.checkMaterial(materialTemplate);
		}
		this.updateMaterials();
	}

	private void updateMaterials()
	{
		foreach (Material current in this.materials)
		{
			if (current && current.HasProperty("_LightPos") && this.lastPosition != base.transform.position)
			{
				current.SetVector("_LightPos", base.transform.position);
			}
			if (current && current.HasProperty("_LightMaxDistance") && this.lastDistance != this.maxDistance)
			{
				current.SetFloat("_LightMaxDistance", this.maxDistance);
			}
			if (current && current.HasProperty("_LightRampTexture") && this.rampTexture && this.lastRampTexture != this.rampTexture)
			{
				current.SetTexture("_LightRampTexture", this.rampTexture);
			}
			if (current && this.lastBlending != this.additiveBlending)
			{
				if (this.additiveBlending)
				{
					current.EnableKeyword("DIST_LIGHT_ADDITIVE");
				}
				else
				{
					current.DisableKeyword("DIST_LIGHT_ADDITIVE");
				}
			}
		}
		this.lastPosition = base.transform.position;
		this.lastDistance = this.maxDistance;
		this.lastRampTexture = this.rampTexture;
		this.lastBlending = this.additiveBlending;
	}

	private void Update()
	{
		this.updateMaterials();
	}
}
