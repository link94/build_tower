using System;
using UnityEngine;

namespace Kirnu
{
	[AddComponentMenu("Image Effects/Marvelous/Screen Texture Blend"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
	public class ScreenTextureBlend : MonoBehaviour
	{
		public enum BlendMode
		{
			Darken,
			Multiply,
			ColorBurn,
			LinearBurn,
			Lighten,
			Screen,
			ColorDodge,
			LinearDodge,
			Overlay,
			SoftLight,
			HardLight,
			VividLight,
			LinearLight,
			PinLight,
			Difference,
			Exclusion
		}

		public ScreenTextureBlend.BlendMode blendMode;

		private int currentBlendMode;

		[Range(0f, 1f)]
		public float blendIntensity;

		[Range(0f, 1f)]
		public float vignetteIntensity;

		[Range(0f, 1f)]
		public float vignetteMaxValue = 0.2f;

		public Texture2D gradientTexture;

		private Shader shader;

		public Material material;

		private void Start()
		{
			this.currentBlendMode = -1;
			if (this.gradientTexture == null)
			{
				this.gradientTexture = Texture2D.whiteTexture;
			}
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
		}

		public void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (base.enabled && this.material && this.gradientTexture)
			{
				if (this.currentBlendMode != (int)this.blendMode)
				{
					this.currentBlendMode = (int)this.blendMode;
					if ((this.blendMode & ScreenTextureBlend.BlendMode.Multiply) != ScreenTextureBlend.BlendMode.Darken)
					{
						this.material.EnableKeyword("B1");
					}
					else
					{
						this.material.DisableKeyword("B1");
					}
					if ((this.blendMode & ScreenTextureBlend.BlendMode.ColorBurn) != ScreenTextureBlend.BlendMode.Darken)
					{
						this.material.EnableKeyword("B2");
					}
					else
					{
						this.material.DisableKeyword("B2");
					}
					if ((this.blendMode & ScreenTextureBlend.BlendMode.Lighten) != ScreenTextureBlend.BlendMode.Darken)
					{
						this.material.EnableKeyword("B3");
					}
					else
					{
						this.material.DisableKeyword("B3");
					}
					if ((this.blendMode & ScreenTextureBlend.BlendMode.Overlay) != ScreenTextureBlend.BlendMode.Darken)
					{
						this.material.EnableKeyword("B4");
					}
					else
					{
						this.material.DisableKeyword("B4");
					}
				}
				this.material.SetFloat("_VignetteMax", this.vignetteMaxValue);
				this.material.SetFloat("_BlendIntensity", this.blendIntensity);
				this.material.SetFloat("_VignetteIntensity", this.vignetteIntensity);
				this.material.SetTexture("_Gradient", this.gradientTexture);
				Graphics.Blit(src, dest, this.material, 0);
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
