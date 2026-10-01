using System;
using UnityEngine;
using UnityEngine.UI;

public class TCP2_Demo_PBS : MonoBehaviour
{
	[Serializable]
	public class SkyboxSetting
	{
		public Material SkyMaterial;

		public Color lightColor;

		public Vector3 DirLightEuler;
	}

	public Light DirLight;

	public GameObject PointLights;

	public MeshRenderer Robot;

	public GameObject Canvas;

	public TCP2_Demo_PBS.SkyboxSetting[] SkySettings;

	public bool FlipLight = true;

	public Texture2D[] RampTextures;

	public Slider SmoothnessSlider;

	public Text SmoothnessValue;

	public Slider MetallicSlider;

	public Text MetallicValue;

	public Text BumpScaleValue;

	public Text ShaderText;

	public Text SkyboxValue;

	public Text RampValue;

	public Slider RampThresholdSlider;

	public Text RampThresholdValue;

	public Slider RampSmoothSlider;

	public Text RampSmoothValue;

	public Slider RampSmoothAddSlider;

	public Text RampSmoothAddValue;

	public RawImage RampImage;

	private int currentSky;

	private int currentRamp;

	private Material robotMaterial;

	private bool mUseOutline;

	private bool mRotatePointLights = true;

	public bool ShowPointLights
	{
		set
		{
			this.PointLights.SetActive(value);
		}
	}

	public bool ShowDirLight
	{
		set
		{
			this.DirLight.enabled = value;
		}
	}

	public bool RotatePointLights
	{
		get
		{
			return this.mRotatePointLights;
		}
		set
		{
			this.mRotatePointLights = value;
		}
	}

	public bool UseOutline
	{
		get
		{
			return this.mUseOutline;
		}
		set
		{
			this.mUseOutline = value;
			if (this.robotMaterial.shader.name.Contains("Toony"))
			{
				this.ShowTCP2Shader();
			}
		}
	}

	public bool UseRampTexture
	{
		set
		{
			this.robotMaterial.SetFloat("_TCP2_RAMPTEXT", (!value) ? 0f : 1f);
			if (value)
			{
				this.robotMaterial.EnableKeyword("TCP2_RAMPTEXT");
			}
			else
			{
				this.robotMaterial.DisableKeyword("TCP2_RAMPTEXT");
			}
		}
	}

	public bool UseStylizedFresnel
	{
		set
		{
			this.robotMaterial.SetFloat("_TCP2_STYLIZED_FRESNEL", (!value) ? 0f : 1f);
			if (value)
			{
				this.robotMaterial.EnableKeyword("TCP2_STYLIZED_FRESNEL");
			}
			else
			{
				this.robotMaterial.DisableKeyword("TCP2_STYLIZED_FRESNEL");
			}
		}
	}

	public bool UseStylizedSpecular
	{
		set
		{
			this.robotMaterial.SetFloat("_TCP2_SPEC_TOON", (!value) ? 0f : 1f);
			if (value)
			{
				this.robotMaterial.EnableKeyword("TCP2_SPEC_TOON");
			}
			else
			{
				this.robotMaterial.DisableKeyword("TCP2_SPEC_TOON");
			}
		}
	}

	private void Awake()
	{
		this.robotMaterial = this.Robot.material;
		this.mUseOutline = this.robotMaterial.shader.name.Contains("Outline");
		this.MetallicSlider.value = this.robotMaterial.GetFloat("_Metallic");
		this.SmoothnessSlider.value = this.robotMaterial.GetFloat("_Glossiness");
		this.RampThresholdSlider.value = this.robotMaterial.GetFloat("_RampThreshold");
		this.RampSmoothSlider.value = this.robotMaterial.GetFloat("_RampSmooth");
		this.RampSmoothAddSlider.value = this.robotMaterial.GetFloat("_RampSmoothAdd");
		this.UpdateSky();
		this.UpdateRamp();
	}

