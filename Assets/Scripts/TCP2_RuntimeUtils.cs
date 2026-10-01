using System;
using System.Collections.Generic;
using UnityEngine;

public static class TCP2_RuntimeUtils
{
	private const string BASE_SHADER_PATH = "Toony Colors Pro 2/";

	private const string VARIANT_SHADER_PATH = "Hidden/Toony Colors Pro 2/Variants/";

	private const string BASE_SHADER_NAME = "Desktop";

	private const string BASE_SHADER_NAME_MOB = "Mobile";

	private static List<string[]> ShaderVariants = new List<string[]>
	{
		new string[]
		{
			"Specular",
			"TCP2_SPEC"
		},
		new string[]
		{
			"Reflection",
			"TCP2_REFLECTION",
			"TCP2_REFLECTION_MASKED"
		},
		new string[]
		{
			"Matcap",
			"TCP2_MC"
		},
		new string[]
		{
			"Rim",
			"TCP2_RIM"
		},
		new string[]
		{
			"RimOutline",
			"TCP2_RIMO"
		},
		new string[]
		{
			"Outline",
			"OUTLINES"
		},
		new string[]
		{
			"OutlineBlending",
			"OUTLINE_BLENDING"
		}
	};

	public static Shader GetShaderWithKeywords(Material material)
	{
		bool flag = material.shader != null && material.shader.name.ToLower().Contains("mobile");
		string text = (!flag) ? "Desktop" : "Mobile";
		string text2 = text;
		foreach (string[] current in TCP2_RuntimeUtils.ShaderVariants)
		{
			string[] shaderKeywords = material.shaderKeywords;
			for (int i = 0; i < shaderKeywords.Length; i++)
			{
				string a = shaderKeywords[i];
				for (int j = 1; j < current.Length; j++)
				{
					if (a == current[j])
					{
						text2 = text2 + " " + current[0];
					}
				}
			}
		}
		text2 = text2.TrimEnd(new char[0]);
		string str = "Toony Colors Pro 2/";
		if (text2 != text)
		{
			str = "Hidden/Toony Colors Pro 2/Variants/";
		}
		return Shader.Find(str + text2);
	}
}
