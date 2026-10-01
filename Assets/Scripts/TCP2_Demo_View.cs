using System;
using UnityEngine;

public class TCP2_Demo_View : MonoBehaviour
{
	[Header("Orbit")]
	public float OrbitStrg = 3f;

	public float OrbitClamp = 50f;

	[Header("Panning")]
	public float PanStrg = 0.1f;

	public float PanClamp = 2f;

	[Header("Zooming")]
	public float ZoomStrg = 40f;

	public float ZoomClamp = 30f;

	[Header("Misc")]
	public float Decceleration = 8f;

	public Transform CharacterTransform;

	private Vector3 mouseDelta;

	private Vector3 orbitAcceleration;

	private Vector3 panAcceleration;

	private Vector3 moveAcceleration;

	private float zoomAcceleration;

	private const float XMax = 60f;

	private const float XMin = 300f;

	private Vector3 prevHit;

	private float hitTimer;

	private Vector3 mResetCamPos;

	private Vector3 mResetCamRot;

	private bool mMouseDown;

	private void Awake()
	{
		this.mResetCamPos = Camera.main.transform.position;
		this.mResetCamRot = Camera.main.transform.eulerAngles;
	}

	private void OnEnable()
	{
		this.mouseDelta = UnityEngine.Input.mousePosition;
	}

	private void Update()
	{
		this.mouseDelta = UnityEngine.Input.mousePosition - this.mouseDelta;
		if (!this.mMouseDown)
		{
			bool arg_60_1;
			if (Input.GetMouseButtonDown(0))
			{
				Rect rect = new Rect(0f, 65f, 230f, 260f);
				if (!rect.Contains(UnityEngine.Input.mousePosition))
				{
					arg_60_1 = true;
					goto IL_60;
				}
			}
			arg_60_1 = false;
			IL_60:
			this.mMouseDown = arg_60_1;
		}
		else
		{
			this.mMouseDown = !Input.GetMouseButtonUp(0);
		}
		if (this.mMouseDown)
		{
			this.orbitAcceleration.y = this.orbitAcceleration.y - Mathf.Clamp(-this.mouseDelta.x * this.OrbitStrg, -this.OrbitClamp, this.OrbitClamp);
		}
		else if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
		{
			this.panAcceleration.y = this.panAcceleration.y + Mathf.Clamp(-this.mouseDelta.y * this.PanStrg, -this.PanClamp, this.PanClamp);
		}
		this.orbitAcceleration.y = this.orbitAcceleration.y + (float)((!Input.GetKey(KeyCode.LeftArrow)) ? ((!Input.GetKey(KeyCode.RightArrow)) ? 0 : (-15)) : 15);
		this.zoomAcceleration += (float)((!Input.GetKey(KeyCode.UpArrow)) ? ((!Input.GetKey(KeyCode.DownArrow)) ? 0 : (-1)) : 1);
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			this.ResetView();
		}
		Vector3 localEulerAngles = Camera.main.transform.localEulerAngles;
		if (localEulerAngles.x < 180f && localEulerAngles.x >= 60f && this.orbitAcceleration.y > 0f)
		{
			this.orbitAcceleration.y = 0f;
		}
		if (localEulerAngles.x > 180f && localEulerAngles.x <= 300f && this.orbitAcceleration.y < 0f)
		{
			this.orbitAcceleration.y = 0f;
		}
		this.CharacterTransform.Rotate(-this.orbitAcceleration * Time.deltaTime, Space.World);
		Camera.main.transform.Translate(this.panAcceleration * Time.deltaTime, Space.World);
		float axis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
		this.zoomAcceleration += axis * this.ZoomStrg;
		this.zoomAcceleration = Mathf.Clamp(this.zoomAcceleration, -this.ZoomClamp, this.ZoomClamp);
		Camera.main.transform.Translate(Vector3.forward * this.zoomAcceleration * Time.deltaTime, Space.World);
		if (Camera.main.transform.position.y > 1.65f)
		{
			Vector3 position = Camera.main.transform.position;
			position.y = 1.65f;
			Camera.main.transform.position = position;
		}
		else if (Camera.main.transform.position.y < 0.3f)
		{
			Vector3 position2 = Camera.main.transform.position;
			position2.y = 0.3f;
			Camera.main.transform.position = position2;
		}
		if (Camera.main.transform.position.z < -1.8f)
		{
			Vector3 position3 = Camera.main.transform.position;
			position3.z = -1.8f;
			Camera.main.transform.position = position3;
		}
		else if (Camera.main.transform.position.z > -0.6f)
		{
			Vector3 position4 = Camera.main.transform.position;
			position4.z = -0.6f;
			Camera.main.transform.position = position4;
		}
		this.orbitAcceleration = Vector3.Lerp(this.orbitAcceleration, Vector3.zero, this.Decceleration * Time.deltaTime);
		this.panAcceleration = Vector3.Lerp(this.panAcceleration, Vector3.zero, this.Decceleration * Time.deltaTime);
		this.zoomAcceleration = Mathf.Lerp(this.zoomAcceleration, 0f, this.Decceleration * Time.deltaTime);
		this.moveAcceleration = Vector3.Lerp(this.moveAcceleration, Vector3.zero, this.Decceleration * Time.deltaTime);
		this.mouseDelta = UnityEngine.Input.mousePosition;
	}

	public void ResetView()
	{
		Camera.main.transform.position = this.mResetCamPos;
		Camera.main.transform.eulerAngles = this.mResetCamRot;
	}
}
