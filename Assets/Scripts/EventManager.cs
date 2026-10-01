using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public delegate void Play();

	public delegate void LevelComplete();

	public delegate void GameOver();

	public delegate void Compare();









	public static event EventManager.Play OnPlay;

	public static event EventManager.LevelComplete OnLevelComplete;

	public static event EventManager.GameOver OnGameOver;

	public static event EventManager.Compare OnCompare;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static void CallOnPlay()
	{
		if (EventManager.OnPlay != null)
		{
			EventManager.OnPlay();
		}
	}

	public static void CallOnLevelComplete()
	{
		if (EventManager.OnLevelComplete != null)
		{
			EventManager.OnLevelComplete();
		}
	}

	public static void CallOnGameOver()
	{
		if (EventManager.OnGameOver != null)
		{
			EventManager.OnGameOver();
		}
	}

	public static void CallOnCompare()
	{
		if (EventManager.OnCompare != null)
		{
			EventManager.OnCompare();
		}
	}
}
