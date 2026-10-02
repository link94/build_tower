using UnityEngine;

/// <summary>
/// Ảnh bầu trời dán sát far clip của Main Camera, đi theo camera khi tháp cao lên.
/// </summary>
public class SkyBackdrop : MonoBehaviour
{
	private static readonly Color SkyBlue = new Color(0.47f, 0.74f, 0.96f, 1f);

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Attach()
	{
		Camera camera = Camera.main;
		if (camera == null)
			return;

		if (camera.GetComponent<SkyBackdrop>() == null)
			camera.gameObject.AddComponent<SkyBackdrop>();
	}

	private Camera _camera;
	private Transform _quad;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
		if (_camera != null)
			_camera.backgroundColor = SkyBlue;

		if (transform.Find("SkyBackdrop") != null)
		{
			_quad = transform.Find("SkyBackdrop");
			return;
		}

		Texture texture = LoadSkyTexture();
		if (texture == null || _camera == null)
			return;

		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.name = "SkyBackdrop";
		Collider collider = quad.GetComponent<Collider>();
		if (collider != null)
			Destroy(collider);

		quad.transform.SetParent(transform, false);
		quad.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		_quad = quad.transform;

		MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
		Shader shader = Shader.Find("BuildTower/SkyBackdrop");
		if (shader == null)
			shader = Shader.Find("Unlit/Texture");
		if (shader == null)
			return;

		Material material = new Material(shader);
		material.mainTexture = texture;
		renderer.sharedMaterial = material;
		renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		renderer.receiveShadows = false;
		renderer.allowOcclusionWhenDynamic = false;
		Fit();
	}

	private void LateUpdate()
	{
		Fit();
	}

	private static Texture LoadSkyTexture()
	{
		Sprite sprite = Resources.Load<Sprite>("ShopIAP/bg_app");
		if (sprite != null)
			return sprite.texture;

		return Resources.Load<Texture>("ShopIAP/bg_app");
	}

	private void Fit()
	{
		if (_quad == null || _camera == null)
			return;

		float distance = Mathf.Clamp(20f, _camera.nearClipPlane + 1f, _camera.farClipPlane * 0.5f);
		float height = 2f * distance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		float width = height * Mathf.Max(_camera.aspect, 0.01f);
		_quad.localPosition = new Vector3(0f, 0f, distance);
		_quad.localScale = new Vector3(width, height, 1f);
	}
}
