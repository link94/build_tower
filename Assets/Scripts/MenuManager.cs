//using ArmNomads.Haptic;
using DG.Tweening;
//using FantomLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	private sealed class _PlayButton_c__AnonStorey3
	{
		internal Color zeroAlpha;

		internal MenuManager _this;

		internal void __m__0()
		{
			this._this.fade.DOColor(this.zeroAlpha, 0.2f).OnComplete(delegate
			{
				this._this.fade.gameObject.SetActive(false);
				this._this.GamePlayPanelAnim(true);
			});
			this._this.SetPlayPanels();
		}

		internal void __m__1()
		{
			this._this.fade.gameObject.SetActive(false);
			this._this.GamePlayPanelAnim(true);
		}
	}

	private sealed class _Compare_c__AnonStorey4
	{
		internal CanvasGroup canvasGroup;

		internal float __m__0()
		{
			return this.canvasGroup.alpha;
		}

		internal void __m__1(float x)
		{
			this.canvasGroup.alpha = x;
		}
	}

	private sealed class _OpenFacebookPage_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal MenuManager _this;

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

		public _OpenFacebookPage_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.leftApp = false;
				Application.OpenURL("fb://page/1648649728735660");
				this._current = new WaitForSecondsRealtime(1f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this.leftApp)
				{
					this._this.leftApp = false;
				}
				else
				{
					Application.OpenURL("https://facebook.com/armnomads/");
				}
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

	private sealed class _OpenInstagramPage_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal MenuManager _this;

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

		public _OpenInstagramPage_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.leftApp = false;
				Application.OpenURL("instagram://user?username=armnomadsgames");
				this._current = new WaitForSecondsRealtime(1f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this.leftApp)
				{
					this._this.leftApp = false;
				}
				else
				{
					Application.OpenURL("https://instagram.com/armnomadsgames/");
				}
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

	private sealed class _OpenTwitterPage_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal MenuManager _this;

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

		public _OpenTwitterPage_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.leftApp = false;
				Application.OpenURL("twitter://user?user_id=3253237769");
				this._current = new WaitForSecondsRealtime(1f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this.leftApp)
				{
					this._this.leftApp = false;
				}
				else
				{
					Application.OpenURL("https://twitter.com/armnomads");
				}
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

	public static MenuManager instance;

	[SerializeField]
	private GameObject startPanel;

	[SerializeField]
	private GameObject gamePlayPanel;

	[SerializeField]
	private GameObject levelCompletePanel;

	[SerializeField]
	public GameObject comparePanel;

	[SerializeField]
	public GameObject gameOverPanel;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private Image fade;

	[Header("Buttons"), SerializeField]
	private Image soundButtonIcon;

	[SerializeField]
	private Sprite soundEnableIcon;

	[SerializeField]
	private Sprite soundDisableIcon;

	private bool sound;

	[SerializeField]
	private Image vibrationButtonIcon;

	[SerializeField]
	private Sprite vibrationEnableIcon;

	[SerializeField]
	private Sprite vibrationDisableIcon;

	public bool vibration;

	[SerializeField]
	private Image socialButtonIconImage;

	[SerializeField]
	private List<RectTransform> socialButtons;

	[SerializeField]
	private List<Sprite> socialButtonIcons;

	[SerializeField]
	private GameObject socialButtonCloseText;

	private bool social;

	[Header("GamePlayPanelAnim"), SerializeField]
	private RectTransform levelBar;

	[SerializeField]
	private RectTransform score;

	[Header("LevelCompletePanelAnim"), SerializeField]
	private RectTransform levelCompletePanelLevelText;

