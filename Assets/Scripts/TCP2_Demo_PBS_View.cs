using System;
using UnityEngine;

public class TCP2_Demo_PBS_View : MonoBehaviour
{
	public Transform Pivot;

	[Header("Orbit")]
	public float OrbitStrg = 3f;

	public float OrbitClamp = 50f;

	[Header("Panning")]
	public float PanStrg = 0.1f;

	public float PanClamp = 2f;

	public float yMin;

	public float yMax;

	[Header("Zooming")]
	public float ZoomStrg = 40f;

	public float ZoomClamp = 30f;

	public float ZoomDistMin = 1f;

	public float ZoomDistMax = 2f;

	[Header("Misc")]
	public float Decceleration = 8f;

	public Rect ignoreMouseRect;

	private Vector3 mouseDelta;

	private Vector3 orbitAcceleration;

	private Vector3 panAcceleration;

	private Vector3 moveAcceleration;

	private float zoomAcceleration;

	private const float XMax = 60f;

	private const float XMin = 300f;

	private Vector3 mResetCamPos;

	private Vector3 mResetPivotPos;

	private Vector3 mResetCamRot;

	private Vector3 mResetPivotRot;

	private bool leftMouseHeld;

	private bool rightMouseHeld;

	private bool middleMouseHeld;

	private void Awake()
	{
		this.mResetCamPos = base.transform.position;
		this.mResetCamRot = base.transform.eulerAngles;
		this.mResetPivotPos = this.Pivot.position;
		this.mResetPivotRot = this.Pivot.eulerAngles;
	}

	private void OnEnable()
	{
		this.mouseDelta = UnityEngine.Input.mousePosition;
	}

	private void Update()
	{
		this.mouseDelta = UnityEngine.Input.mousePosition - this.mouseDelta;
		Rect rect = this.ignoreMouseRect;
		rect.x = (float)Screen.width - this.ignoreMouseRect.width;
		bool flag = rect.Contains(UnityEngine.Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			this.leftMouseHeld = !flag;
		}
		else if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
		{
			this.leftMouseHeld = false;
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.rightMouseHeld = !flag;
		}
		else if (Input.GetMouseButtonUp(1) || !Input.GetMouseButton(1))
		{
			this.rightMouseHeld = false;
		}
		if (Input.GetMouseButtonDown(2))
		{
			this.middleMouseHeld = !flag;
		}
		else if (Input.GetMouseButtonUp(2) || !Input.GetMouseButton(2))
		{
			this.middleMouseHeld = false;
		}
		if (this.leftMouseHeld)
		{
			this.orbitAcceleration.x = this.orbitAcceleration.x + Mathf.Clamp(this.mouseDelta.x * this.OrbitStrg, -this.OrbitClamp, this.OrbitClamp);
			this.orbitAcceleration.y = this.orbitAcceleration.y + Mathf.Clamp(-this.mouseDelta.y * this.OrbitStrg, -this.OrbitClamp, this.OrbitClamp);
		}
		else if (this.middleMouseHeld || this.rightMouseHeld)
		{
			this.panAcceleration.y = this.panAcceleration.y + Mathf.Clamp(-this.mouseDelta.y * this.PanStrg, -this.PanClamp, this.PanClamp);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			this.ResetView();
		}
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		if (localEulerAngles.x < 180f && localEulerAngles.x >= 60f && this.orbitAcceleration.y > 0f)
		{
			this.orbitAcceleration.y = 0f;
		}
		if (localEulerAngles.x > 180f && localEulerAngles.x <= 300f && this.orbitAcceleration.y < 0f)
		{
			this.orbitAcceleration.y = 0f;
		}
		base.transform.RotateAround(this.Pivot.position, base.transform.right, this.orbitAcceleration.y * Time.deltaTime);
		base.transform.RotateAround(this.Pivot.position, Vector3.up, this.orbitAcceleration.x * Time.deltaTime);
		Vector3 position = this.Pivot.transform.position;
		float num = position.y;
		position.y += this.panAcceleration.y * Time.deltaTime;
		position.y = Mathf.Clamp(position.y, this.yMin, this.yMax);
		num = position.y - num;
		this.Pivot.transform.position = position;
		position = base.transform.position;
		position.y += num;
		base.transform.position = position;
		float axis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
		this.zoomAcceleration += axis * this.ZoomStrg;
		this.zoomAcceleration = Mathf.Clamp(this.zoomAcceleration, -this.ZoomClamp, this.ZoomClamp);
		float num2 = Vector3.Distance(base.transform.position, this.Pivot.position);
		if ((num2 >= this.ZoomDistMin && this.zoomAcceleration > 0f) || (num2 <= this.ZoomDistMax && this.zoomAcceleration < 0f))
		{
			base.transform.Translate(Vector3.forward * this.zoomAcceleration * Time.deltaTime, Space.Self);
		}
		this.orbitAcceleration = Vector3.Lerp(this.orbitAcceleration, Vector3.zero, this.Decceleration * Time.deltaTime);
		this.panAcceleration = Vector3.Lerp(this.panAcceleration, Vector3.zero, this.Decceleration * Time.deltaTime);
		this.zoomAcceleration = Mathf.Lerp(this.zoomAcceleration, 0f, this.Decceleration * Time.deltaTime);
		this.moveAcceleration = Vector3.Lerp(this.moveAcceleration, Vector3.zero, this.Decceleration * Time.deltaTime);
		this.mouseDelta = UnityEngine.Input.mousePosition;
	}

	public void ResetView()
	{
		this.moveAcceleration = Vector3.zero;
		this.orbitAcceleration = Vector3.zero;
		this.panAcceleration = Vector3.zero;
		this.zoomAcceleration = 0f;
		base.transform.position = this.mResetCamPos;
		base.transform.eulerAngles = this.mResetCamRot;
		this.Pivot.position = this.mResetPivotPos;
		this.Pivot.eulerAngles = this.mResetPivotRot;
	}
}
