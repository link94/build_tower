using System;
using UnityEngine;

public class TCP2_Demo : MonoBehaviour
{
	public Material[] AffectedMaterials;

	public Texture2D[] RampTextures;

	public GUISkin GuiSkin;

	public Light DirLight;

	public GameObject Robot;

	public GameObject Ethan;

	private bool mUnityShader;

	private bool mShaderSpecular = true;

	private bool mShaderBump = true;

	private bool mShaderReflection;

	private bool mShaderRim = true;

	private bool mShaderRimOutline;

	private bool mShaderOutline = true;

	private float mRimMin = 0.5f;

	private float mRimMax = 1f;

	private bool mRampTextureFlag;

	private Texture2D mRampTexture;

	private float mRampSmoothing = 0.15f;

	private float mLightRotationX = 80f;

	private float mLightRotationY = 25f;

	private bool mViewRobot;

	private bool mRobotOutlineNormals = true;

	private TCP2_Demo_View DemoView;

	private void Awake()
	{
		this.DemoView = base.GetComponent<TCP2_Demo_View>();
		this.mRampTexture = this.RampTextures[0];
		this.UpdateShader();
	}

	private void OnDestroy()
	{
		this.RestoreRimColors();
		this.UpdateShader();
	}

	private void OnGUI()
	{
		GUI.skin = this.GuiSkin;
		GUILayout.BeginArea(new Rect(new Rect((float)(Screen.width - 310), 20f, 290f, 30f)));
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Demo Character:", new GUILayoutOption[0]);
		if (GUILayout.Button("Ethan", (!this.mViewRobot) ? "ButtonOn" : "Button", new GUILayoutOption[0]))
		{
			this.mViewRobot = false;
			this.Robot.SetActive(false);
			this.Ethan.SetActive(true);
			this.DemoView.CharacterTransform = this.Ethan.transform;
		}
		if (GUILayout.Button("Robot Kyle", this.mViewRobot ? "ButtonOn" : "Button", new GUILayoutOption[0]))
		{
			this.mViewRobot = true;
			this.Robot.SetActive(true);
			this.Ethan.SetActive(false);
			this.DemoView.CharacterTransform = this.Robot.transform;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(new Rect((float)(Screen.width - 310), 55f, 290f, (float)(Screen.height - 40 - 90))));
		if (this.mViewRobot)
		{
			GUILayout.Label("Outline Normals", new GUILayoutOption[0]);
			this.mRobotOutlineNormals = !GUILayout.Toggle(!this.mRobotOutlineNormals, "Regular Normals", new GUILayoutOption[0]);
			this.mRobotOutlineNormals = GUILayout.Toggle(this.mRobotOutlineNormals, "TCP2's Encoded Smoothed Normals", new GUILayoutOption[0]);
			GUILayout.Label("Toony Colors Pro 2 introduces an innovative way to fix broken outline caused by hard-edge shading.\nRead the documentation to learn more!", "SmallLabelShadow", new GUILayoutOption[0]);
			Rect lastRect = GUILayoutUtility.GetLastRect();
			GUI.Label(lastRect, "Toony Colors Pro 2 introduces an innovative way to fix broken outline caused by hard-edge shading.\nRead the documentation to learn more!", "SmallLabel");
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(new Rect((float)(Screen.width - 210), (float)(Screen.height - 60), 190f, 50f)));
		GUILayout.Label("Quality Settings:", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("<", new GUILayoutOption[]
		{
			GUILayout.Width(26f)
		}))
		{
			QualitySettings.DecreaseLevel(true);
		}
		GUILayout.Label(QualitySettings.names[QualitySettings.GetQualityLevel()], "LabelCenter", new GUILayoutOption[0]);
		if (GUILayout.Button(">", new GUILayoutOption[]
		{
			GUILayout.Width(26f)
		}))
		{
			QualitySettings.IncreaseLevel(true);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(20f, 110f, (float)(Screen.width - 40), (float)(Screen.height - 40)));
		this.mUnityShader = GUILayout.Toggle(this.mUnityShader, "View with Unity " + ((!this.mViewRobot) ? "\"Bumped Specular\"" : "\"Diffuse Specular\""), new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUI.enabled = !this.mUnityShader;
		GUILayout.Label("Toony Colors Pro 2 Settings", new GUILayoutOption[0]);
		this.mShaderSpecular = GUILayout.Toggle(this.mShaderSpecular, "Specular", new GUILayoutOption[0]);
		GUI.enabled = !this.mViewRobot;
		if (GUI.enabled)
		{
			this.mShaderBump = GUILayout.Toggle(this.mShaderBump, "Bump", new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.Toggle(false, "Bump", new GUILayoutOption[0]);
		}
		GUI.enabled = !this.mUnityShader;
		this.mShaderReflection = GUILayout.Toggle(this.mShaderReflection, "Reflection", new GUILayoutOption[0]);
		bool flag = this.mShaderRim;
		this.mShaderRim = GUILayout.Toggle(this.mShaderRim, "Rim Lighting", new GUILayoutOption[0]);
		flag = (flag != this.mShaderRim);
		if (flag && this.mShaderRim && this.mShaderRimOutline)
		{
			this.mShaderRimOutline = false;
		}
		if (flag && this.mShaderRim)
		{
			this.RestoreRimColors();
		}
		flag = this.mShaderRimOutline;
		this.mShaderRimOutline = GUILayout.Toggle(this.mShaderRimOutline, "Rim Outline", new GUILayoutOption[0]);
		flag = (flag != this.mShaderRimOutline);
		if (flag && this.mShaderRimOutline && this.mShaderRim)
		{
			this.mShaderRim = false;
		}
		if (flag && this.mShaderRimOutline)
		{
			this.RimOutlineColor();
		}
		GUI.enabled &= (this.mShaderRim || this.mShaderRimOutline);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Rim Min", new GUILayoutOption[]
		{
			GUILayout.Width(70f)
		});
		this.mRimMin = GUILayout.HorizontalSlider(this.mRimMin, 0f, 1f, new GUILayoutOption[]
		{
			GUILayout.Width(130f)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Rim Max", new GUILayoutOption[]
		{
			GUILayout.Width(70f)
		});
		this.mRimMax = GUILayout.HorizontalSlider(this.mRimMax, 0f, 1f, new GUILayoutOption[]
		{
			GUILayout.Width(130f)
		});
		GUILayout.EndHorizontal();
		GUI.enabled = !this.mUnityShader;
		this.mShaderOutline = GUILayout.Toggle(this.mShaderOutline, "Outline", new GUILayoutOption[0]);
		GUILayout.Space(6f);
		GUILayout.Label("Ramp Settings", new GUILayoutOption[0]);
		this.mRampTextureFlag = GUILayout.Toggle(this.mRampTextureFlag, "Textured Ramp", new GUILayoutOption[0]);
		GUI.enabled &= this.mRampTextureFlag;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		Rect position = GUILayoutUtility.GetRect(200f, 20f, new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(false)
		});
		position.y += 4f;
		GUI.DrawTexture(position, this.mRampTexture);
		if (GUILayout.Button("<", new GUILayoutOption[]
		{
			GUILayout.Width(26f)
		}))
		{
			this.PrevRamp();
		}
		if (GUILayout.Button(">", new GUILayoutOption[]
		{
			GUILayout.Width(26f)
		}))
		{
			this.NextRamp();
		}
		GUILayout.EndHorizontal();
		GUI.enabled = !this.mUnityShader;
		GUI.enabled &= !this.mRampTextureFlag;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Smoothing", new GUILayoutOption[]
		{
			GUILayout.Width(85f)
		});
		this.mRampSmoothing = GUILayout.HorizontalSlider(this.mRampSmoothing, 0.01f, 1f, new GUILayoutOption[]
		{
			GUILayout.Width(115f)
		});
		GUILayout.EndHorizontal();
		if (GUI.changed)
		{
			if (this.mUnityShader)
			{
				this.UnityDiffuseShader();
			}
			else
			{
				this.UpdateShader();
			}
		}
		GUI.enabled = true;
		GUILayout.Space(10f);
		GUILayout.Label("Light Rotation", new GUILayoutOption[0]);
		this.mLightRotationX = GUILayout.HorizontalSlider(this.mLightRotationX, 0f, 360f, new GUILayoutOption[]
		{
			GUILayout.Width(200f)
		});
		this.mLightRotationY = GUILayout.HorizontalSlider(this.mLightRotationY, 0f, 360f, new GUILayoutOption[]
		{
			GUILayout.Width(200f)
		});
		GUILayout.Space(4f);
		GUILayout.Label("Hold Left mouse button to rotate character", "SmallLabelShadow", new GUILayoutOption[0]);
		position = GUILayoutUtility.GetLastRect();
		GUI.Label(position, "Hold Left mouse button to rotate character", "SmallLabel");
		GUILayout.Label("Hold Right/Middle mouse button to scroll", "SmallLabelShadow", new GUILayoutOption[0]);
		position = GUILayoutUtility.GetLastRect();
		GUI.Label(position, "Hold Right/Middle mouse button to scroll", "SmallLabel");
		GUILayout.Label("Use mouse scroll wheel or up/down keys to zoom", "SmallLabelShadow", new GUILayoutOption[0]);
		position = GUILayoutUtility.GetLastRect();
		GUI.Label(position, "Use mouse scroll wheel or up/down keys to zoom", "SmallLabel");
		if (GUI.changed)
		{
			Vector3 eulerAngles = this.DirLight.transform.eulerAngles;
			eulerAngles.y = this.mLightRotationX;
			eulerAngles.x = this.mLightRotationY;
			this.DirLight.transform.eulerAngles = eulerAngles;
		}
		GUILayout.EndArea();
	}