	private void Update()
	{
		if (this.mRotatePointLights)
		{
			this.PointLights.transform.Rotate(Vector3.up * 20f * Time.deltaTime);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.H))
		{
			this.Canvas.SetActive(!this.Canvas.activeSelf);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.NextSky();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.PrevSky();
		}
	}

	public void ToggleShader()
	{
		if (this.robotMaterial.shader.name.Contains("Toony"))
		{
			this.ShowUnityStandardShader();
			this.ShaderText.text = "View with TCP2 PBS shader";
		}
		else
		{
			this.ShowTCP2Shader();
			this.ShaderText.text = "View with Unity Standard shader";
		}
	}

	public void NextSky()
	{
		this.currentSky++;
		if (this.currentSky >= this.SkySettings.Length)
		{
			this.currentSky = 0;
		}
		this.UpdateSky();
	}

	public void PrevSky()
	{
		this.currentSky--;
		if (this.currentSky < 0)
		{
			this.currentSky = this.SkySettings.Length - 1;
		}
		this.UpdateSky();
	}

	public void NextRamp()
	{
		this.currentRamp++;
		if (this.currentRamp >= this.RampTextures.Length)
		{
			this.currentRamp = 0;
		}
		this.UpdateRamp();
	}

	public void PrevRamp()
	{
		this.currentRamp--;
		if (this.currentRamp < 0)
		{
			this.currentRamp = this.RampTextures.Length - 1;
		}
		this.UpdateRamp();
	}

	public void SetMetallic(float f)
	{
		this.robotMaterial.SetFloat("_Metallic", f);
		this.MetallicValue.text = f.ToString("0.00");
	}

	public void SetSmoothness(float f)
	{
		this.robotMaterial.SetFloat("_Glossiness", f);
		this.SmoothnessValue.text = f.ToString("0.00");
	}

	public void SetBumpScale(float f)
	{
		this.robotMaterial.SetFloat("_BumpScale", f);
		this.BumpScaleValue.text = f.ToString("0.00");
	}

	public void SetRampThreshold(float f)
	{
		this.robotMaterial.SetFloat("_RampThreshold", f);
		this.RampThresholdValue.text = f.ToString("0.00");
	}

	public void SetRampSmooth(float f)
	{
		this.robotMaterial.SetFloat("_RampSmooth", f);
		this.RampSmoothValue.text = f.ToString("0.00");
	}

	public void SetRampSmoothAdd(float f)
	{
		this.robotMaterial.SetFloat("_RampSmoothAdd", f);
		this.RampSmoothAddValue.text = f.ToString("0.00");
	}

	private void UpdateRamp()
	{
		this.robotMaterial.SetTexture("_Ramp", this.RampTextures[this.currentRamp]);
		this.RampValue.text = string.Format("{0}/{1}", this.currentRamp + 1, this.RampTextures.Length);
		this.RampImage.texture = this.RampTextures[this.currentRamp];
	}

	private void UpdateSky()
	{
		TCP2_Demo_PBS.SkyboxSetting skyboxSetting = this.SkySettings[this.currentSky];
		this.DirLight.transform.eulerAngles = skyboxSetting.DirLightEuler;
		if (this.FlipLight)
		{
			this.DirLight.transform.Rotate(Vector3.up, 180f, Space.Self);
		}
		this.DirLight.color = skyboxSetting.lightColor;
		RenderSettings.skybox = skyboxSetting.SkyMaterial;
		RenderSettings.customReflection = (skyboxSetting.SkyMaterial.GetTexture("_Tex") as Cubemap);
		DynamicGI.UpdateEnvironment();
		this.SkyboxValue.text = string.Format("{0}/{1}", this.currentSky + 1, this.SkySettings.Length);
	}

	private void ShowUnityStandardShader()
	{
		this.robotMaterial.shader = Shader.Find("Standard");
	}

	public void ShowTCP2Shader()
	{
		string name = (!this.mUseOutline) ? "Toony Colors Pro 2/Standard PBS" : "Hidden/Toony Colors Pro 2/Standard PBS Outline";
		Shader shader = Shader.Find(name);
		if (shader != null)
		{
			this.robotMaterial.shader = shader;
		}
	}

	private void ToggleKeyword(Material m, bool enabled, string keyword)
	{
		if (enabled)
		{
			m.EnableKeyword(keyword);
		}
		else
		{
			m.DisableKeyword(keyword);
		}
	}
}
