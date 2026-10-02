using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Màn cửa hàng gói điểm. Bật/tắt cả GameObject, không đi qua stack menu.
/// </summary>
public class ShopScreenIAP : MonoBehaviour
{
	public static ShopScreenIAP Instance { get; private set; }

	public bool IsOpen => gameObject.activeSelf;

	[SerializeField]
	private Text _scoreText;

	[SerializeField]
	private Button _backButton;

	[SerializeField]
	private Text _messageText;

	private void Awake()
	{
		Instance = this;
		HookCloseButton();
	}

	public void Bind(Text scoreText, Button backButton, Text messageText)
	{
		_scoreText = scoreText;
		_backButton = backButton;
		_messageText = messageText;
		HookCloseButton();
	}

	private void HookCloseButton()
	{
		if (_backButton == null)
			return;

		_backButton.onClick.RemoveListener(Close);
		_backButton.onClick.AddListener(Close);
	}

	private void OnDestroy()
	{
		if (Instance == this)
			Instance = null;
	}

	private void OnEnable()
	{
		if (_messageText != null)
			_messageText.text = string.Empty;

		RefreshScore();
		RefreshIapPrices();
		Subscribe();
	}

	private void OnDisable()
	{
		Unsubscribe();
	}

	public void Open()
	{
		gameObject.SetActive(true);
		transform.SetAsLastSibling();
		if (Generator.instance != null)
			Generator.instance.SetGameplayPaused(true);
	}

	public void Close()
	{
		gameObject.SetActive(false);
		if (Generator.instance != null)
			Generator.instance.SetGameplayPaused(false);
	}

	public static bool IsBlockingInput()
	{
		return Instance != null && Instance.IsOpen;
	}

	private void Subscribe()
	{
		if (IAPManager.Instance == null)
			return;

		IAPManager.Instance.PurchaseFailed -= OnPurchaseFailed;
		IAPManager.Instance.PurchaseFailed += OnPurchaseFailed;
		IAPManager.Instance.OnPurchaseSuccess -= OnPurchaseSuccess;
		IAPManager.Instance.OnPurchaseSuccess += OnPurchaseSuccess;
	}

	private void Unsubscribe()
	{
		if (IAPManager.Instance == null)
			return;

		IAPManager.Instance.PurchaseFailed -= OnPurchaseFailed;
		IAPManager.Instance.OnPurchaseSuccess -= OnPurchaseSuccess;
	}

	private void OnPurchaseSuccess(string productId)
	{
		RefreshScore();

		if (_messageText != null)
			_messageText.text = string.Empty;
	}

	private void OnPurchaseFailed(string productId, string reason)
	{
		if (_messageText == null)
			return;

		_messageText.text = "Mua không thành công. Vui lòng thử lại.";
	}

	private void RefreshScore()
	{
		if (_scoreText == null)
			return;

		int score = GameManager.instance != null ? GameManager.instance.Score : 0;
		_scoreText.text = score.ToString();
	}

	private void RefreshIapPrices()
	{
		IAPButton[] buttons = GetComponentsInChildren<IAPButton>(true);
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].RefreshPrice();
	}
}
