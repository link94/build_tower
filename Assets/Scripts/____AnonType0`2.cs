using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

internal sealed class ____AnonType0<_k___T, _v___T>
{
	private readonly _k___T _k_;

	private readonly _v___T _v_;

	public _k___T k
	{
		get
		{
			return this._k_;
		}
	}

	public _v___T v
	{
		get
		{
			return this._v_;
		}
	}

	public ____AnonType0(_k___T k, _v___T v)
	{
		this._k_ = k;
		this._v_ = v;
	}

	public override bool Equals(object obj)
	{
		var ____AnonType = obj as ____AnonType0<_k___T, _v___T>;
		return ____AnonType != null && EqualityComparer<_k___T>.Default.Equals(this._k_, ____AnonType._k_) && EqualityComparer<_v___T>.Default.Equals(this._v_, ____AnonType._v_);
	}

	public override int GetHashCode()
	{
		int num = ((-2128831035 ^ EqualityComparer<_k___T>.Default.GetHashCode(this._k_)) * 16777619 ^ EqualityComparer<_v___T>.Default.GetHashCode(this._v_)) * 16777619;
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return num + (num << 5);
	}

	public override string ToString()
	{
		string[] expr_06 = new string[6];
		expr_06[0] = "{";
		expr_06[1] = " k = ";
		int arg_46_1 = 2;
		string arg_46_2;
		if (this._k_ != null)
		{
			_k___T _k___T = this._k_;
			arg_46_2 = _k___T.ToString();
		}
		else
		{
			arg_46_2 = string.Empty;
		}
		expr_06[arg_46_1] = arg_46_2;
		expr_06[3] = ", v = ";
		int arg_7F_1 = 4;
		string arg_7F_2;
		if (this._v_ != null)
		{
			_v___T _v___T = this._v_;
			arg_7F_2 = _v___T.ToString();
		}
		else
		{
			arg_7F_2 = string.Empty;
		}
		expr_06[arg_7F_1] = arg_7F_2;
		expr_06[5] = " }";
		return string.Concat(expr_06);
	}
}
