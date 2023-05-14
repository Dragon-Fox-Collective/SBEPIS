//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.FlexibleInt"/>、<see cref="Arbor.FlexibleLong"/>、<see cref="Arbor.FlexibleFloat"/>のタイプがConstantの時に範囲を制限する。
	/// </summary>
#else
	/// <summary>
	/// Limit the value range when <see cref = "Arbor.FlexibleInt" /> , <see cref="Arbor.FlexibleLong"/> , <see cref = "Arbor.FlexibleFloat" /> type is Constant.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class ConstantRangeAttribute : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 最小値
		/// </summary>
#else
		/// <summary>
		/// Min value
		/// </summary>
#endif
		public readonly float min;

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大値
		/// </summary>
#else
		/// <summary>
		/// Max value
		/// </summary>
#endif
		public readonly float max;

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="Arbor.FlexibleInt"/>、<see cref="Arbor.FlexibleLong"/>、<see cref="Arbor.FlexibleInt"/>のタイプがConstantの時に範囲を制限する。
		/// </summary>
		/// <param name="min">最小値</param>
		/// <param name="max">最大値</param>
#else
		/// <summary>
		/// Limit the value range when <see cref = "Arbor.FlexibleInt" /> , <see cref="Arbor.FlexibleLong"/> , <see cref = "Arbor.FlexibleFloat" /> type is Constant.
		/// </summary>
		/// <param name="min">Min Value</param>
		/// <param name="max">Max Value</param>
#endif
		public ConstantRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}