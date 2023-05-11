//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの最大コーナーの位置を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the position of the maximum corner of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.SetMax")]
	[BehaviourTitle("BoundsInt.SetMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntSetMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大コーナーの位置
		/// </summary>
#else
		/// <summary>
		/// the position of the maximum corner of the rect.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Max = new FlexibleVector3Int();

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
			value.max = _Max.value;
			_Result.SetValue(value);
		}
	}
}