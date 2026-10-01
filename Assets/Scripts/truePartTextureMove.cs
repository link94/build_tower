using DG.Tweening;
using System;
using UnityEngine;

public class truePartTextureMove : MonoBehaviour
{
	[SerializeField]
	private Material truePartMaterial;

	[SerializeField]
	private float truePartMaterialTilingMaxX;

	private void Start()
	{
		EventManager.OnPlay += new EventManager.Play(this.OnPlay);
		this.truePartMaterial.DOOffset(new Vector2(1f, -1f), 2f).SetEase(Ease.Linear).SetLoops(-1);
	}

	private void OnPlay()
	{
		this.truePartMaterial.mainTextureScale = new Vector2(this.truePartMaterial.mainTextureScale.x, LevelManager.instance.currentLevelValues.truePartYScaleNominal * this.truePartMaterialTilingMaxX / LevelManager.instance.levels[0].truePartYScaleNominal);
	}
}
