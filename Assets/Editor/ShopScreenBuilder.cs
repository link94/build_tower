using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Ghi ShopScreenIAP và nút Shop vào Main.unity để còn sau khi tắt Play.
/// </summary>
public static class ShopScreenBuilder
{
	private const string ScenePath = "Assets/Scenes/Main.unity";

	[MenuItem("Build Tower/IAP/Build Shop Screen", priority = 0)]
	public static void BuildShopScreen()
	{
		Scene scene = EditorSceneManager.OpenScene(ScenePath);
		Transform canvas = FindRoot(scene, "Canvas");
		if (canvas == null)
		{
			Debug.LogError("[ShopScreenBuilder] Không tìm thấy Canvas trong " + ScenePath);
			return;
		}

		if (canvas.Find("GameplayPanel") == null && canvas.Find("StartPanel") == null)
		{
			Debug.LogError("[ShopScreenBuilder] Không tìm thấy GameplayPanel hoặc StartPanel.");
			return;
		}

		ShopScreenIAP shop = ShopScreenFactory.Build(canvas);
		WirePersistentOpen(canvas.Find("GameplayPanel"), shop);
		WirePersistentOpen(canvas.Find("StartPanel"), shop);
		ShopScreenFactory.EnsureIapManager();

		EditorSceneManager.MarkSceneDirty(scene);
		EditorSceneManager.SaveScene(scene);
		Debug.Log("[ShopScreenBuilder] Đã dựng ShopScreenIAP và nút Shop.");
	}

	private static void WirePersistentOpen(Transform parent, ShopScreenIAP shop)
	{
		if (parent == null || shop == null)
			return;

		Transform buttonTransform = parent.Find("ShopButton");
		if (buttonTransform == null)
			return;

		Button button = buttonTransform.GetComponent<Button>();
		if (button == null)
			return;

		UnityEventTools.AddPersistentListener(button.onClick, shop.Open);
		EditorUtility.SetDirty(button);
		EditorUtility.SetDirty(shop);
	}

	private static Transform FindRoot(Scene scene, string name)
	{
		GameObject[] roots = scene.GetRootGameObjects();
		for (int i = 0; i < roots.Length; i++)
		{
			if (roots[i].name == name)
				return roots[i].transform;
		}

		return null;
	}
}
