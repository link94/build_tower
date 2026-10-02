using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Dựng ShopScreenIAP bằng sprite cửa hàng cube jump.
/// Lần Play sau xóa bản cũ (nút chữ cam) rồi dựng lại.
/// </summary>
public static class ShopScreenFactory
{
	private const string ScreenName = "ShopScreenIAP";
	private const string ShopButtonName = "ShopButton";

	private static readonly Color White = Color.white;
	private static readonly Color Navy = new Color(0.15f, 0.22f, 0.38f, 1f);
	private static readonly Color Green = new Color(0.29f, 0.78f, 0.45f, 1f);
	private static readonly Color PriceGray = new Color(0.55f, 0.62f, 0.70f, 1f);
	private static readonly Color Subtitle = new Color(0.90f, 0.95f, 1f, 0.92f);
	private static Sprite _rounded;
	private static readonly string[] GoldSprites =
	{
		"iv_gold1", "iv_gold2", "iv_gold3", "iv_gold4", "iv_gold4", "iv_gold5", "iv_gold5"
	};

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void EnsureAtRuntime()
	{
		Scene scene = SceneManager.GetActiveScene();
		Transform canvas = FindRoot(scene, "Canvas");
		if (canvas == null)
			return;

		if (canvas.Find("GameplayPanel") == null && canvas.Find("StartPanel") == null)
			return;

		EnsureIapManager();
		Build(canvas);
	}

	public static ShopScreenIAP Build(Transform canvas)
	{
		Transform oldScreen = canvas.Find(ScreenName);
		if (oldScreen != null)
			Object.DestroyImmediate(oldScreen.gameObject);

		Transform gameplay = canvas.Find("GameplayPanel");
		Transform start = canvas.Find("StartPanel");
		DestroyShopButton(gameplay);
		DestroyShopButton(start);

		Font font = LoadFont();
		ShopScreenIAP shop = CreateScreen(canvas, font);
		return shop;
	}

	public static void EnsureIapManager()
	{
		if (IAPManager.Instance != null)
			return;

		if (Object.FindAnyObjectByType<IAPManager>(FindObjectsInactive.Include) != null)
			return;

		GameObject go = new GameObject("IAPManager");
		go.AddComponent<IAPManager>();
	}

	private static void DestroyShopButton(Transform parent)
	{
		if (parent == null)
			return;

		Transform oldButton = parent.Find(ShopButtonName);
		if (oldButton != null)
			Object.DestroyImmediate(oldButton.gameObject);
	}

