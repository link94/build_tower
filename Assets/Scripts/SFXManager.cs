using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	private sealed class _PlayNote_c__AnonStorey0
	{
		internal GameObject audio;

		internal void __m__0()
		{
			UnityEngine.Object.Destroy(this.audio);
		}
	}

	public static SFXManager instance;

	[Range(0f, 1f), SerializeField]
	private float volume;

	[Range(0f, 1f), SerializeField]
	private float volumeSFX;

	[Range(0f, 1f), SerializeField]
	private float volumePerfectSFX;

	[SerializeField]
	private List<AudioClip> firstNotes;

	[SerializeField]
	private List<AudioClip> secondNotes;

	[SerializeField]
	private List<AudioClip> perfectNotes;

	[SerializeField]
	private AudioClip wrongMove;

	[SerializeField]
	private AudioSource uiClick;

	[Header("OST"), SerializeField]
	private AudioSource ost;

	[SerializeField]
	private AudioLowPassFilter ostLowPassFilter;

	[SerializeField]
	private float lowPassFrequenceOnMenu;

	[SerializeField]
	private float lowPassFrequenceOnGame;

	private float currentClipVolume;

	private int clipID;

	private int perfectClipID;

	private List<AudioClip> currentList;

	public float ddd;

	private void Awake()
	{
		SFXManager.instance = this;
	}

	private void Start()
	{
		EventManager.OnPlay += new EventManager.Play(this.OnPlay);
		EventManager.OnGameOver += new EventManager.GameOver(this.OnGameOver);
		EventManager.OnLevelComplete += new EventManager.LevelComplete(this.OnGameOver);
		this.currentList = this.firstNotes;
	}

	private void Update()
	{
	}

	public void PLaySfx()
	{
		this.currentClipVolume = this.volumeSFX;
		this.perfectClipID = 0;
		this.PlayNote(this.currentList[this.clipID]);
		this.clipID++;
		if (this.clipID == this.currentList.Count)
		{
			if (this.currentList == this.firstNotes)
			{
				this.currentList = this.secondNotes;
			}
			else
			{
				this.currentList = this.firstNotes;
			}
			this.clipID = 0;
		}
	}

	public void PlayPerfectSfx()
	{
		this.currentClipVolume = this.volumePerfectSFX;
		this.clipID = 0;
		this.PlayNote(this.perfectNotes[this.perfectClipID]);
		this.perfectClipID++;
		if (this.perfectClipID == this.perfectNotes.Count)
		{
			this.perfectClipID = 2;
		}
	}

	public void PlayGameOverSfx()
	{
		this.PlayNote(this.wrongMove);
	}

	private void PlayNote(AudioClip clip)
	{
		GameObject audio = new GameObject();
		AudioSource audioSource = audio.AddComponent<AudioSource>();
		audioSource.volume = this.currentClipVolume;
		audioSource.clip = clip;
		audioSource.Play();
		DOVirtual.DelayedCall(clip.length, delegate
		{
			UnityEngine.Object.Destroy(audio);
		}, true);
	}

	private void OnPlay()
	{
		if (!this.ost.isPlaying)
		{
			this.ost.Play();
		}
		DOVirtual.Float(this.ostLowPassFilter.cutoffFrequency, this.lowPassFrequenceOnGame, 2f, delegate(float t)
		{
			this.ostLowPassFilter.cutoffFrequency = t;
		});
	}

	public void OnGameOver()
	{
		DOVirtual.Float(this.ostLowPassFilter.cutoffFrequency, this.lowPassFrequenceOnMenu, 2f, delegate(float t)
		{
			this.ostLowPassFilter.cutoffFrequency = t;
		});
		this.clipID = 0;
		this.perfectClipID = 0;
	}

	public void PlayUIClick()
	{
		this.uiClick.Play();
	}
}