//	[SerializeField]
//	private YesNoDialogController exitDialog;

	private Sequence gamePlayAnimSeq;

	private bool leftApp;

	private int spriteID;

	private Sequence socialIconAnimPlay;

	private Sequence socialIconAnimStop;

	private Sequence socialAnim;

	private static TweenCallback __f__am_cache0;

	private void Awake()
	{
		MenuManager.instance = this;
		EventManager.OnLevelComplete += new EventManager.LevelComplete(this.OnLevelComplete);
		EventManager.OnLevelComplete += new EventManager.LevelComplete(this.CallRateUs);
		EventManager.OnGameOver += new EventManager.GameOver(this.OnGameOver);
	}

	private void Start()
	{
		this.Load();
		this.startPanel.SetActive(true);
		this.levelCompletePanel.SetActive(false);
		this.SocialButtonAnimPlay();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			this.EscButton();
		}
	}

	private void EscButton()
	{
		
	}

	public void ExitDialogYes()
	{
		Application.Quit();
	}

	public void PlayButton()
	{
		if (this.social)
		{
			this.OnSocialButton();
		}
		this.startPanel.SetActive(false);
		if (LevelManager.instance.levelComplete || Generator.instance.gameOver)
		{
			Color zeroAlpha = new Color(this.fade.color.r, this.fade.color.g, this.fade.color.b, 0f);
			this.fade.color = zeroAlpha;
			this.fade.gameObject.SetActive(true);
			this.fade.DOColor(Color.black, 0.2f).OnComplete(delegate
			{
				this.fade.DOColor(zeroAlpha, 0.2f).OnComplete(delegate
				{
					this.fade.gameObject.SetActive(false);
					this.GamePlayPanelAnim(true);
				});
				this.SetPlayPanels();
			});
		}
		else
		{
			this.SetPlayPanels();
			this.GamePlayPanelAnim(true);
		}
	}

	private void SetPlayPanels()
	{
		this.title.SetActive(false);
		this.levelCompletePanel.SetActive(false);
		this.comparePanel.SetActive(false);
		this.gameOverPanel.SetActive(false);
		EventManager.CallOnPlay();
	}

	public void PlayNextLevel()
	{
		if (!this.comparePanel.activeInHierarchy)
		{
			this.Compare();
		}
		else
		{
			this.PlayButton();
			Generator.instance.OnPlayNextLevel();
			LevelManager.instance.PlayNextLevel();
			ColorController.instance.SetColor();
		}
	}

	public void OnLevelComplete()
	{
		this.GamePlayPanelAnim(false);
		this.levelCompletePanel.SetActive(true);
		this.LevelCompletePanelAnim();
        StartCoroutine(ShowAds());
        DOVirtual.DelayedCall(2f, delegate
		{
			if (this.levelCompletePanel.activeInHierarchy && !this.comparePanel.activeInHierarchy)
			{
				this.Compare();
			}
		}, true);
	}

	private void Compare()
	{
		EventManager.CallOnCompare();
		CompareTower.instance.StartCompare();
		CanvasGroup canvasGroup = this.comparePanel.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		this.comparePanel.SetActive(true);
		DOTween.To(() => canvasGroup.alpha, delegate(float x)
		{
			canvasGroup.alpha = x;
		}, 1f, 0.2f);
	}

	public void OnGameOver()
	{
		this.startPanel.SetActive(true);
		this.GamePlayPanelAnim(false);
        StartCoroutine(ShowAds());
	}

    IEnumerator ShowAds()
    {
        yield return new WaitForSeconds(1.0f);
        AdsControl.Instance.showAds();
    }

    public void OnSoundButton()
	{
		this.sound = !this.sound;
		if (this.sound)
		{
			this.soundButtonIcon.sprite = this.soundEnableIcon;
			PlayerPrefs.SetInt("sound", 1);
			AudioListener.volume = 1f;
		}
		else
		{
			this.soundButtonIcon.sprite = this.soundDisableIcon;
			PlayerPrefs.SetInt("sound", 0);
			AudioListener.volume = 0f;
		}
		this.soundButtonIcon.SetNativeSize();
		PlayerPrefs.Save();
	}

	public void OnVibrationButton()
	{
		this.vibration = !this.vibration;
		if (this.vibration)
		{
			this.vibrationButtonIcon.sprite = this.vibrationEnableIcon;
			PlayerPrefs.SetInt("vibration", 1);
			//HapticManager.Impact(ImpactFeedback.Light);
		}
		else
		{
			this.vibrationButtonIcon.sprite = this.vibrationDisableIcon;
			PlayerPrefs.SetInt("vibration", 0);
		}
		this.vibrationButtonIcon.SetNativeSize();
		PlayerPrefs.Save();
	}

	private void Load()
	{
		if (PlayerPrefs.HasKey("sound") && PlayerPrefs.GetInt("sound") == 0)
		{
			this.sound = true;
		}
		else
		{
			this.sound = false;
		}
		this.OnSoundButton();
		if (PlayerPrefs.HasKey("vibration") && PlayerPrefs.GetInt("vibration") == 0)
		{
			this.vibration = true;
		}
		else
		{
			this.vibration = false;
		}
		this.OnVibrationButton();
	}

	private void SocialButtonAnimPlay()
	{
		this.socialIconAnimPlay = DOTween.Sequence();
		this.socialIconAnimPlay.Append(this.socialButtonIconImage.transform.DORotate(new Vector3(0f, 0f, 360f), 0.2f, RotateMode.FastBeyond360));
		this.socialIconAnimPlay.Join(this.socialButtonIconImage.transform.DOScale(0f, 0.2f));
		this.socialIconAnimPlay.AppendCallback(delegate
		{
			this.spriteID++;
			if (this.spriteID == this.socialButtonIcons.Count)
			{
				this.spriteID = 0;
			}
			this.socialButtonIconImage.sprite = this.socialButtonIcons[this.spriteID];
			this.socialButtonIconImage.SetNativeSize();
		});
		this.socialIconAnimPlay.Append(this.socialButtonIconImage.transform.DORotate(new Vector3(0f, 0f, 360f), 0.2f, RotateMode.FastBeyond360));
		this.socialIconAnimPlay.Join(this.socialButtonIconImage.transform.DOScale(0.25f, 0.2f));
		this.socialIconAnimPlay.AppendInterval(2f);
		this.socialIconAnimPlay.OnComplete(new TweenCallback(this.SocialButtonAnimPlay));
	}

	private void SocialButtonAnimStop()
	{
		this.socialIconAnimPlay.Kill(false);
		this.socialIconAnimStop = DOTween.Sequence();
		this.socialIconAnimStop.Append(this.socialButtonIconImage.transform.DORotate(new Vector3(0f, 0f, 360f), 0.2f, RotateMode.FastBeyond360));
		this.socialIconAnimStop.Join(this.socialButtonIconImage.transform.DOScale(0f, 0.2f));
		this.socialIconAnimStop.Append(this.socialButtonCloseText.transform.DORotate(new Vector3(0f, 0f, 360f), 0.2f, RotateMode.FastBeyond360));
		this.socialIconAnimStop.Join(this.socialButtonCloseText.transform.DOScale(1f, 0.2f));
	}

	public void OnSocialButton()
	{
		this.social = !this.social;
		if (this.social)
		{
			this.SocialButtonAnimStop();
			this.socialAnim.Kill(false);
			this.socialAnim = DOTween.Sequence();
			this.socialAnim.Insert(0f, this.socialButtons[0].DOAnchorPos(new Vector2(-50f, 0f), 0.3f, false));
			this.socialAnim.Insert(0f, this.socialButtons[0].DOScale(0.8f, 0.2f));
			this.socialAnim.Insert(0.1f, this.socialButtons[1].DOAnchorPos(new Vector2(-40f, 40f), 0.3f, false));
			this.socialAnim.Insert(0.1f, this.socialButtons[1].DOScale(0.8f, 0.2f));
			this.socialAnim.Insert(0.2f, this.socialButtons[2].DOAnchorPos(new Vector2(0f, 50f), 0.3f, false));
			this.socialAnim.Insert(0.2f, this.socialButtons[2].DOScale(0.8f, 0.2f));
		}
		else
		{
			this.socialIconAnimStop.Kill(false);
			this.socialAnim.Kill(false);
			this.socialAnim = DOTween.Sequence();
			this.socialAnim.Insert(0f, this.socialButtons[2].DOAnchorPos(Vector2.zero, 0.3f, false));
			this.socialAnim.Insert(0.1f, this.socialButtons[2].DOScale(0f, 0.2f));
			this.socialAnim.Insert(0.1f, this.socialButtons[1].DOAnchorPos(Vector2.zero, 0.3f, false));
			this.socialAnim.Insert(0.2f, this.socialButtons[1].DOScale(0f, 0.2f));
			this.socialAnim.Insert(0.2f, this.socialButtons[0].DOAnchorPos(Vector2.zero, 0.3f, false));
			this.socialAnim.Insert(0.3f, this.socialButtons[0].DOScale(0f, 0.2f));
			this.socialAnim.Insert(0.1f, this.socialButtonCloseText.transform.DORotate(new Vector3(0f, 0f, 360f), 0.2f, RotateMode.FastBeyond360));
			this.socialAnim.Insert(0.2f, this.socialButtonCloseText.transform.DOScale(0f, 0.2f)).OnComplete(new TweenCallback(this.SocialButtonAnimPlay));
		}
	}

	public void OnFacebookButton()
	{
		base.StartCoroutine(this.OpenFacebookPage());
	}

	public void OnInstagramButton()
	{
		base.StartCoroutine(this.OpenInstagramPage());
	}

	public void OnTwetterButton()
	{
		base.StartCoroutine(this.OpenTwitterPage());
	}

	private IEnumerator OpenFacebookPage()
	{
		MenuManager._OpenFacebookPage_c__Iterator0 _OpenFacebookPage_c__Iterator = new MenuManager._OpenFacebookPage_c__Iterator0();
		_OpenFacebookPage_c__Iterator._this = this;
		return _OpenFacebookPage_c__Iterator;
	}

	private IEnumerator OpenInstagramPage()
	{
		MenuManager._OpenInstagramPage_c__Iterator1 _OpenInstagramPage_c__Iterator = new MenuManager._OpenInstagramPage_c__Iterator1();
		_OpenInstagramPage_c__Iterator._this = this;
		return _OpenInstagramPage_c__Iterator;
	}

	private IEnumerator OpenTwitterPage()
	{
		MenuManager._OpenTwitterPage_c__Iterator2 _OpenTwitterPage_c__Iterator = new MenuManager._OpenTwitterPage_c__Iterator2();
		_OpenTwitterPage_c__Iterator._this = this;
		return _OpenTwitterPage_c__Iterator;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		this.leftApp = true;
	}

	private void CallRateUs()
	{
		if (LevelManager.instance.currentLevel == 5 || LevelManager.instance.currentLevel == 20)
		{
			this.OnRateUs();
		}
	}

	private void OnRateUs()
	{
		Rateus.instance.OnRateusButton();
	}

	private void GamePlayPanelAnim(bool gamePlayOpened)
	{
		this.levelBar.DOKill(false);
		this.score.DOKill(false);
		this.gamePlayAnimSeq.Kill(false);
		if (!gamePlayOpened)
		{
			this.gamePlayPanel.SetActive(false);
			this.gamePlayAnimSeq = DOTween.Sequence();
			this.gamePlayAnimSeq.Append(this.levelBar.DOAnchorPosY(-30f, 0.2f, false));
			this.gamePlayAnimSeq.Append(this.levelBar.DOAnchorPosY(30f, 0.2f, false));
			this.gamePlayAnimSeq.Insert(0.1f, this.score.DOAnchorPosY(-10f, 0.2f, false));
			this.gamePlayAnimSeq.Append(this.score.DOAnchorPosY(80f, 0.2f, false));
			this.gamePlayAnimSeq.OnComplete(delegate
			{
			});
		}
		else
		{
			this.gamePlayPanel.SetActive(true);
			this.gamePlayAnimSeq = DOTween.Sequence();
			this.gamePlayAnimSeq.Append(this.levelBar.DOAnchorPosY(-30f, 0.2f, false));
			this.gamePlayAnimSeq.Append(this.levelBar.DOAnchorPosY(-20f, 0.2f, false));
			this.gamePlayAnimSeq.Insert(0.1f, this.score.DOAnchorPosY(-10f, 0.2f, false));
			this.gamePlayAnimSeq.Append(this.score.DOAnchorPosY(0f, 0.2f, false));
		}
	}

	private void LevelCompletePanelAnim()
	{
		this.levelCompletePanelLevelText.anchoredPosition = new Vector2(this.levelCompletePanelLevelText.anchoredPosition.x, 100f);
		Sequence s = DOTween.Sequence();
		s.AppendInterval(0.2f);
		s.Append(this.levelCompletePanelLevelText.DOAnchorPosY(-45f, 0.2f, false));
		s.Append(this.levelCompletePanelLevelText.DOAnchorPosY(-35f, 0.2f, false));
	}
}
