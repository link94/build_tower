using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Nút mua một gói IAP. Hiện giá nội tệ sau khi store khởi tạo xong.
/// </summary>
[RequireComponent(typeof(Button))]
public class IAPButton : MonoBehaviour
{
	[Tooltip("ID sản phẩm: iap1 … iap7.")]
	public string productId = "iap1";

	[Tooltip("Nếu có, sẽ cập nhật thành giá sau khi IAP init xong.")]
	public Text priceText;

	private Button _button;
	private string _placeholder;
	private bool _subscribed;
	private Coroutine _bindRoutine;

	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClick);

		if (priceText != null)
			_placeholder = priceText.text;
	}

	private void OnEnable()
	{
		TrySubscribeAndRefresh();

		if (!_subscribed || IAPManager.Instance == null || !IAPManager.Instance.IsInitialized)
		{
			if (_bindRoutine == null)
				_bindRoutine = StartCoroutine(BindWhenReady());
		}
	}

	private void OnDisable()
	{
		if (_bindRoutine != null)
		{
			StopCoroutine(_bindRoutine);
			_bindRoutine = null;
		}

		Unsubscribe();
	}

	private IEnumerator BindWhenReady()
	{
		while (IAPManager.Instance == null)
			yield return null;

		TrySubscribeAndRefresh();

		while (IAPManager.Instance != null && !IAPManager.Instance.IsInitialized)
			yield return null;

		UpdatePriceDisplay();
		_bindRoutine = null;
	}

	private void TrySubscribeAndRefresh()
	{
		if (IAPManager.Instance == null)
			return;

		if (!_subscribed)
		{
			IAPManager.Instance.OnIAPInitialized += UpdatePriceDisplay;
			_subscribed = true;
		}

		if (IAPManager.Instance.IsInitialized)
			UpdatePriceDisplay();
	}

	private void Unsubscribe()
	{
		if (!_subscribed || IAPManager.Instance == null)
		{
			_subscribed = false;
			return;
		}

		IAPManager.Instance.OnIAPInitialized -= UpdatePriceDisplay;
		_subscribed = false;
	}

	private void OnClick()
	{
		if (IAPManager.Instance == null)
		{
			Debug.LogWarning("[IAPButton] IAPManager chưa có trong scene.");
			return;
		}

		IAPManager.Instance.BuyProduct(productId);
	}

	public void RefreshPrice()
	{
		UpdatePriceDisplay();
	}

	private void UpdatePriceDisplay()
	{
		if (priceText == null || IAPManager.Instance == null)
			return;

		if (string.IsNullOrEmpty(_placeholder))
			_placeholder = priceText.text;

		var product = IAPManager.Instance.GetProduct(productId);
		string price = (product != null && product.metadata != null)
			? product.metadata.localizedPriceString
			: null;

		if (!HasRealPrice(price))
		{
			if (!string.IsNullOrEmpty(_placeholder))
				priceText.text = _placeholder;

			return;
		}

		priceText.text = price;
	}

	private static bool HasRealPrice(string price)
	{
		if (string.IsNullOrEmpty(price))
			return false;

		for (int i = 0; i < price.Length; i++)
		{
			if (price[i] >= '1' && price[i] <= '9')
				return true;
		}

		return false;
	}
}
