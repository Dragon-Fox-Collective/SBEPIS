//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのYMax成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the YMax component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetYMax")]
	[BehaviourTitle("BoundsInt.GetYMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetYMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output YMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _YMax = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_YMax.SetValue(_BoundsInt.value.yMax);
		}
	}
}