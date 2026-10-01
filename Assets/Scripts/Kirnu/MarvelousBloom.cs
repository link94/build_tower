using System;
using UnityEngine;

namespace Kirnu
{
	[AddComponentMenu("Image Effects/Marvelous/Marvelous Bloom"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
	public class MarvelousBloom : MonoBehaviour
	{
		public enum Resolution
		{
			Lower,
			Low,
			High,
			Higher
		}

		public enum BlurType
		{
			Standard,
			Sgx
		}

		public Color bloomColor = Color.white;

		[Range(0f, 1.5f)]
		public float threshold = 0.05f;

		[Range(0f, 1f)]
		public float intensity = 0.05f;

		public MarvelousBloom.Resolution resolution = MarvelousBloom.Resolution.Low;

		[Range(1f, 8f)]
		public int blurIterations = 1;

		private Shader shader;

		public Material material;

		private void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
		}

		public void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (base.enabled && this.material)
			{
				int num = (this.resolution != MarvelousBloom.Resolution.Lower) ? ((this.resolution != MarvelousBloom.Resolution.Low) ? ((this.resolution != MarvelousBloom.Resolution.High) ? 1 : 2) : 4) : 8;
				int num2 = src.width / num;
				int num3 = src.height / num;
				this.material.SetColor("_BloomColor", this.bloomColor);
				this.material.SetVector("_Parameter", new Vector4((float)num2, (float)num3, this.threshold, this.intensity));
				src.filterMode = FilterMode.Bilinear;
				RenderTexture renderTexture = RenderTexture.GetTemporary(num2, num3, 0, src.format);
				renderTexture.filterMode = FilterMode.Bilinear;
				Graphics.Blit(src, renderTexture, this.material, 1);
				for (int i = 0; i < this.blurIterations; i++)
				{
					this.material.SetVector("_Parameter", new Vector4(0f, 1f, this.threshold, this.intensity));
					RenderTexture temporary = RenderTexture.GetTemporary(num2, num3, 0, src.format);
					temporary.filterMode = FilterMode.Bilinear;
					Graphics.Blit(renderTexture, temporary, this.material, 2);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
					this.material.SetVector("_Parameter", new Vector4(1f, 0f, this.threshold, this.intensity));
					temporary = RenderTexture.GetTemporary(num2, num3, 0, src.format);
					temporary.filterMode = FilterMode.Bilinear;
					Graphics.Blit(renderTexture, temporary, this.material, 2);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
				}
				this.material.SetTexture("_Bloom", renderTexture);
				Graphics.Blit(src, dest, this.material, 0);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture.DiscardContents(true, true);
			}
			else
			{
				Graphics.Blit(src, dest);
			}
		}

		private void OnDisable()
		{
		}
	}
}