	private static ShopScreenIAP CreateScreen(Transform canvas, Font font)
	{
		RectTransform root = NewUI(ScreenName, canvas);
		Stretch(root);
		AddSprite(root, LoadShopSprite("bg_app"), true, false, Image.Type.Simple);

		ShopScreenIAP shop = root.gameObject.AddComponent<ShopScreenIAP>();

		RectTransform close = NewUI("close button", root);
		Place(close, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(10f, -8f), new Vector2(36f, 36f));
		Image closeImage = AddSprite(close, LoadShopSprite("iv_back"), true, true, Image.Type.Simple);
		Button closeButton = close.gameObject.AddComponent<Button>();
		closeButton.targetGraphic = closeImage;

		RectTransform title = TopCenter("title", root, 6f, 180f, 36f);
		AddText(title, font, "Shop", 24, TextAnchor.MiddleCenter, White);

		RectTransform scoreRow = TopCenter("score pill", root, 46f, 108f, 32f);
		Image pill = AddSprite(scoreRow, RoundedSprite(), false, false, Image.Type.Sliced);
		pill.color = White;
		HorizontalLayoutGroup scoreLayout = scoreRow.gameObject.AddComponent<HorizontalLayoutGroup>();
		scoreLayout.padding = new RectOffset(8, 10, 4, 4);
		scoreLayout.childAlignment = TextAnchor.MiddleCenter;
		scoreLayout.childControlWidth = true;
		scoreLayout.childControlHeight = true;
		scoreLayout.childForceExpandWidth = false;
		scoreLayout.childForceExpandHeight = true;
		scoreLayout.spacing = 6f;

		RectTransform coin = NewUI("coin icon", scoreRow);
		AddSprite(coin, LoadShopSprite("coin"), false, true, Image.Type.Simple);
		LayoutElement coinLayout = coin.gameObject.AddComponent<LayoutElement>();
		coinLayout.preferredWidth = 22f;
		coinLayout.preferredHeight = 22f;

		RectTransform scoreLabel = NewUI("score text", scoreRow);
		Text scoreText = AddText(scoreLabel, font, "0", 18, TextAnchor.MiddleLeft, Navy);
		LayoutElement scoreLayoutElement = scoreLabel.gameObject.AddComponent<LayoutElement>();
		scoreLayoutElement.preferredWidth = 48f;

		RectTransform subtitle = TopCenter("subtitle", root, 82f, 320f, 22f);
		AddText(subtitle, font, "Use coins to unlock new blocks and more!", 11, TextAnchor.MiddleCenter, Subtitle);

		RectTransform rows = NewUI("rows", root);
		rows.anchorMin = Vector2.zero;
		rows.anchorMax = Vector2.one;
		rows.offsetMin = new Vector2(10f, 28f);
		rows.offsetMax = new Vector2(-10f, -108f);
		rows.pivot = new Vector2(0.5f, 0.5f);

		VerticalLayoutGroup rowsLayout = rows.gameObject.AddComponent<VerticalLayoutGroup>();
		rowsLayout.spacing = 6f;
		rowsLayout.childAlignment = TextAnchor.UpperCenter;
		rowsLayout.childControlWidth = true;
		rowsLayout.childControlHeight = true;
		rowsLayout.childForceExpandWidth = true;
		rowsLayout.childForceExpandHeight = true;

		for (int i = 0; i < IAPCatalog.Packs.Length; i++)
			BuildRow(rows, font, IAPCatalog.Packs[i], i);

		RectTransform message = NewUI("message text", root);
		message.anchorMin = new Vector2(0f, 0f);
		message.anchorMax = new Vector2(1f, 0f);
		message.pivot = new Vector2(0.5f, 0f);
		message.anchoredPosition = new Vector2(0f, 22f);
		message.sizeDelta = new Vector2(-16f, 16f);
		Text messageText = AddText(message, font, string.Empty, 12, TextAnchor.MiddleCenter, new Color(1f, 0.55f, 0.45f, 1f));

		RectTransform footer = NewUI("footer", root);
		footer.anchorMin = new Vector2(0f, 0f);
		footer.anchorMax = new Vector2(1f, 0f);
		footer.pivot = new Vector2(0.5f, 0f);
		footer.anchoredPosition = new Vector2(0f, 4f);
		footer.sizeDelta = new Vector2(-16f, 18f);
		AddText(footer, font, "Build Higher  ·  Reach Further", 11, TextAnchor.MiddleCenter, Subtitle);

		shop.Bind(scoreText, closeButton, messageText);
		root.SetAsLastSibling();
		root.gameObject.SetActive(false);
		return shop;
	}

