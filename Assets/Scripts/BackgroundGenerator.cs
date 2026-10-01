using System;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
	public static BackgroundGenerator instance;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	public Material tubeMat;

	[SerializeField]
	private Vector2 positionX;

	[SerializeField]
	private Vector2 positionY;

	[SerializeField]
	private Vector2 scaleFactor;

	private float createPositionY;

	private bool first = true;

	private GameObject parent;

	private void Awake()
	{
		BackgroundGenerator.instance = this;
		EventManager.OnPlay += new EventManager.Play(this.OnPlay);
	}

	private void Start()
	{
	}

	private void Update()
	{
		this.tubeMat.SetFloat("_FogYStartPos", Camera.main.transform.position.y - 5f);
	}

	private void CreateFirstTubes()
	{
		this.parent = new GameObject("BackgroundTubesParent");
		this.parent.transform.position = Vector3.zero;
		this.parent.transform.localScale = Vector3.one;
		this.first = true;
		this.createPositionY = 0f;
		for (int i = 0; i < 10; i++)
		{
			this.CreateTube();
			this.createPositionY += 5f;
		}
		this.first = false;
	}

	public void CreateTube()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
		gameObject.transform.SetParent(this.parent.transform);
		int num;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			num = 1;
		}
		else
		{
			num = -1;
		}
		gameObject.transform.position = new Vector3(UnityEngine.Random.Range(this.positionX.x, this.positionX.y) * (float)num, this.createPositionY + 10f, gameObject.transform.position.z);
		float num2 = UnityEngine.Random.Range(this.scaleFactor.x, this.scaleFactor.y);
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * num2, gameObject.transform.localScale.y * num2 * 5f, gameObject.transform.localScale.z * num2);
	}

	private void DeleteAll()
	{
		UnityEngine.Object.Destroy(this.parent);
	}

	public void OnPlay()
	{
		this.DeleteAll();
		this.CreateFirstTubes();
	}
}
