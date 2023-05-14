//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の最大コーナーの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the maximum corner of the rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetMax")]
	[BehaviourTitle("Rect.GetMax")]
	[BuiltInBehaviour]
	public sealed class RectGetMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の最大コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the maximum corner of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Max = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Max.SetValue(_Rect.value.max);
		}
	}
}