	private static void BuildRow(Transform parent, Font font, IAPCatalog.Pack pack, int index)
	{
		RectTransform row = NewUI("row " + pack.ProductId, parent);
		Image card = AddSprite(row, RoundedSprite(), false, false, Image.Type.Sliced);
		card.color = White;

		HorizontalLayoutGroup rowLayout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
		rowLayout.padding = new RectOffset(8, 8, 4, 4);
		rowLayout.spacing = 6f;
		rowLayout.childAlignment = TextAnchor.MiddleLeft;
		rowLayout.childControlWidth = true;
		rowLayout.childControlHeight = true;
		rowLayout.childForceExpandWidth = false;
		rowLayout.childForceExpandHeight = true;

		RectTransform gold = NewUI("gold icon", row);
		AddSprite(gold, LoadShopSprite(GoldSprites[index]), false, true, Image.Type.Simple);
		LayoutElement goldLayout = gold.gameObject.AddComponent<LayoutElement>();
		goldLayout.preferredWidth = 40f;
		goldLayout.preferredHeight = 40f;

		RectTransform info = NewUI("info", row);
		VerticalLayoutGroup infoLayout = info.gameObject.AddComponent<VerticalLayoutGroup>();
		infoLayout.childAlignment = TextAnchor.MiddleLeft;
		infoLayout.childControlWidth = true;
		infoLayout.childControlHeight = true;
		infoLayout.childForceExpandWidth = true;
		infoLayout.childForceExpandHeight = false;
		infoLayout.spacing = 0f;
		LayoutElement infoElement = info.gameObject.AddComponent<LayoutElement>();
		infoElement.flexibleWidth = 1f;
		infoElement.minWidth = 70f;

		RectTransform name = NewUI("name text", info);
		AddText(name, font, pack.DisplayName, 14, TextAnchor.MiddleLeft, Navy);
		LayoutElement nameElement = name.gameObject.AddComponent<LayoutElement>();
		nameElement.preferredHeight = 18f;

		RectTransform amountRow = NewUI("amount row", info);
		HorizontalLayoutGroup amountLayout = amountRow.gameObject.AddComponent<HorizontalLayoutGroup>();
		amountLayout.childAlignment = TextAnchor.MiddleLeft;
		amountLayout.childControlWidth = true;
		amountLayout.childControlHeight = true;
		amountLayout.childForceExpandWidth = false;
		amountLayout.childForceExpandHeight = true;
		amountLayout.spacing = 4f;
		LayoutElement amountElement = amountRow.gameObject.AddComponent<LayoutElement>();
		amountElement.preferredHeight = 18f;

		RectTransform amountIcon = NewUI("coin icon", amountRow);
		AddSprite(amountIcon, LoadShopSprite("coin"), false, true, Image.Type.Simple);
		LayoutElement amountIconLayout = amountIcon.gameObject.AddComponent<LayoutElement>();
		amountIconLayout.preferredWidth = 14f;
		amountIconLayout.preferredHeight = 14f;

		RectTransform amount = NewUI("amount text", amountRow);
		AddText(amount, font, pack.Points.ToString(), 13, TextAnchor.MiddleLeft, Navy);
		LayoutElement amountTextLayout = amount.gameObject.AddComponent<LayoutElement>();
		amountTextLayout.preferredWidth = 48f;

		RectTransform buyColumn = NewUI("buy column", row);
		VerticalLayoutGroup buyLayout = buyColumn.gameObject.AddComponent<VerticalLayoutGroup>();
		buyLayout.childAlignment = TextAnchor.MiddleCenter;
		buyLayout.childControlWidth = true;
		buyLayout.childControlHeight = true;
		buyLayout.childForceExpandWidth = true;
		buyLayout.childForceExpandHeight = false;
		buyLayout.spacing = 1f;
		LayoutElement buyColumnLayout = buyColumn.gameObject.AddComponent<LayoutElement>();
		buyColumnLayout.preferredWidth = 72f;
		buyColumnLayout.flexibleWidth = 0f;

		RectTransform buy = NewUI("buy button", buyColumn);
		Image buyImage = AddSprite(buy, RoundedSprite(), true, false, Image.Type.Sliced);
		buyImage.color = Green;
		Button buyButton = buy.gameObject.AddComponent<Button>();
		buyButton.targetGraphic = buyImage;
		LayoutElement buyElement = buy.gameObject.AddComponent<LayoutElement>();
		buyElement.preferredHeight = 32f;

		RectTransform buyLabel = NewUI("label", buy);
		Stretch(buyLabel);
		AddText(buyLabel, font, "Buy", 13, TextAnchor.MiddleCenter, White);

		RectTransform price = NewUI("price text", buyColumn);
		Text priceText = AddText(price, font, pack.PricePlaceholder, 11, TextAnchor.MiddleCenter, PriceGray);
		LayoutElement priceElement = price.gameObject.AddComponent<LayoutElement>();
		priceElement.preferredHeight = 14f;

		IAPButton iapButton = buy.gameObject.AddComponent<IAPButton>();
		iapButton.productId = pack.ProductId;
		iapButton.priceText = priceText;
	}

	private static void CreateShopButton(Transform parent, ShopScreenIAP shop)
	{
		if (parent == null || shop == null)
			return;

		RectTransform buttonRect = NewUI(ShopButtonName, parent);
		Place(buttonRect, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-8f, -8f), new Vector2(56f, 56f));
		Image image = AddSprite(buttonRect, LoadShopSprite("shop"), true, true, Image.Type.Simple);
		Button button = buttonRect.gameObject.AddComponent<Button>();
		button.targetGraphic = image;
		button.onClick.AddListener(shop.Open);
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