	private void UnityDiffuseShader()
	{
		Shader shader = Shader.Find("Bumped Specular");
		Shader shader2 = Shader.Find("Specular");
		Material[] affectedMaterials = this.AffectedMaterials;
		for (int i = 0; i < affectedMaterials.Length; i++)
		{
			Material material = affectedMaterials[i];
			if (material.name.Contains("Robot"))
			{
				material.shader = shader2;
			}
			else
			{
				material.shader = shader;
			}
		}
	}

	private void UpdateShader()
	{
		Material[] affectedMaterials = this.AffectedMaterials;
		for (int i = 0; i < affectedMaterials.Length; i++)
		{
			Material material = affectedMaterials[i];
			this.ToggleKeyword(material, this.mShaderSpecular, "TCP2_SPEC");
			if (!material.name.Contains("Robot"))
			{
				this.ToggleKeyword(material, this.mShaderBump, "TCP2_BUMP");
			}
			this.ToggleKeyword(material, this.mShaderReflection, "TCP2_REFLECTION_MASKED");
			this.ToggleKeyword(material, this.mShaderRim, "TCP2_RIM");
			this.ToggleKeyword(material, this.mShaderRimOutline, "TCP2_RIMO");
			this.ToggleKeyword(material, this.mShaderOutline, "OUTLINES");
			this.ToggleKeyword(material, this.mRampTextureFlag, "TCP2_RAMPTEXT");
			material.SetFloat("_RampSmooth", this.mRampSmoothing);
			material.SetTexture("_Ramp", this.mRampTexture);
			material.SetFloat("_RimMin", this.mRimMin);
			material.SetFloat("_RimMax", this.mRimMax);
			if (material.name.Contains("Robot"))
			{
				this.ToggleKeyword(material, this.mRobotOutlineNormals, "TCP2_TANGENT_AS_NORMALS");
			}
		}
		Material[] affectedMaterials2 = this.AffectedMaterials;
		for (int j = 0; j < affectedMaterials2.Length; j++)
		{
			Material material2 = affectedMaterials2[j];
			Shader shaderWithKeywords = TCP2_RuntimeUtils.GetShaderWithKeywords(material2);
			if (shaderWithKeywords == null)
			{
				string text = string.Empty;
				string[] shaderKeywords = material2.shaderKeywords;
				for (int k = 0; k < shaderKeywords.Length; k++)
				{
					string str = shaderKeywords[k];
					text = text + str + ",";
				}
				text = text.TrimEnd(new char[]
				{
					','
				});
				UnityEngine.Debug.LogError(string.Concat(new string[]
				{
					"[TCP2 Demo] Can't find shader for keywords: \"",
					text,
					"\" in material \"",
					material2.name,
					"\"\nThe missing shaders probably need to be unpacked. See TCP2 Documentation!"
				}));
			}
			else
			{
				material2.shader = shaderWithKeywords;
			}
		}
	}

