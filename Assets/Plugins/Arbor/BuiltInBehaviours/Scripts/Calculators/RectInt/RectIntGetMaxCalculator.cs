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
	[AddBehaviourMenu("RectInt/RectInt.GetMax")]
	[BehaviourTitle("RectInt.GetMax")]
	[BuiltInBehaviour]
	public sealed class RectIntGetMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の最大コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the maximum corner of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Max = new OutputSlotVector2Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Max.SetValue(_RectInt.value.max);
		}
	}
}