using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField]
	private Text scoreText;

	[SerializeField]
	private GameObject scorePopupPrefab;

	private const string IapScoreKey = "iapScore";

	private int runScore;

	public int Score
	{
		get { return PlayerPrefs.GetInt(IapScoreKey, 0); }
	}

	[InspectorButton("OnButtonClicked")]
	public bool DeletePlayerPrefs;

	private void OnButtonClicked()
	{
		PlayerPrefs.DeleteAll();
		UnityEngine.Debug.Log("PlayerPrefs Deleted");
	}

	private void Awake()
	{
		Application.targetFrameRate = 60;
		GameManager.instance = this;
	}

	private void Start()
	{
		this.ResetRunScore();
	}

	private void Update()
	{
	}

	public void ResetRunScore()
	{
		this.runScore = 0;
		this.ShowRunScore();
	}

	public void GetScore(int getedScore)
	{
		this.runScore += getedScore;
		this.ShowRunScore();
		this.scoreText.transform.DOScale(1.2f, 0.2f).OnComplete(delegate
		{
			this.scoreText.transform.DOScale(1f, 0.2f);
		});
		GameObject popup = UnityEngine.Object.Instantiate<GameObject>(this.scorePopupPrefab);
		popup.transform.position = new Vector3(popup.transform.position.x, Generator.instance.currentTube.startPositionY, popup.transform.position.z);
		popup.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + getedScore.ToString();
		Generator.instance.RegisterFloatingText(popup);
	}

	/// <summary>
	/// Cộng điểm một gói IAP vào HUD và lưu riêng, để lần mở game sau vẫn còn.
	/// Điểm xếp ống trong phiên không ghi vào số này.
	/// </summary>
	public void AddPurchasedScore(int points)
	{
		if (points <= 0)
			return;

		int stored = PlayerPrefs.GetInt(IapScoreKey, 0) + points;
		PlayerPrefs.SetInt(IapScoreKey, stored);
		PlayerPrefs.Save();

	}

	private void ShowRunScore()
	{
		if (this.scoreText == null)
			return;

		this.scoreText.text = this.runScore.ToString();
	}

	/// <summary>Ghi điểm mua khi GameManager chưa kịp có instance.</summary>
	public static void StorePurchasedScore(int points)
	{
		if (points <= 0)
			return;

		int stored = PlayerPrefs.GetInt(IapScoreKey, 0) + points;
		PlayerPrefs.SetInt(IapScoreKey, stored);
		PlayerPrefs.Save();
	}
}