	private void RimOutlineColor()
	{
		Material[] affectedMaterials = this.AffectedMaterials;
		for (int i = 0; i < affectedMaterials.Length; i++)
		{
			Material material = affectedMaterials[i];
			material.SetColor("_RimColor", Color.black);
		}
	}

	private void RestoreRimColors()
	{
		Material[] affectedMaterials = this.AffectedMaterials;
		for (int i = 0; i < affectedMaterials.Length; i++)
		{
			Material material = affectedMaterials[i];
			if (material.name.Contains("Robot"))
			{
				material.SetColor("_RimColor", new Color(0.2f, 0.6f, 1f, 0.5f));
			}
			else
			{
				material.SetColor("_RimColor", new Color(1f, 1f, 1f, 0.25f));
			}
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

	private void PrevRamp()
	{
		int num = Array.IndexOf<Texture2D>(this.RampTextures, this.mRampTexture);
		num = Mathf.Clamp(num, 0, this.RampTextures.Length - 1);
		num--;
		if (num < 0)
		{
			num = this.RampTextures.Length - 1;
		}
		this.mRampTexture = this.RampTextures[num];
	}

	private void NextRamp()
	{
		int num = Array.IndexOf<Texture2D>(this.RampTextures, this.mRampTexture);
		num = Mathf.Clamp(num, 0, this.RampTextures.Length - 1);
		num++;
		if (num >= this.RampTextures.Length)
		{
			num = 0;
		}
		this.mRampTexture = this.RampTextures[num];
	}
}
