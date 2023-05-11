//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのXMax成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the XMax component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.SetXMax")]
	[BehaviourTitle("BoundsInt.SetXMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntSetXMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMax成分
		/// </summary>
#else
		/// <summary>
		/// XMax component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _XMax = new FlexibleInt();

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
			value.xMax = _XMax.value;
			_Result.SetValue(value);
		}
	}
}