using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
	public static float kDefaultButtonWidth = 150f;

	public static float kDefaultButtonHeight = 150f;

	public readonly string MethodName;

	private float _buttonWidth = InspectorButtonAttribute.kDefaultButtonWidth;

	private float _buttonHeight = InspectorButtonAttribute.kDefaultButtonHeight;

	public float ButtonWidth
	{
		get
		{
			return this._buttonWidth;
		}
		set
		{
			this._buttonWidth = value;
		}
	}

	public float ButtonHeight
	{
		get
		{
			return this._buttonHeight;
		}
		set
		{
			this._buttonHeight = value;
		}
	}

	public InspectorButtonAttribute(string MethodName)
	{
		this.MethodName = MethodName;
	}
}
