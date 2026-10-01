using System;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
	private void Start()
	{
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
	}
}
