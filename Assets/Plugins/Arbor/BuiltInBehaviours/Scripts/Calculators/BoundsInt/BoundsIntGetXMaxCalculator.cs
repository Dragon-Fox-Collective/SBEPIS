//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのXMax成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the XMax component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetXMax")]
	[BehaviourTitle("BoundsInt.GetXMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetXMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output XMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _XMax = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_XMax.SetValue(_BoundsInt.value.xMax);
		}
	}
}