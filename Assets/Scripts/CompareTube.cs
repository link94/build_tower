using System;
using UnityEngine;

public class CompareTube : MonoBehaviour
{
	[Header("Parts"), SerializeField]
	public MeshRenderer mainTubeRend;

	[SerializeField]
	private MeshRenderer truePartRend;

	[SerializeField]
	public Transform foots;

	[SerializeField]
	private GameObject firstBlur;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
