using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CompareTower : MonoBehaviour
{
	private sealed class _CameraShot_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal CompareTower _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CameraShot_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.towerRendCamera.gameObject.SetActive(true);
				this._current = new WaitForEndOfFrame();
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.towerRendCamera.gameObject.SetActive(false);
				this._this.GetImageSize();
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _Measure_c__AnonStorey1
	{
		internal float meter;

		internal CompareTower _this;

		internal float __m__0()
		{
			return this.meter;
		}

		internal void __m__1(float x)
		{
			this.meter = x;
		}

		internal void __m__2()
		{
			this._this.measureMeter.text = this.meter.ToString("0.0") + LevelManager.instance.currentLevelValues.compareUnit;
		}

		internal void __m__3()
		{
			this._this.nextButton.SetActive(true);
		}
	}

	public static CompareTower instance;

	[SerializeField]
	private RectTransform canvas;

	[SerializeField]
	private Camera towerRendCamera;

	[SerializeField]
	private RenderTexture tubeRendTexture;

	[SerializeField]
	private List<GameObject> towerPrefabs;

	[SerializeField]
	private Material mainTubeMaterial;

	private GameObject tower;

	private GameObject currentTowerPrefab;

	private CompareTube[] towerChildTubes;

	[SerializeField]
	private RectTransform picturePanel;

	[SerializeField]
	private RectTransform tubePanel;

	[SerializeField]
	private Image compareImage;

	[SerializeField]
	private RectTransform tubeImage;

	[SerializeField]
	private RectTransform tubeImageBlur;

	[SerializeField]
	public RectTransform compareImageBlur;

	[SerializeField]
	private Text compareImageName;

	[SerializeField]
	private ParticleSystem compareTowerParticle;

	[Header("Measure"), SerializeField]
	private RectTransform measureTop;

	[SerializeField]
	private RectTransform measureBody;

	[SerializeField]
	private Text measureMeter;

	[Header(" "), SerializeField]
	private GameObject nextButton;

	private void Awake()
	{
		CompareTower.instance = this;
	}

	private void Start()
	{
		this.SetPanelSize();
	}

	private void Update()
	{
	}

	private void SetPanelSize()
	{
		this.picturePanel.sizeDelta = new Vector2(this.canvas.sizeDelta.x / 2f - 25f, this.picturePanel.sizeDelta.y);
		this.tubePanel.sizeDelta = new Vector2(this.canvas.sizeDelta.x / 2f - 25f, this.tubePanel.sizeDelta.y);
	}

	public void StartCompare()
	{
		this.OnPlay();
		this.GetChilds();
	}

	public void OnPlay()
	{
		this.nextButton.SetActive(false);
		this.measureBody.DOKill(false);
		this.measureTop.DOKill(false);
		this.measureBody.sizeDelta = new Vector2(this.measureBody.sizeDelta.x, 0f);
		this.measureTop.sizeDelta = new Vector2(0f, this.measureTop.sizeDelta.y);
		this.tubeImage.sizeDelta = new Vector2(0f, 0f);
		this.tubeImageBlur.sizeDelta = new Vector2(0f, 0f);
		this.measureMeter.text = "0";
		this.compareImage.sprite = LevelManager.instance.currentLevelValues.compareSprite;
		this.compareImageBlur.sizeDelta = LevelManager.instance.currentLevelValues.blurSizeDelta;
		this.compareImageBlur.anchoredPosition = LevelManager.instance.currentLevelValues.blurAnchorPosition;
		this.compareImageBlur.GetComponent<Image>().color = LevelManager.instance.currentLevelValues.blurColor;
		this.compareImageName.text = LevelManager.instance.currentLevelValues.compareSpriteName;
	}

	private void GetTowerType()
	{
		for (int i = 0; i < this.towerPrefabs.Count; i++)
		{
			if (this.towerPrefabs[i].CompareTag(Generator.instance.tubePrefab.transform.tag))
			{
				this.currentTowerPrefab = this.towerPrefabs[i];
			}
		}
	}

	private void GetChilds()
	{
		if (this.tower != null)
		{
			UnityEngine.Object.Destroy(this.tower);
		}
		this.GetTowerType();
		this.tower = UnityEngine.Object.Instantiate<GameObject>(this.currentTowerPrefab);
		this.towerChildTubes = this.tower.GetComponentsInChildren<CompareTube>();
		this.SetColors();
		base.StartCoroutine("CameraShot");
	}

	private void SetColors()
	{
		for (int i = 0; i < this.towerChildTubes.Length; i++)
		{
			float time = 1f - (float)i / (float)(this.towerChildTubes.Length - 1);
			Color color = Generator.instance.gradient.Evaluate(time);
			this.towerChildTubes[i].mainTubeRend.material = this.mainTubeMaterial;
			this.towerChildTubes[i].mainTubeRend.material.color = color;
			for (int j = 0; j < this.towerChildTubes[i].foots.childCount; j++)
			{
				this.towerChildTubes[i].foots.GetChild(j).GetComponent<MeshRenderer>().material.color = color;
			}
		}
	}

	private IEnumerator CameraShot()
	{
		CompareTower._CameraShot_c__Iterator0 _CameraShot_c__Iterator = new CompareTower._CameraShot_c__Iterator0();
		_CameraShot_c__Iterator._this = this;
		return _CameraShot_c__Iterator;
	}

	private void GetImageSize()
	{
		RectTransform component = MenuManager.instance.comparePanel.GetComponent<RectTransform>();
		float num;
		if (this.compareImage.sprite.textureRect.size.x / this.compareImage.sprite.textureRect.size.y > this.picturePanel.sizeDelta.x / this.picturePanel.sizeDelta.y)
		{
			num = this.picturePanel.sizeDelta.x / this.compareImage.sprite.textureRect.size.x * this.compareImage.sprite.textureRect.size.y;
			float num2 = this.picturePanel.sizeDelta.y - num;
			component.offsetMax = new Vector2(0f, 0f);
			component.offsetMax = new Vector2(component.anchoredPosition.x, component.anchoredPosition.y + num2);
		}
		else
		{
			component.offsetMax = new Vector2(0f, 0f);
			num = this.picturePanel.sizeDelta.y;
		}
		this.tubeImage.DOSizeDelta(new Vector2(num, num), 1f, false).OnComplete(new TweenCallback(this.Measure));
		this.tubeImageBlur.DOSizeDelta(new Vector2(num / 1.3f, num / 2f), 1f, false);
		this.compareTowerParticle.Play();
	}

	private void Measure()
	{
		float meter = 0f;
		Sequence s = DOTween.Sequence();
		s.Append(this.measureBody.DOSizeDelta(new Vector2(this.measureBody.sizeDelta.x, this.tubeImage.sizeDelta.y * 10f), 2f, false));
		s.Join(DOTween.To(() => meter, delegate(float x)
		{
			meter = x;
		}, LevelManager.instance.currentLevelValues.compareMeter, 2f).OnUpdate(delegate
		{
			this.measureMeter.text = meter.ToString("0.0") + LevelManager.instance.currentLevelValues.compareUnit;
		}));
		s.Append(this.measureTop.DOSizeDelta(new Vector2(200f, this.measureTop.sizeDelta.y), 0.3f, false));
		s.AppendCallback(delegate
		{
			this.nextButton.SetActive(true);
		});
	}

	public void Test()
	{
		if (LevelManager.instance.testLevel.text != string.Empty)
		{
			LevelManager.instance.currentLevel = int.Parse(LevelManager.instance.testLevel.text);
			if (LevelManager.instance.currentLevel <= LevelManager.instance.levels.Count)
			{
				LevelManager.instance.GetLevelValues();
			}
		}
		this.compareImage.sprite = LevelManager.instance.currentLevelValues.compareSprite;
		this.compareImageBlur.sizeDelta = LevelManager.instance.currentLevelValues.blurSizeDelta;
		this.compareImageBlur.anchoredPosition = LevelManager.instance.currentLevelValues.blurAnchorPosition;
		this.compareImageBlur.GetComponent<Image>().color = LevelManager.instance.currentLevelValues.blurColor;
		this.compareImageName.text = LevelManager.instance.currentLevelValues.compareSpriteName;
		this.GetImageSize();
	}
}
