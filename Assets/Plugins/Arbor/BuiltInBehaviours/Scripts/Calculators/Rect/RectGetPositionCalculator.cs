//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetPosition")]
	[BehaviourTitle("Rect.GetPosition")]
	[BuiltInBehaviour]
	public sealed class RectGetPositionCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the minimum corner of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Position = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Position.SetValue(_Rect.value.position);
		}
	}
}
