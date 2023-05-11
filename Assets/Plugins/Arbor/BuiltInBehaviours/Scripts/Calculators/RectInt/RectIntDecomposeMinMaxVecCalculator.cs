//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectIntをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose RectInt into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.DecomposeMinMaxVec")]
	[BehaviourTitle("RectInt.DecomposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class RectIntDecomposeMinMaxVecCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Min = new OutputSlotVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Max = new OutputSlotVector2Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rect = _RectInt.value;
			_Min.SetValue(rect.min);
			_Max.SetValue(rect.max);
		}
	}
}