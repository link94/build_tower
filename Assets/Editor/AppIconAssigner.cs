using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// Gán Assets/Art/AppIcon.jpg làm icon Android (legacy, tròn và adaptive).
/// </summary>
[InitializeOnLoad]
public static class AppIconAssigner
{
	private const string IconPath = "Assets/Art/AppIcon.jpg";

	static AppIconAssigner()
	{
		EditorApplication.delayCall += Assign;
	}

	[MenuItem("Build Tower/IAP/Assign App Icon", priority = 1)]
	public static void Assign()
	{
		Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
		if (icon == null)
			return;

		NamedBuildTarget android = NamedBuildTarget.Android;
		PlatformIconKind[] kinds = PlayerSettings.GetSupportedIconKinds(android);
		for (int i = 0; i < kinds.Length; i++)
		{
			PlatformIcon[] icons = PlayerSettings.GetPlatformIcons(android, kinds[i]);
			for (int j = 0; j < icons.Length; j++)
			{
				Texture2D[] layers = new Texture2D[icons[j].maxLayerCount];
				for (int layer = 0; layer < layers.Length; layer++)
					layers[layer] = icon;

				icons[j].SetTextures(layers);
			}

			PlayerSettings.SetPlatformIcons(android, kinds[i], icons);
		}

		AssetDatabase.SaveAssets();
		Debug.Log("[AppIcon] Đã gán icon Android " + IconPath);
	}
}
