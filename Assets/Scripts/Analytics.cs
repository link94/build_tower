//using GameAnalyticsSDK;
using System;
using UnityEngine;

public class Analytics : MonoBehaviour
{
	public static Analytics instance;

	private void Awake()
	{
	}

	private void Start()
	{
	//GameAnalytics.Initialize();
		Analytics.instance = this;
		EventManager.OnPlay += new EventManager.Play(this.LevelStart);
		EventManager.OnGameOver += new EventManager.GameOver(this.LevelLost);
		EventManager.OnLevelComplete += new EventManager.LevelComplete(this.LevelComplete);
	}

	public void LevelStart()
	{
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_" + LevelManager.instance.currentLevel.ToString());
	}

	public void LevelComplete()
	{
	//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_" + LevelManager.instance.currentLevel.ToString());
	}

	public void LevelLost()
	{
	//	GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_" + LevelManager.instance.currentLevel.ToString());
	}
}
