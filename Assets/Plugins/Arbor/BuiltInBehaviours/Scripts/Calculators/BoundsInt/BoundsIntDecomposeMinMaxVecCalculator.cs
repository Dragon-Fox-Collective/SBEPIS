//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose BoundsInt into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.DecomposeMinMaxVec")]
	[BehaviourTitle("BoundsInt.DecomposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class BoundsIntDecomposeMinMaxVecCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Min = new OutputSlotVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Max = new OutputSlotVector3Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt bounds = _BoundsInt.value;
			_Min.SetValue(bounds.min);
			_Max.SetValue(bounds.max);
		}
	}
}