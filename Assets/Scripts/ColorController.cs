using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
	public static ColorController instance;

	[SerializeField]
	private List<Colors> Colors;

	[Header("TEST"), SerializeField]
	private bool test;

	[SerializeField]
	private int colorID;

	[SerializeField]
	private InputField testColorID;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		ColorController.instance = this;
	}

	private void Start()
	{
		if (this.test)
		{
			this.testColorID.gameObject.SetActive(true);
		}
		else
		{
			this.testColorID.gameObject.SetActive(false);
		}
		EventManager.OnLevelComplete += new EventManager.LevelComplete(this.OnLevelComplete);
		this.Load();
		this.SetColor();
	}

	private void Update()
	{
	}

	public void SetColor()
	{
		Sequence s = DOTween.Sequence();
		s.AppendInterval(0.2f);
		//s.Append(Camera.main.DOColor(this.Colors[this.colorID].cameraBackgroundColor, 1f));
		//s.Join(BackgroundGenerator.instance.tubeMat.DOColor(this.Colors[this.colorID].cameraBackgroundColor, "_FogColor", 1f));
		//s.Join(BackgroundGenerator.instance.tubeMat.DOColor(this.Colors[this.colorID].backgroundTubesFrontColor, "_FrontColor", 1f));
		s.AppendCallback(delegate
		{
			MenuManager.instance.comparePanel.GetComponent<Image>().color = this.Colors[this.colorID].cameraBackgroundColor;
		});
		LevelManager.instance.levelBarFill.color = this.Colors[this.colorID].levelBarColor;
		Generator.instance.gradient = this.Colors[this.colorID].gradient;
	}

	private void OnLevelComplete()
	{
		this.colorID++;
		if (this.colorID == this.Colors.Count)
		{
			this.colorID = 0;
		}
		PlayerPrefs.SetInt("colorID", this.colorID);
		PlayerPrefs.Save();
	}

	private void Load()
	{
		if (PlayerPrefs.HasKey("colorID"))
		{
			this.colorID = PlayerPrefs.GetInt("colorID");
		}
		else
		{
			this.colorID = 0;
		}
	}

	public void Test()
	{
		if (this.testColorID.text != string.Empty)
		{
			this.colorID = int.Parse(this.testColorID.text);
			if (this.colorID < this.Colors.Count)
			{
				this.SetColor();
			}
		}
	}
}
