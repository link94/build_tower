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

	private int score;

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
		this.score = 0;
		this.scoreText.text = this.score.ToString();
	}

	private void Update()
	{
	}

	public void GetScore(int getedScore)
	{
		this.score += getedScore;
		this.scoreText.text = this.score.ToString();
		this.scoreText.transform.DOScale(1.2f, 0.2f).OnComplete(delegate
		{
			this.scoreText.transform.DOScale(1f, 0.2f);
		});
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.scorePopupPrefab);
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, Generator.instance.currentTube.startPositionY, gameObject.transform.position.z);
		gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + getedScore.ToString();
	}
}
