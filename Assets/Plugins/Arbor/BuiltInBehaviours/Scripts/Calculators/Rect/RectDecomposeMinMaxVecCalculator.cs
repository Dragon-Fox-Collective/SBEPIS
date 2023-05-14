//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Rect into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.DecomposeMinMaxVec")]
	[BehaviourTitle("Rect.DecomposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class RectDecomposeMinMaxVecCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Min = new OutputSlotVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Max = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect rect = _Rect.value;
			_Min.SetValue(rect.min);
			_Max.SetValue(rect.max);
		}
	}
}
