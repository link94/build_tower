using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

// Unity IAP 5.x đánh dấu API coded IAP cũ là [Obsolete] nhưng vẫn chạy đủ.
#pragma warning disable CS0618

/// <summary>
/// Google Play Billing qua Unity Purchasing. Mua thành công thì cộng đúng số điểm của gói.
/// </summary>
public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
	public static IAPManager Instance { get; private set; }

	[Header("Local validation (receipt) – bật nếu dùng CrossPlatformValidator")]
	public bool isLocalValidation;

	[Header("Loại sản phẩm")]
	[Tooltip("false = NonConsumable (mỗi tài khoản chỉ mua được 1 lần).\n" +
	         "true = Consumable (mua lại nhiều lần) – chỉ bật khi sản phẩm trên store cũng là consumable.")]
	public bool pointPacksAreConsumable = true;

	private IStoreController _storeController;
	private IExtensionProvider _extensionProvider;
	private bool _isInitialized;

	public bool IsInitialized => _isInitialized;

	public event Action<string> OnPurchaseSuccess;
	public event Action<string, string> PurchaseFailed;
	public event Action OnIAPInitialized;
	public event Action<string> OnIAPInitializeFailed;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
		InitializePurchasing();
	}

	private void OnDestroy()
	{
		if (Instance == this)
			Instance = null;
	}

	private void InitializePurchasing()
	{
		if (_isInitialized)
			return;

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		ProductType type = pointPacksAreConsumable ? ProductType.Consumable : ProductType.NonConsumable;

		for (int i = 0; i < IAPCatalog.Packs.Length; i++)
			builder.AddProduct(IAPCatalog.Packs[i].ProductId, type);

		Debug.Log($"[IAP] Initialize – đăng ký {IAPCatalog.Packs.Length} product ({type}).");
		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		_storeController = controller;
		_extensionProvider = extensions;
		_isInitialized = true;
		Debug.Log("[IAP] Initialized.");
		OnIAPInitialized?.Invoke();
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogWarning($"[IAP] Init failed: {error}");
		OnIAPInitializeFailed?.Invoke(error.ToString());
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		Debug.LogWarning($"[IAP] Init failed: {error} - {message}");
		OnIAPInitializeFailed?.Invoke($"{error}: {message}");
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		var product = args.purchasedProduct;
		string id = product.definition.id;
		Debug.Log($"[IAP] Purchase OK: {id}");

		if (isLocalValidation)
		{
			// Có thể thêm CrossPlatformValidator trước khi cộng điểm.
		}

		int points = IAPCatalog.GetPoints(id);
		if (points > 0)
		{
			if (GameManager.instance != null)
				GameManager.instance.AddPurchasedScore(points);
			else
				GameManager.StorePurchasedScore(points);

			Debug.Log($"[IAP] Cộng {points} điểm cho {id}.");
		}

		PlayerPrefs.SetInt("IAP_" + id, 1);
		PlayerPrefs.Save();
		OnPurchaseSuccess?.Invoke(id);
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.LogWarning($"[IAP] Purchase failed: {product.definition.id} - {failureReason}");
		PurchaseFailed?.Invoke(product.definition.id, failureReason.ToString());
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
	{
		Debug.LogWarning($"[IAP] Purchase failed: {product.definition.id} - {failureDescription.reason} {failureDescription.message}");
		PurchaseFailed?.Invoke(product.definition.id, failureDescription.message);
	}

	public void BuyProduct(string productId)
	{
		if (!_isInitialized || _storeController == null)
		{
			Debug.LogWarning("[IAP] Chưa khởi tạo. Đợi hoặc kiểm tra kết nối.");
			PurchaseFailed?.Invoke(productId, "Not initialized");
			return;
		}

		Product product = _storeController.products.WithID(productId);
		if (product != null && product.availableToPurchase)
		{
			_storeController.InitiatePurchase(product);
		}
		else
		{
			Debug.LogWarning($"[IAP] Product không tồn tại hoặc không khả dụng: {productId}");
			PurchaseFailed?.Invoke(productId, "Product not available");
		}
	}

	public void RestorePurchases()
	{
		if (!_isInitialized || _extensionProvider == null)
			return;

		var apple = _extensionProvider.GetExtension<IAppleExtensions>();
		if (apple != null)
			apple.RestoreTransactions(OnRestoreFinished);
		else
			Debug.Log("[IAP] Restore chỉ hỗ trợ trên Apple.");
	}

	private void OnRestoreFinished(bool success, string message)
	{
		Debug.Log($"[IAP] Restore finished: {success} - {message}");
	}

	public Product GetProduct(string productId)
	{
		if (!_isInitialized || _storeController == null)
			return null;

		return _storeController.products.WithID(productId);
	}

	public static bool HasPurchased(string productId)
	{
		return PlayerPrefs.GetInt("IAP_" + productId, 0) == 1;
	}
}
