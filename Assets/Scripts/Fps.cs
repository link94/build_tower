using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fps : MonoBehaviour
{
	private sealed class _Start_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal Fps _this;

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

		public _Start_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.style.normal.textColor = this._this.textColor;
				GUI.depth = 2;
				break;
			case 1u:
				this._this.count = 1f / Time.deltaTime;
				this._this.label = "FPS :" + Mathf.Round(this._this.count);
				goto IL_D3;
			case 2u:
				break;
			default:
				return false;
			}
			if (Time.timeScale == 1f)
			{
				this._current = new WaitForSeconds(0.1f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.label = "Pause";
			IL_D3:
			this._current = new WaitForSeconds(0.5f);
			if (!this._disposing)
			{
				this._PC = 2;
			}
			return true;
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

	public Color textColor = Color.white;

	private string label = string.Empty;

	private float count;

	private GUIStyle style = new GUIStyle();

	private IEnumerator Start()
	{
		Fps._Start_c__Iterator0 _Start_c__Iterator = new Fps._Start_c__Iterator0();
		_Start_c__Iterator._this = this;
		return _Start_c__Iterator;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(5f, 10f, 100f, 25f), this.label, this.style);
	}
}
