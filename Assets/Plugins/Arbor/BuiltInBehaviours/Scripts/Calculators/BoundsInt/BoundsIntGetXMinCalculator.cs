//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのXMin成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the XMin component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetXMin")]
	[BehaviourTitle("BoundsInt.GetXMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetXMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMin成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output XMin component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _XMin = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_XMin.SetValue(_BoundsInt.value.xMin);
		}
	}
}