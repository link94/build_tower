using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
	public enum TubeType
	{
		Cube,
		Cylinder,
		Triangle,
		Star,
		Starline
	}

	private sealed class _ReverseAction_c__AnonStorey0
	{
		internal Transform tubeTransform;

		internal int i;

		internal float speed;

		internal Generator _this;

		internal void __m__0()
		{
			UnityEngine.Object.Destroy(this.tubeTransform.gameObject);
		}

		internal void __m__1()
		{
			this._this.allTubes.Remove(this._this.allTubes[this.i]);
			this.i--;
			if (this.i > 0)
			{
				this._this.ReverseAction();
				Camera.main.DOKill(false);
				Camera.main.transform.DOMoveY(this._this.allTubes[this.i].position.y + 2f, this.speed, false).SetEase(Ease.Linear);
			}
		}
	}

	public static Generator instance;

	public Generator.TubeType tubeType;

	[SerializeField]
	private int tubePrefabID;

	[Header("Prefabs"), SerializeField]
	private List<GameObject> tubePrefabs;

	[SerializeField]
	public GameObject tubePrefab;

	[SerializeField]
	public GameObject auraPrefab;

	[SerializeField]
	public GameObject perfectTextPrefab;

	[SerializeField]
	public GameObject levelCompleteParticlePrefab;

	[Header("Scales"), SerializeField]
	public float tubeScaleCoefficient;

	[Header("Speeds"), SerializeField]
	public float tubeMoveUpSpeed;

	[Header("Tubes Start Values"), SerializeField]
	public float truePartScaleY;

	[SerializeField]
	public float tubeMoveDownSpeed;

	[Header("Tubes Materials and colors"), SerializeField]
	public Material mainTubeMaterial;

	[SerializeField]
	public Color mainTubeDefaultColor;

	[SerializeField]
	public Material truePartMaterial;

	[SerializeField]
	public Color truePartDefaultColor;

	[SerializeField]
	public Material topPartMaterial;

	[SerializeField]
	public Color topPartDefaultColor;

	[SerializeField]
	public Material perfectPartMaterial;

	[SerializeField]
	public Color perfectPartDefaultColor;

	[Header(" "), SerializeField]
	public Gradient gradient;

	[Header("Second Chance"), SerializeField]
	private GameObject secondChancePanel;

	[SerializeField]
	private Transform secondChanceAnim;

	[SerializeField]
	private Image secondChanceTimerFill;

	private Sequence secondChance;

	[Header(" ")]
	public float generationCount;

	public List<Transform> allTubes;

	public Transform lastTube;

	public Tube currentTube;

	private int perfectCount;

	public bool perfectContineus;

	public int perfectCounter;

	public bool perfect;

	public bool gameOver;

	private float reverseActionSpeedCoefficient;

	public int coinFromTube = 1;

	[Header("Tube Foots"), SerializeField]
	public float tubeFootsGoOutTime;

	[SerializeField]
	public float tubeFootsRotationTime;

	public float footRotateCoefficient;

	[Header(" "), Range(0.1f, 1f), SerializeField]
	private float timeScale;

    public List<Color> colorListTube;

    int _random;

    private void Awake()
	{
		Generator.instance = this;
		this.Load();
		this.SelectTubeType();
		EventManager.OnPlay += new EventManager.Play(this.OnPlay);
		EventManager.OnGameOver += new EventManager.GameOver(this.OnGameOver);
	}

	private void Start()
	{
		this.allTubes = new List<Transform>();
		this.truePartMaterial.mainTextureOffset = Vector2.zero;
		this.GenerateFirstTube();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.S))
		{
			if (this.timeScale == 1f)
			{
				this.timeScale = 0.1f;
			}
			else
			{
				this.timeScale = 1f;
			}
		}
		if (UnityEngine.Input.touchCount > 0)
		{
		}
		if (Input.GetMouseButtonDown(0) && this.currentTube != null && !this.gameOver && !LevelManager.instance.levelComplete)
		{
			this.currentTube.CheckPosition();
		}
	}

	private void SetDefaultColors()
	{
	}

	public void GenerateFirstTube()
	{
		this.generationCount = 1f;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tubePrefab);
		this.allTubes.Add(gameObject.transform);
		gameObject.transform.position = Vector3.zero;
		gameObject.GetComponent<Tube>().first = true;
		this.lastTube = gameObject.transform;
		this.currentTube = gameObject.GetComponent<Tube>();
		this.MainTubeColor();
        _random = UnityEngine.Random.Range(0, 10);
    }

	private void StartPlay()
	{
		Camera.main.transform.DOKill(false);
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, this.lastTube.position.y + 4f, Camera.main.transform.position.z);
		Sequence s = DOTween.Sequence();
		s.Append(this.lastTube.transform.DOScale(this.lastTube.transform.localScale.y * this.tubeScaleCoefficient, this.tubeMoveUpSpeed));
		s.InsertCallback(0f, new TweenCallback(this.GenerateTube));
	}

	public void GenerateTube()
	{
		if (!LevelManager.instance.LevelPass())
		{
			this.generationCount += 1f;
			this.DisableTopParts();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tubePrefab);
			this.currentTube = gameObject.GetComponent<Tube>();
			this.MainTubeColor();
			this.allTubes.Add(gameObject.transform);
		}
		else
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.levelCompleteParticlePrefab);
			gameObject2.transform.position = new Vector3(this.currentTube.topPart.transform.position.x, this.currentTube.topPart.transform.position.y - 0.2f, this.currentTube.topPart.transform.position.z);
			gameObject2.transform.SetParent(this.currentTube.transform);
		}
	}

	public void TubeScale()
	{
		Sequence s = DOTween.Sequence();
		s.Append(this.lastTube.transform.DOScale(this.lastTube.transform.localScale.y * this.tubeScaleCoefficient, this.tubeMoveUpSpeed));
		s.InsertCallback(this.tubeMoveUpSpeed / 10f, new TweenCallback(this.GenerateTube));
		Camera.main.transform.DOMoveY(this.lastTube.position.y + 4f, 2f, false);
	}

	private void DisableTopParts()
	{
		for (int i = 0; i < this.allTubes.Count - 1; i++)
		{
			this.allTubes[i].GetComponent<Tube>().topPart.SetActive(false);
		}
	}

	public void StartPerfectMode()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.perfectTextPrefab);
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, this.currentTube.transform.position.y, gameObject.transform.position.z);
		this.coinFromTube++;
		if (this.coinFromTube > 10)
		{
			this.coinFromTube = 10;
		}
		if (this.perfectContineus)
		{
			this.perfectCount++;
			if (this.perfectCount > 2)
			{
				this.perfectCount = 2;
			}
		}
		else
		{
			this.perfectCount = 1;
		}
		this.perfect = true;
		this.perfectCounter = this.perfectCount;
		this.tubeMoveUpSpeed /= 2f;
	}

	public void EndPerfectMode()
	{
		this.perfectCounter--;
		if (this.perfectCounter == 0)
		{
			this.perfect = false;
			this.perfectContineus = true;
			this.tubeMoveUpSpeed *= 2f;
		}
	}

	private void SelectTubeType()
	{
		this.tubePrefab = this.tubePrefabs[this.tubePrefabID];
		if (this.tubePrefab.CompareTag("cube"))
		{
			this.footRotateCoefficient = 180f;
			this.tubeType = Generator.TubeType.Cube;
		}
		else if (this.tubePrefab.CompareTag("cylinder"))
		{
			this.footRotateCoefficient = 360f;
			this.tubeType = Generator.TubeType.Cylinder;
		}
		else if (this.tubePrefab.CompareTag("star"))
		{
			this.footRotateCoefficient = 180f;
			this.tubeType = Generator.TubeType.Star;
		}
		else if (this.tubePrefab.CompareTag("triangle"))
		{
			this.footRotateCoefficient = 120f;
			this.tubeType = Generator.TubeType.Triangle;
		}
		else if (this.tubePrefab.CompareTag("starline"))
		{
			this.footRotateCoefficient = 144f;
			this.tubeType = Generator.TubeType.Starline;
		}
	}

	private void DeleteAll()
	{
		for (int i = 0; i < this.allTubes.Count; i++)
		{
			UnityEngine.Object.Destroy(this.allTubes[i].gameObject);
		}
		this.allTubes.Clear();
	}

	private void DeleteOldTubes()
	{
		while (this.allTubes.Count >= 15)
		{
			GameObject gameObject = this.allTubes[0].gameObject;
			this.allTubes.Remove(this.allTubes[0]);
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private void MainTubeColor()
	{
        /*
		float time = this.generationCount / (float)LevelManager.instance.currentLevelValues.generatableTubesCount;
		Color color = this.gradient.Evaluate(time);
		this.currentTube.mainTubeRend.material.color = color;
		for (int i = 0; i < this.currentTube.foots.childCount; i++)
		{
			this.currentTube.foots.GetChild(i).GetComponent<MeshRenderer>().material.color = color;
		}
		*/
        this.currentTube.mainTubeRend.material.color = colorListTube[_random];
        for (int i = 0; i < this.currentTube.foots.childCount; i++)
        {
            this.currentTube.foots.GetChild(i).GetComponent<MeshRenderer>().material.color = colorListTube[_random];
        }

    }

	public void OnPlay()
	{
		this.SetDefaultColors();
		this.perfectContineus = false;
		if (this.perfect)
		{
			this.perfect = false;
			this.tubeMoveUpSpeed *= 2f;
		}
		this.coinFromTube = 1;
		if (this.gameOver || LevelManager.instance.levelComplete)
		{
			this.DeleteAll();
			this.gameOver = false;
			LevelManager.instance.levelComplete = false;
			this.GenerateFirstTube();
		}
		this.StartPlay();
	}

	public void OnPlayNextLevel()
	{
		if (LevelManager.instance.currentLevel % 3 == 0)
		{
			this.tubePrefabID++;
			if (this.tubePrefabID == this.tubePrefabs.Count)
			{
				this.tubePrefabID = 0;
			}
			PlayerPrefs.SetInt("tubePrefabID", this.tubePrefabID);
			PlayerPrefs.Save();
			this.SelectTubeType();
		}
	}

	public void OnGameOver()
	{
		this.gameOver = true;
		this.reverseActionSpeedCoefficient = 1f;
		this.ReverseTubeFoots();
	}

	private void ReverseTubeFoots()
	{
		for (int i = this.allTubes.Count - 1; i >= 0; i--)
		{
			if (i == 0)
			{
				this.allTubes[i].GetComponent<Tube>().lastReversedTube = true;
			}
			this.allTubes[i].GetComponent<Tube>().FootsReverseMove();
		}
	}

	public void ReverseAction()
	{
		if (this.reverseActionSpeedCoefficient > 0.3f)
		{
			this.reverseActionSpeedCoefficient -= 0.2f;
		}
		else
		{
			this.reverseActionSpeedCoefficient = 0.1f;
		}
		int i = this.allTubes.Count - 1;
		float speed = this.tubeMoveUpSpeed * this.reverseActionSpeedCoefficient;
		Tube component = this.allTubes[i].GetComponent<Tube>();
		Transform tubeTransform = this.allTubes[i];
		this.allTubes[i - 1].parent = null;
		Sequence s = DOTween.Sequence();
		s.Append(tubeTransform.DOMoveY(this.allTubes[i - 1].transform.position.y, speed, false).SetEase(Ease.Linear));
		s.Join(this.allTubes[i - 1].DOScale(1f, speed));
		s.AppendCallback(delegate
		{
			UnityEngine.Object.Destroy(tubeTransform.gameObject);
		});
		s.InsertCallback(speed, delegate
		{
			this.allTubes.Remove(this.allTubes[i]);
			i--;
			if (i > 0)
			{
				this.ReverseAction();
				Camera.main.DOKill(false);
				Camera.main.transform.DOMoveY(this.allTubes[i].position.y + 2f, speed, false).SetEase(Ease.Linear);
			}
		});
	}

	private void Load()
	{
		if (PlayerPrefs.HasKey("tubePrefabID"))
		{
			this.tubePrefabID = PlayerPrefs.GetInt("tubePrefabID");
		}
	}

	public bool CheckSecondChance()
	{
		if (LevelManager.instance.levelCompletePercent > 50)
		{
			this.SeconChancePanelAnim();
			return true;
		}
		return false;
	}

	private void SeconChancePanelAnim()
	{
		this.secondChance.Kill(false);
		this.secondChancePanel.SetActive(true);
		this.secondChanceTimerFill.fillAmount = 1f;
		this.secondChanceAnim.localScale = Vector3.one;
		this.secondChance = DOTween.Sequence();
		this.secondChance.Append(this.secondChanceAnim.DOScale(1.2f, 0.5f).SetLoops(6, LoopType.Yoyo));
		this.secondChance.Insert(0f, this.secondChanceTimerFill.DOFillAmount(0f, 3f).SetEase(Ease.Linear));
		this.secondChance.OnComplete(new TweenCallback(this.OnSkipButton));
		this.secondChance.OnKill(delegate
		{
			this.secondChancePanel.SetActive(false);
		});
	}

	public void OnShowVideoButton()
	{
		
			this.secondChance.Kill(false);
		
	}

	public void OnSkipButton()
	{
		this.secondChance.Complete(true);
		EventManager.CallOnGameOver();
	}
}
