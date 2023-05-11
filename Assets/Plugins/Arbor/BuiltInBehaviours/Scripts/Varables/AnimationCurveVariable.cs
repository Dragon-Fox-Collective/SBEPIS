//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なAnimationCurve型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible AnimationCurve type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleAnimationCurve : FlexibleField<AnimationCurve>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAnimationCurveコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
		/// <remarks>valueはコピーされず参照を持ち続けます。</remarks>
#else
		/// <summary>
		/// FlexibleAnimationCurve constructor
		/// </summary>
		/// <param name="value">Value</param>
		/// <remarks>value is not copied and keeps reference.</remarks>
#endif
		public FlexibleAnimationCurve(AnimationCurve value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAnimationCurveコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleAnimationCurve constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleAnimationCurve(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAnimationCurveコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleAnimationCurve constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleAnimationCurve(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAnimationCurveをAnimationCurveにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleAnimationCurve</param>
#else
		/// <summary>
		/// Cast FlexibleAnimationCurve to AnimationCurve.
		/// </summary>
		/// <param name="flexible">FlexibleAnimationCurve</param>
#endif
		public static explicit operator AnimationCurve(FlexibleAnimationCurve flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimationCurveをFlexibleAnimationCurveにキャスト。
		/// </summary>
		/// <param name="value">AnimationCurve</param>
		/// <remarks>valueはコピーされ、参照されるインスタンスが変わります。</remarks>
#else
		/// <summary>
		/// Cast AnimationCurve to FlexibleAnimationCurve.
		/// </summary>
		/// <param name="value">AnimationCurve</param>
		/// <remarks>value is copied, and the referenced instance changes.</remarks>
#endif
		public static explicit operator FlexibleAnimationCurve(AnimationCurve value)
		{
			return new FlexibleAnimationCurve(new AnimationCurve(value.keys));
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// AnimationCurve型の入力スロット
	/// </summary>
#else
	/// <summary>
	/// AnimationCurve type of input slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class InputSlotAnimationCurve : InputSlot<AnimationCurve>
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// AnimationCurve型の出力スロット
	/// </summary>
#else
	/// <summary>
	/// AnimationCurve type of output slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class OutputSlotAnimationCurve : OutputSlot<AnimationCurve>
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// AnimationCurve型のVariable
	/// </summary>
#else
	/// <summary>
	/// AnimationCurve type Variable
	/// </summary>
#endif
	[AddVariableMenu("UnityEngine/AnimationCurve")]
	[BehaviourTitle("AnimationCurve")]
	[AddComponentMenu("")]
	public sealed class AnimationCurveVariable : Variable<AnimationCurve>
	{
	}
}