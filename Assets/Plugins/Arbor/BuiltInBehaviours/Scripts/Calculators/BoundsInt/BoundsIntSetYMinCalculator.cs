//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのYMin成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the YMin component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.SetYMin")]
	[BehaviourTitle("BoundsInt.SetYMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntSetYMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMin成分
		/// </summary>
#else
		/// <summary>
		/// YMin component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _YMin = new FlexibleInt();

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
			value.yMin = _YMin.value;
			_Result.SetValue(value);
		}
	}
}