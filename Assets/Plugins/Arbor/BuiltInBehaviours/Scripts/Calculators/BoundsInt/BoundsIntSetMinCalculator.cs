//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの最小コーナーの位置を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the position of the minimum corner of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.SetMin")]
	[BehaviourTitle("BoundsInt.SetMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntSetMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最小コーナーの位置
		/// </summary>
#else
		/// <summary>
		/// the position of the minimum corner of the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Min = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBoundsInt _Result = new OutputSlotBoundsInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt value = _BoundsInt.value;
			value.min = _Min.value;
			_Result.SetValue(value);
		}
	}
}