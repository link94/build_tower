//using ArmNomads.Haptic;
using DG.Tweening;
using System;
using UnityEngine;

public class Tube : MonoBehaviour
{
	[Header("Parts"), SerializeField]
	public Transform clickCircle;

	[SerializeField]
	private Transform mainTube;

	[SerializeField]
	public MeshRenderer mainTubeRend;

	[SerializeField]
	private Transform truePart;

	[SerializeField]
	private MeshRenderer truePartRend;

	[SerializeField]
	private Transform perfectPart;

	[SerializeField]
	public GameObject topPart;

	[SerializeField]
	public GameObject topPartObj;

	[SerializeField]
	public Transform foots;

	[SerializeField]
	private GameObject firstBlur;

	[Header("")]
	public float startPositionY;

	private float topPosition;

	public float targetPositionY;

	public bool first;

	private bool canClick;

	private Sequence move;

	private int lastFoot;

	public bool lastReversedTube;

	private bool gameOver;

	private void Start()
	{
		this.Init();
	}

	private void Update()
	{
		if (this.canClick)
		{
			this.CheckPerfectLine();
		}
	}

	public void Init()
	{
		this.canClick = false;
		this.gameOver = false;
		if (Generator.instance.lastTube != null)
		{
			this.startPositionY = Generator.instance.lastTube.GetComponent<Tube>().targetPositionY;
		}
		else
		{
			this.startPositionY = 0f;
		}
		this.topPosition = this.startPositionY + this.mainTube.lossyScale.y * 2f;
		if (this.first)
		{
			this.truePart.gameObject.SetActive(false);
			this.perfectPart.gameObject.SetActive(false);
			this.firstBlur.SetActive(true);
		}
		else
		{
			int num;
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				num = 1;
			}
			else
			{
				num = -1;
			}
			float num2 = LevelManager.instance.currentLevelValues.truePartYScaleNominal + LevelManager.instance.currentLevelValues.truePartYScaleNominal * (float)num * UnityEngine.Random.Range(0f, LevelManager.instance.currentLevelValues.truePartYScaleFactor) / 100f;
			this.truePart.localScale = new Vector3(this.truePart.localScale.x, num2, this.truePart.localScale.z);
			float num3 = UnityEngine.Random.Range(this.mainTube.position.y + this.mainTube.localScale.y / 2.5f, this.mainTube.position.y + this.mainTube.localScale.y * 2f - this.truePart.localScale.y * 2f);
			this.truePart.position = new Vector3(this.truePart.position.x, num3, this.truePart.position.z);
			this.perfectPart.localScale = new Vector3(this.perfectPart.localScale.x, LevelManager.instance.currentLevelValues.perfectPartScaleY, this.perfectPart.localScale.z);
			float y = UnityEngine.Random.Range(num3, num3 + num2 * 2f - this.perfectPart.localScale.y * 2f);
			this.perfectPart.position = new Vector3(this.perfectPart.position.x, y, this.perfectPart.position.z);
			base.transform.position = new Vector3(base.transform.position.x, this.startPositionY, base.transform.position.z);
			if (Generator.instance.perfect)
			{
				this.truePart.gameObject.SetActive(false);
				this.perfectPart.gameObject.SetActive(false);
			}
			Generator.instance.lastTube.GetComponent<Tube>().topPart.SetActive(true);
			this.topPart.SetActive(false);
			this.Move();
		}
	}

	private void Move()
	{
		float num = UnityEngine.Random.Range(LevelManager.instance.currentLevelValues.tubeMoveDownSpeedFactor.x, LevelManager.instance.currentLevelValues.tubeMoveDownSpeedFactor.y) / 100f;
		float duration = Generator.instance.tubeMoveDownSpeed - Generator.instance.tubeMoveDownSpeed * num;
		if (Generator.instance.perfect)
		{
			Generator.instance.EndPerfectMode();
			this.targetPositionY = this.startPositionY + this.mainTube.localScale.y * 2f;
			this.move = DOTween.Sequence();
			this.move.Append(base.transform.DOMoveY(this.topPosition, Generator.instance.tubeMoveUpSpeed, false));
			this.move.InsertCallback(Generator.instance.tubeMoveUpSpeed / 2f, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Generator.instance.auraPrefab);
				gameObject.transform.position = new Vector3(base.transform.position.x, this.startPositionY, base.transform.position.z);
				gameObject.GetComponent<AuraAnimControl>().PlayAnim(Generator.instance.perfectCounter);
				this.Foots();
				this.TubeEnd();
				SFXManager.instance.PlayPerfectSfx();
			});
		}
		else
		{
			this.move = DOTween.Sequence();
			this.move.Append(base.transform.DOMoveY(this.topPosition, Generator.instance.tubeMoveUpSpeed, false));
			this.move.AppendCallback(delegate
			{
				this.canClick = true;
				Generator.instance.lastTube.GetComponent<Tube>().clickCircle.DOScale(1.6f, 0.2f);
			});
			this.move.Append(base.transform.DOMoveY(this.startPositionY, duration, false));
			this.move.OnComplete(delegate
			{
				if (!Generator.instance.gameOver)
				{
					this.OnGameOver();
				}
			});
		}
	}

	private void CheckPerfectLine()
	{
		if (Generator.instance.gameOver)
		{
			return;
		}
		if (this.startPositionY - this.perfectPart.position.y > 0f && this.startPositionY - this.perfectPart.position.y <= this.perfectPart.lossyScale.y * 2f)
		{
			if (Generator.instance.lastTube.GetComponent<Tube>().topPartObj.GetComponent<MeshRenderer>().material != Generator.instance.perfectPartMaterial)
			{
				Generator.instance.lastTube.GetComponent<Tube>().topPartObj.GetComponent<MeshRenderer>().material = Generator.instance.perfectPartMaterial;
				Generator.instance.lastTube.GetComponent<Tube>().clickCircle.GetChild(0).GetComponent<SpriteRenderer>().color = Generator.instance.perfectPartMaterial.color;
			}
		}
		else if (Generator.instance.lastTube.GetComponent<Tube>().topPartObj.GetComponent<MeshRenderer>().material != Generator.instance.topPartMaterial)
		{
			Generator.instance.lastTube.GetComponent<Tube>().topPartObj.GetComponent<MeshRenderer>().material = Generator.instance.topPartMaterial;
			Generator.instance.lastTube.GetComponent<Tube>().clickCircle.GetChild(0).GetComponent<SpriteRenderer>().color = Generator.instance.topPartMaterial.color;
			Generator.instance.lastTube.GetComponent<Tube>().topPart.transform.localScale = new Vector3(1f, 0.001f, 1f);
		}
	}

	public void CheckPosition()
	{
		if (!this.canClick || this.first)
		{
			return;
		}
	//MonoBehaviour.print("CHECK POSITION");
		Generator.instance.lastTube.GetComponent<Tube>().clickCircle.DOScale(0f, 0.2f);
		float num = this.startPositionY - this.truePart.position.y;
		if (num > 0f && num <= this.truePart.lossyScale.y * 2f)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Generator.instance.auraPrefab);
			gameObject.transform.position = new Vector3(base.transform.position.x, this.startPositionY, base.transform.position.z);
			if (this.startPositionY - this.perfectPart.position.y > 0f && this.startPositionY - this.perfectPart.position.y <= this.perfectPart.lossyScale.y * 2f)
			{
				if (!Generator.instance.perfect)
				{
					//MonoBehaviour.print("perfect");
					if (MenuManager.instance.vibration)
					{
						//HapticManager.Impact(ImpactFeedback.Medium);
					}
					Generator.instance.StartPerfectMode();
					SFXManager.instance.PlayPerfectSfx();
					gameObject.GetComponent<AuraAnimControl>().PlayAnim(Generator.instance.perfectCounter);
				}
			}
			else
			{
				Generator.instance.perfectContineus = false;
				Generator.instance.coinFromTube = 1;
				if (MenuManager.instance.vibration)
				{
					//HapticManager.Impact(ImpactFeedback.Light);
				}
				SFXManager.instance.PLaySfx();
				gameObject.GetComponent<AuraAnimControl>().PlayAnim(0);
			}
			GameManager.instance.GetScore(Generator.instance.coinFromTube);
			this.Foots();
			//MonoBehaviour.print("GOOD");
			this.move.Kill(false);
			this.targetPositionY = base.transform.position.y;
			this.TubeEnd();
		}
		else
		{
			if (MenuManager.instance.vibration)
			{
				//HapticManager.Impact(ImpactFeedback.Heavy);
			}
			SFXManager.instance.PlayGameOverSfx();
			this.OnGameOver();
			//MonoBehaviour.print("LOSE");
		}
	}

	public void OnGameOver()
	{
		if (this.gameOver)
		{
			return;
		}
		this.gameOver = true;
		//MonoBehaviour.print("GAME OVER");
		Color truePartDefaultColor = Generator.instance.truePartDefaultColor;
		this.move.timeScale = 0f;
		this.truePartRend.sharedMaterial = Generator.instance.topPartMaterial;
		Sequence s = DOTween.Sequence();
		s.Append(Generator.instance.topPartMaterial.DOColor(Color.red, 0.2f));
		s.Append(Generator.instance.topPartMaterial.DOColor(truePartDefaultColor, 0.2f));
		s.Append(Generator.instance.topPartMaterial.DOColor(Color.red, 0.2f));
		s.Append(Generator.instance.topPartMaterial.DOColor(truePartDefaultColor, 0.2f));
		s.AppendCallback(delegate
		{
			this.move.timeScale = 10f;
			this.truePart.gameObject.SetActive(false);
			this.perfectPart.gameObject.SetActive(false);
			if (!Generator.instance.CheckSecondChance())
			{
				EventManager.CallOnGameOver();
			}
		});
	}

	public void GiveSecondChance()
	{
		this.truePart.gameObject.SetActive(true);
		this.truePart.GetComponentInChildren<MeshRenderer>().sharedMaterial = Generator.instance.truePartMaterial;
		this.perfectPart.gameObject.SetActive(true);
		this.move.timeScale = 1f;
		this.Init();
	}

	private void TubeEnd()
	{
		this.truePart.gameObject.SetActive(false);
		this.perfectPart.gameObject.SetActive(false);
		Generator.instance.lastTube.SetParent(base.transform);
		Generator.instance.lastTube = base.transform;
		Generator.instance.TubeScale();
	}

	private void Foots()
	{
		this.foots.gameObject.SetActive(true);
		if (Generator.instance.tubeType == Generator.TubeType.Cylinder)
		{
		}
		this.foots.position = new Vector3(this.foots.position.x, this.startPositionY, this.foots.position.z);
		for (int i = 0; i < this.foots.childCount; i++)
		{
			Transform child = this.foots.GetChild(i);
			child.DOMoveX(child.localPosition.x + child.forward.x * child.localScale.y / 2f, Generator.instance.tubeFootsGoOutTime, false);
			child.DOMoveZ(child.localPosition.z + child.forward.z * child.localScale.y / 2f, Generator.instance.tubeFootsGoOutTime, false);
			child.DORotate(new Vector3(child.eulerAngles.x, child.eulerAngles.y, child.eulerAngles.z + Generator.instance.footRotateCoefficient), Generator.instance.tubeFootsRotationTime, RotateMode.FastBeyond360);
		}
		DOVirtual.DelayedCall(0.1f, new TweenCallback(this.FootAfterShock), true);
	}

	public void FootsReverseAction()
	{
		for (int i = 0; i < this.foots.childCount; i++)
		{
			Transform child = this.foots.GetChild(i);
			child.GetComponentInChildren<ParticleSystem>().Play();
		}
	}

	public void FootsReverseMove()
	{
		this.lastFoot = 0;
		for (int i = 0; i < this.foots.childCount; i++)
		{
			if (i == this.foots.childCount - 1)
			{
				this.lastFoot = i;
			}
			Transform child = this.foots.GetChild(i);
			child.GetComponentInChildren<ParticleSystem>().Play();
			Sequence sequence = DOTween.Sequence();
			sequence.AppendInterval(0.7f);
			sequence.Append(child.DOMoveX(0f, Generator.instance.tubeFootsGoOutTime * 3f, false));
			sequence.Join(child.DOMoveZ(0f, Generator.instance.tubeFootsGoOutTime * 3f, false));
			sequence.Join(child.DORotate(new Vector3(child.eulerAngles.x, child.eulerAngles.y, child.eulerAngles.z + Generator.instance.footRotateCoefficient), Generator.instance.tubeFootsRotationTime, RotateMode.FastBeyond360));
			sequence.Join(this.clickCircle.DOScale(0f, 0.1f));
			sequence.OnComplete(delegate
			{
				if (this.lastFoot == this.foots.childCount - 1 && this.lastReversedTube)
				{
					this.lastFoot = 0;
					Generator.instance.ReverseAction();
				}
			});
		}
	}

	private void FootAfterShock()
	{
		for (int i = 0; i < this.foots.childCount; i++)
		{
			this.foots.GetChild(i).DOLocalMove(this.foots.GetChild(i).localPosition - this.foots.GetChild(i).forward * this.foots.GetChild(i).localScale.y / 10f, Generator.instance.tubeFootsGoOutTime, false);
		}
	}
}
