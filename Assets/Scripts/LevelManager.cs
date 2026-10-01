using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;

	[SerializeField]
	public List<Level> levels;

	[SerializeField]
	public int currentLevel = 1;

	[SerializeField]
	public Image levelBarFill;

	[SerializeField]
	private Text levelBarText;

	[SerializeField]
	private Text levelTextOnLevelComplete;

	public Level currentLevelValues;

	public bool levelComplete;

	private int generatedTubesCount;

	[Header("LevelCompletePercent"), SerializeField]
	private Text levelCompletePercentText;

	public int levelCompletePercent;

	private GameObject percentText;

	[Header("TEST"), SerializeField]
	private bool test;

	[SerializeField]
	public InputField testLevel;

	private void Awake()
	{
		LevelManager.instance = this;
		this.Load();
	}

	private void Start()
	{
		if (!this.test)
		{
			this.testLevel.gameObject.SetActive(false);
		}
		else
		{
			this.testLevel.gameObject.SetActive(true);
		}
		EventManager.OnPlay += new EventManager.Play(this.OnPlay);
		EventManager.OnGameOver += new EventManager.GameOver(this.OnGameOver);
		this.levelBarFill.fillAmount = 0f;
	}

	private void Update()
	{
	}

	public void PlayNextLevel()
	{
		this.currentLevel++;
		if (this.currentLevel == this.levels.Count + 1)
		{
			this.currentLevel = 1;
		}
		PlayerPrefs.SetInt("currentLevel", this.currentLevel);
		PlayerPrefs.Save();
		this.GetLevelValues();
	}

	public void GetLevelValues()
	{
		this.currentLevelValues = this.levels[this.currentLevel - 1];
		this.levelBarText.text = "Level " + this.currentLevel;
		this.levelTextOnLevelComplete.text = this.currentLevel.ToString();
	}

	public bool LevelPass()
	{
		if (this.generatedTubesCount == this.currentLevelValues.generatableTubesCount)
		{
			this.levelBarFill.DOFillAmount(1f, 0.5f);
			this.levelComplete = true;
			EventManager.CallOnLevelComplete();
			return true;
		}
		this.generatedTubesCount++;
		float num = (float)(this.generatedTubesCount - 1) / (float)this.currentLevelValues.generatableTubesCount;
		this.levelBarFill.DOFillAmount(num, 0.5f);
		this.levelCompletePercent = (int)(num * 100f);
		return false;
	}

	public void OnPlay()
	{
		if (this.test && this.testLevel.text != string.Empty)
		{
			this.currentLevel = int.Parse(this.testLevel.text);
			if (this.currentLevel < this.levels.Count)
			{
				this.GetLevelValues();
			}
		}
		this.generatedTubesCount = 0;
		this.levelBarFill.DOKill(false);
		this.levelBarFill.fillAmount = 0f;
		this.levelComplete = false;
		if (this.percentText != null)
		{
			this.percentText.transform.DOKill(false);
			UnityEngine.Object.Destroy(this.percentText);
		}
	}

	private void OnGameOver()
	{
		if (this.levelCompletePercent >= 60)
		{
			this.levelCompletePercentText.text = this.levelCompletePercent.ToString() + "%";
			MenuManager.instance.gameOverPanel.SetActive(true);
		}
	}

	private void Load()
	{
		if (PlayerPrefs.HasKey("currentLevel"))
		{
			this.currentLevel = PlayerPrefs.GetInt("currentLevel");
		}
		else
		{
			this.currentLevel = 1;
		}
		this.GetLevelValues();
	}
}
