using System;
using System.Collections.Generic;
using UnityEngine;

public class Splasher : MonoBehaviour
{
	public List<Transform> splashers;

	public float minTime = 5f;

	public float maxTime = 10f;

	private List<Transform> activeSplashes = new List<Transform>();

	private float time;

	private float nextTime;

	private int nextSplash;

	private void Start()
	{
		this.nextValues();
	}

	private void nextValues()
	{
		this.nextTime = UnityEngine.Random.Range(this.minTime, this.maxTime);
		this.nextSplash = (int)Mathf.Floor(UnityEngine.Random.Range(0f, (float)this.splashers.Count));
	}

	private void Update()
	{
		this.time += Time.deltaTime;
		if (this.time > this.nextTime)
		{
			this.createSplash();
			this.nextValues();
		}
		if (Input.GetMouseButton(0))
		{
			this.createSplash();
		}
		for (int i = 0; i < this.activeSplashes.Count; i++)
		{
			ParticleSystem component = this.activeSplashes[i].GetComponent<ParticleSystem>();
			if (!component.IsAlive())
			{
				UnityEngine.Object.Destroy(this.activeSplashes[i].gameObject);
				this.activeSplashes.Remove(this.activeSplashes[i]);
				break;
			}
		}
	}

	private void createSplash()
	{
		this.time = 0f;
		if (this.activeSplashes.Count > 20 || this.splashers.Count == 0)
		{
			return;
		}
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.splashers[this.nextSplash], this.splashers[this.nextSplash].position, Quaternion.identity);
		ParticleSystem component = transform.GetComponent<ParticleSystem>();
		component.Play();
		this.activeSplashes.Add(transform);
	}
}
