//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なGradient型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Gradient type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleGradient : FlexibleField<Gradient>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGradientコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
		/// <remarks>valueはコピーされず参照を持ち続けます。</remarks>
#else
		/// <summary>
		/// FlexibleGradient constructor
		/// </summary>
		/// <param name="value">Value</param>
		/// <remarks>value is not copied and keeps reference.</remarks>
#endif
		public FlexibleGradient(Gradient value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGradientコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleGradient constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleGradient(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGradientコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleGradient constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleGradient(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGradientをGradientにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleGradient</param>
#else
		/// <summary>
		/// Cast FlexibleGradient to Gradient.
		/// </summary>
		/// <param name="flexible">FlexibleGradient</param>
#endif
		public static explicit operator Gradient(FlexibleGradient flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GradientをFlexibleGradientにキャスト。
		/// </summary>
		/// <param name="value">Gradient</param>
		/// <remarks>valueはコピーされ、参照されるインスタンスが変わります。</remarks>
#else
		/// <summary>
		/// Cast Gradient to FlexibleGradient.
		/// </summary>
		/// <param name="value">Gradient</param>
		/// <remarks>value is copied, and the referenced instance changes.</remarks>
#endif
		public static explicit operator FlexibleGradient(Gradient value)
		{
			Gradient newGradient = new Gradient();
			newGradient.SetKeys(value.colorKeys, value.alphaKeys);
			return new FlexibleGradient(newGradient);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// Gradient型の入力スロット
	/// </summary>
#else
	/// <summary>
	/// Gradient type of input slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class InputSlotGradient : InputSlot<Gradient>
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// Gradient型の出力スロット
	/// </summary>
#else
	/// <summary>
	/// Gradient type of output slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class OutputSlotGradient : OutputSlot<Gradient>
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// Gradient型のVariable
	/// </summary>
#else
	/// <summary>
	/// Gradient type Variable
	/// </summary>
#endif
	[AddVariableMenu("UnityEngine/Gradient")]
	[BehaviourTitle("Gradient")]
	[AddComponentMenu("")]
	public sealed class GradientVariable : Variable<Gradient>
	{
	}
}