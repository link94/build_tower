using System;
using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField]
	public int generatableTubesCount;

	[SerializeField]
	public float truePartYScaleNominal;

	[SerializeField]
	public float truePartYScaleFactor;

	[SerializeField]
	public float perfectPartScaleY;

	[SerializeField]
	public Vector2 tubeMoveDownSpeedFactor;

	[Header("Compare"), SerializeField]
	public Sprite compareSprite;

	[SerializeField]
	public string compareSpriteName;

	[SerializeField]
	public float compareMeter;

	[SerializeField]
	public string compareUnit;

	[SerializeField]
	public Vector2 blurAnchorPosition;

	[SerializeField]
	public Vector2 blurSizeDelta;

	[SerializeField]
	public Color blurColor;
}