	private static Font LoadFont()
	{
		Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
		if (font == null)
			font = Resources.GetBuiltinResource<Font>("Arial.ttf");

		return font;
	}

	private static Sprite LoadShopSprite(string name)
	{
		Sprite sprite = Resources.Load<Sprite>("ShopIAP/" + name);
		if (sprite == null)
			Debug.LogWarning("[ShopScreen] Thiếu sprite Resources/ShopIAP/" + name);

		return sprite;
	}

	/// <summary>
	/// Ô trắng đổ màu, bo góc mượt. Viền 9-slice nhỏ hơn chiều cao nút Buy nên không bị vỡ góc.
	/// </summary>
	private static Sprite RoundedSprite()
	{
		if (_rounded != null)
			return _rounded;

		const int size = 256;
		const float radius = 64f;
		const float cornerUi = 12f;
		const float canvasPixelsPerUnit = 100f;
		float pixelsPerUnit = radius * canvasPixelsPerUnit / cornerUi;
		float antialias = 1.6f * (radius / cornerUi);

		Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
		texture.filterMode = FilterMode.Bilinear;
		texture.wrapMode = TextureWrapMode.Clamp;
		Color32[] pixels = new Color32[size * size];
		float half = size * 0.5f;
		float box = half - radius;

		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				float px = Mathf.Abs(x + 0.5f - half) - box;
				float py = Mathf.Abs(y + 0.5f - half) - box;
				float outside = Mathf.Sqrt(Mathf.Max(px, 0f) * Mathf.Max(px, 0f) + Mathf.Max(py, 0f) * Mathf.Max(py, 0f));
				float inside = Mathf.Min(Mathf.Max(px, py), 0f);
				float signed = outside + inside - radius;
				float alpha = Mathf.Clamp01(0.5f - signed / antialias);
				byte a = (byte)Mathf.RoundToInt(alpha * 255f);
				pixels[y * size + x] = new Color32(255, 255, 255, a);
			}
		}

		texture.SetPixels32(pixels);
		texture.Apply();
		_rounded = Sprite.Create(
			texture,
			new Rect(0f, 0f, size, size),
			new Vector2(0.5f, 0.5f),
			pixelsPerUnit,
			0,
			SpriteMeshType.FullRect,
			new Vector4(radius, radius, radius, radius));
		return _rounded;
	}

	private static RectTransform NewUI(string name, Transform parent)
	{
		GameObject go = new GameObject(name, typeof(RectTransform));
		go.layer = 5;
		RectTransform rect = go.GetComponent<RectTransform>();
		rect.SetParent(parent, false);
		return rect;
	}

	private static void Stretch(RectTransform rect)
	{
		rect.anchorMin = Vector2.zero;
		rect.anchorMax = Vector2.one;
		rect.offsetMin = Vector2.zero;
		rect.offsetMax = Vector2.zero;
		rect.pivot = new Vector2(0.5f, 0.5f);
	}

	private static RectTransform TopCenter(string name, Transform parent, float yFromTop, float width, float height)
	{
		RectTransform rect = NewUI(name, parent);
		Place(rect, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -yFromTop), new Vector2(width, height));
		return rect;
	}

	private static void Place(RectTransform rect, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
	{
		rect.anchorMin = anchor;
		rect.anchorMax = anchor;
		rect.pivot = pivot;
		rect.anchoredPosition = position;
		rect.sizeDelta = size;
	}

	private static Image AddSprite(RectTransform rect, Sprite sprite, bool raycast, bool preserveAspect, Image.Type type)
	{
		Image image = rect.gameObject.AddComponent<Image>();
		image.sprite = sprite;
		image.color = White;
		image.preserveAspect = preserveAspect;
		image.type = sprite != null ? type : Image.Type.Simple;
		image.raycastTarget = raycast;
		return image;
	}

	private static Text AddText(RectTransform rect, Font font, string value, int size, TextAnchor alignment, Color color)
	{
		Text text = rect.gameObject.AddComponent<Text>();
		text.font = font;
		text.text = value;
		text.fontSize = size;
		text.alignment = alignment;
		text.color = color;
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.verticalOverflow = VerticalWrapMode.Overflow;
		text.raycastTarget = false;
		return text;
	}
}
