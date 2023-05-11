//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのZMin成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the ZMin component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.SetZMin")]
	[BehaviourTitle("BoundsInt.SetZMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntSetZMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// ZMin成分
		/// </summary>
#else
		/// <summary>
		/// ZMin component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _ZMin = new FlexibleInt();

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
			value.zMin = _ZMin.value;
			_Result.SetValue(value);
		}
	}
}