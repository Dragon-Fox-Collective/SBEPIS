//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのYMin成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the YMin component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetYMin")]
	[BehaviourTitle("BoundsInt.GetYMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetYMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMin成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output YMin component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _YMin = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_YMin.SetValue(_BoundsInt.value.yMin);
		}
	}
}