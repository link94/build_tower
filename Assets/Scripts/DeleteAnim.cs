using System;
using UnityEngine;

public class DeleteAnim : MonoBehaviour
{
	public void Delete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
