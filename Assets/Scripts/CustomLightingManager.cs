using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomLightingManager : MonoBehaviour
{
	public Color ambientColor = Color.white;

	public float ambientPower;

	public Color tintColor = Color.white;

	public Color rimColor = Color.black;

	public float rimPower;

	public Color lightmapColor = Color.black;

	public float lightmapPower;

	public bool lightmapEnabled;

	public float lightmapLight;

	private Color lastAmbientColor = Color.clear;

	private float lastAmbientPower = -1f;

	private Color lastTintColor = Color.clear;

	private Color lastRimColor = Color.clear;

	private float lastRimPower = -1f;

	private Color lastLightmapColor = Color.clear;

	private float lastLightmapPower = -1f;

	private bool lastLightmapEnabled = true;

	private float lastLightmapLight = -1f;

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
			if (current)
			{
				if (this.lastAmbientPower != this.ambientPower)
				{
					current.SetFloat("_AmbientPower", this.ambientPower);
				}
				if (this.lastAmbientColor != this.ambientColor)
				{
					current.SetColor("_AmbientColor", this.ambientColor);
				}
				if (this.lastTintColor != this.tintColor)
				{
					current.SetColor("_LightTint", this.tintColor);
				}
				if (this.lastRimPower != this.rimPower)
				{
					current.SetFloat("_RimPower", this.rimPower);
				}
				if (this.lastRimColor != this.rimColor)
				{
					current.SetColor("_RimColor", this.rimColor);
				}
				if (this.lastLightmapPower != this.lightmapPower)
				{
					current.SetFloat("_LightmapPower", this.lightmapPower);
				}
				if (this.lastLightmapColor != this.lightmapColor)
				{
					current.SetColor("_LightmapColor", this.lightmapColor);
				}
				if (this.lastLightmapEnabled != this.lightmapEnabled)
				{
					if (this.lightmapEnabled)
					{
						current.EnableKeyword("LIGHTMAP");
					}
					else
					{
						current.DisableKeyword("LIGHTMAP");
					}
				}
				if (this.lastLightmapLight != this.lightmapLight)
				{
					current.SetFloat("_ShadowPower", this.lightmapLight);
				}
			}
		}
		this.lastAmbientPower = this.ambientPower;
		this.lastAmbientColor = this.ambientColor;
		this.lastTintColor = this.tintColor;
		this.lastRimPower = this.rimPower;
		this.lastRimColor = this.rimColor;
		this.lastLightmapEnabled = this.lightmapEnabled;
		this.lastLightmapLight = this.lightmapLight;
	}

	private void Update()
	{
		this.updateMaterials();
	}
}
