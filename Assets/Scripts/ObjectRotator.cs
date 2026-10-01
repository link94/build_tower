using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectRotator : MonoBehaviour
{
	private sealed class _slowRotationDown_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _snap___0;

		internal float toAngle;

		internal float fromAngle;

		internal float _range___1;

		internal ObjectRotator _this;

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

		public _slowRotationDown_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.animationTime = 0f;
				this._snap___0 = this._this.transformToRotate.rotation.eulerAngles.y / 90f;
				if (this._snap___0 - Mathf.Floor(this._snap___0) <= 0.001f)
				{
					goto IL_12A;
				}
				this._range___1 = this.toAngle - this.fromAngle;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._this.animationTime += Time.deltaTime;
			this._this.setRotation(ObjectRotator.elasticEaseInOut(this._this.animationTime, 0f, this._range___1, ObjectRotator.ANIMATION_DURATION) + this.fromAngle);
			if (this._this.animationTime < ObjectRotator.ANIMATION_DURATION)
			{
				this._current = 0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.setRotation(this.toAngle);
			IL_12A:
			this._PC = -1;
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

	public Transform transformToRotate;

	public bool insideTransformToRotate;

	public float speed = 0.3f;

	private bool horizontal = true;

	private Quaternion fromRotation;

	private Quaternion toRotation;

	private float clickPosition;

	private IEnumerator slowDownCoroutine;

	private DragForce dragForce = new DragForce();

	private DragDirection dragDirection = new DragDirection();

	private float lastRotation;

	private static float MAX_DRAG_FORCE = 6f;

	private static float SNAP_DISTANCE = 5f;

	private float animationTime;

	private static float ANIMATION_DURATION = 1f;

	private void Start()
	{
		if (this.transformToRotate == null)
		{
			this.transformToRotate = base.transform;
		}
	}

	private void OnMouseDown()
	{
		if (this.slowDownCoroutine != null)
		{
			base.StopCoroutine(this.slowDownCoroutine);
		}
		this.clickPosition = this.getCurrentMousePosition();
		this.dragForce.addItem(this.clickPosition);
		this.dragDirection.addItem(this.clickPosition);
	}

	private void OnMouseUp()
	{
		this.dragForce.clear();
		float closestSnapRotation = this.getClosestSnapRotation();
		float fromAngle = (!this.horizontal) ? this.transformToRotate.eulerAngles.x : this.transformToRotate.eulerAngles.y;
		this.slowDownCoroutine = this.slowRotationDown(fromAngle, closestSnapRotation);
		base.StartCoroutine(this.slowDownCoroutine);
	}

	private void OnMouseDrag()
	{
		float currentMousePosition = this.getCurrentMousePosition();
		this.dragForce.addItem(currentMousePosition);
		this.dragDirection.addItem(currentMousePosition);
		float force = this.dragForce.getForce();
		this.rotate(this.getRotationForce(force, ObjectRotator.MAX_DRAG_FORCE, 0f));
		this.snapToRotation();
		this.clickPosition = this.getCurrentMousePosition();
	}

	private void rotate(float force)
	{
		this.lastRotation = this.transformToRotate.eulerAngles.y;
		if (this.horizontal)
		{
			if (this.transformToRotate != base.transform && !this.insideTransformToRotate)
			{
				base.transform.Rotate(Vector3.up * force * this.speed);
			}
			this.transformToRotate.Rotate(Vector3.up * force * this.speed);
		}
		else
		{
			if (this.transformToRotate != base.transform && !this.insideTransformToRotate)
			{
				base.transform.Rotate(-Vector3.forward * force * this.speed);
			}
			this.transformToRotate.Rotate(-Vector3.forward * force * this.speed);
		}
		this.clickPosition = this.getCurrentMousePosition();
	}

	private void setRotation(float rotation)
	{
		this.lastRotation = this.transformToRotate.eulerAngles.y;
		if (this.horizontal)
		{
			Vector3 eulerAngles;
			if (this.transformToRotate != base.transform && !this.insideTransformToRotate)
			{
				eulerAngles = base.transform.eulerAngles;
				eulerAngles.y = rotation;
				base.transform.eulerAngles = eulerAngles;
			}
			eulerAngles = this.transformToRotate.eulerAngles;
			eulerAngles.y = rotation;
			this.transformToRotate.eulerAngles = eulerAngles;
		}
		else
		{
			Vector3 eulerAngles;
			if (this.transformToRotate != base.transform && !this.insideTransformToRotate)
			{
				eulerAngles = base.transform.eulerAngles;
				eulerAngles.x = rotation;
				base.transform.eulerAngles = eulerAngles;
			}
			eulerAngles = this.transformToRotate.eulerAngles;
			eulerAngles.x = rotation;
			this.transformToRotate.eulerAngles = eulerAngles;
		}
	}

	private float getRotationForce(float force, float maxForce, float minForce)
	{
		if (Mathf.Abs(force) > maxForce)
		{
			force = maxForce * Mathf.Sign(force);
		}
		else if (Mathf.Abs(force) < minForce)
		{
			force = minForce * Mathf.Sign(force);
		}
		return force;
	}

	private float getCurrentMousePosition()
	{
		return (!this.horizontal) ? UnityEngine.Input.mousePosition.y : UnityEngine.Input.mousePosition.x;
	}

	private bool snapToRotation()
	{
		float num = this.dragDirection.getDirection();
		if (num == 0f)
		{
			return false;
		}
		num = Mathf.Sign(num) * -1f;
		return this.snapToRotationWithDir(num);
	}

	private bool snapToRotationWithDir(float dir)
	{
		float snapRotationFromDirection = this.getSnapRotationFromDirection(dir, this.lastRotation);
		float snapRotationFromDirection2 = this.getSnapRotationFromDirection(dir, this.transformToRotate.eulerAngles.y);
		bool flag = snapRotationFromDirection2 != snapRotationFromDirection;
		if (Mathf.Abs(this.transformToRotate.eulerAngles.y - snapRotationFromDirection2) < ObjectRotator.SNAP_DISTANCE || flag)
		{
			Vector3 eulerAngles = this.transformToRotate.eulerAngles;
			eulerAngles.y = ((!flag) ? snapRotationFromDirection2 : snapRotationFromDirection);
			if (this.transformToRotate != base.transform && !this.insideTransformToRotate)
			{
				base.transform.eulerAngles = eulerAngles;
			}
			this.transformToRotate.eulerAngles = eulerAngles;
			this.dragForce.clear();
			return true;
		}
		return false;
	}

	private float getSnapRotationFromDirection(float dir, float rotation)
	{
		float num = Mathf.Floor((rotation - 0.1f) / 90f);
		if (dir > 0f)
		{
			num = Mathf.Ceil((rotation + 0.1f) / 90f);
		}
		if (num < 0f)
		{
			num = 3f;
		}
		else if (num > 4f)
		{
			num = 0f;
		}
		return num * 90f;
	}

	private float getClosestSnapDirection()
	{
		float num = Mathf.Abs(this.transformToRotate.rotation.eulerAngles.y - this.getSnapRotationFromDirection(-1f, this.transformToRotate.eulerAngles.y));
		float num2 = Mathf.Abs(this.transformToRotate.rotation.eulerAngles.y - this.getSnapRotationFromDirection(1f, this.transformToRotate.eulerAngles.y));
		return (float)((num >= num2) ? 1 : (-1));
	}

	private float getClosestSnapRotation()
	{
		float snapRotationFromDirection = this.getSnapRotationFromDirection(-1f, this.transformToRotate.eulerAngles.y);
		float snapRotationFromDirection2 = this.getSnapRotationFromDirection(1f, this.transformToRotate.eulerAngles.y);
		float num = Mathf.Abs(this.transformToRotate.rotation.eulerAngles.y - snapRotationFromDirection);
		float num2 = Mathf.Abs(this.transformToRotate.rotation.eulerAngles.y - snapRotationFromDirection2);
		return (num >= num2) ? snapRotationFromDirection2 : snapRotationFromDirection;
	}

	public static float elasticEaseInOut(float t, float b, float c, float d)
	{
		t /= d;
		float num = t * t;
		float num2 = num * t;
		return b + c * (22.645f * num2 * num + -50.09f * num * num + 36.495f * num2 + -11.8f * num + 3.75f * t);
	}

	public IEnumerator slowRotationDown(float fromAngle, float toAngle)
	{
		ObjectRotator._slowRotationDown_c__Iterator0 _slowRotationDown_c__Iterator = new ObjectRotator._slowRotationDown_c__Iterator0();
		_slowRotationDown_c__Iterator.toAngle = toAngle;
		_slowRotationDown_c__Iterator.fromAngle = fromAngle;
		_slowRotationDown_c__Iterator._this = this;
		return _slowRotationDown_c__Iterator;
	}
}
