using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rateus : MonoBehaviour
{
	public static Rateus instance;

	[SerializeField]
	private Color selectedStarColor;

	[SerializeField]
	private Color unselectedStarColor;

	[SerializeField]
	private List<Image> stars;

	[SerializeField]
	private GameObject rateusPopup;

	private int rate;

	private void Start()
	{
		Rateus.instance = this;
	}

	public bool OnEscape()
	{
		if (this.rateusPopup.activeInHierarchy)
		{
			this.OnRateusButton();
			return true;
		}
		return false;
	}

	public void SelectStar(int id)
	{
		this.rate = id;
		for (int i = 0; i < this.stars.Count; i++)
		{
			this.stars[i].color = this.unselectedStarColor;
		}
		for (int j = 0; j < id; j++)
		{
			this.stars[j].color = this.selectedStarColor;
		}
	}

	public void OnRateusButton()
	{
		if (this.rateusPopup.activeInHierarchy)
		{
			this.rateusPopup.GetComponent<Image>().material.DOFloat(0f, "_Size", 0.2f);
			this.rateusPopup.GetComponent<CanvasGroup>().DOFade(0f, 0.2f).OnComplete(delegate
			{
				this.rateusPopup.SetActive(false);
			});
			this.OpenStoreRate();
		}
		else
		{
			if (PlayerPrefs.HasKey("rate") && PlayerPrefs.GetInt("rate") == 5)
			{
				return;
			}
			this.rateusPopup.SetActive(true);
			this.rateusPopup.GetComponent<Image>().material.DOFloat(2f, "_Size", 0.2f);
			this.rateusPopup.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
		}
	}

	private void OpenStoreRate()
	{
		if (this.rate < 5)
		{
			return;
		}
		PlayerPrefs.SetInt("rate", 5);
		PlayerPrefs.Save();
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
	}
}
