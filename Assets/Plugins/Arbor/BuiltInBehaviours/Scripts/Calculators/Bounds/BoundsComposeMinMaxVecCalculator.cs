//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxからBoundsを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create Bounds from Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.ComposeMinMaxVec")]
	[BehaviourTitle("Bounds.ComposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class BoundsComposeMinMaxVecCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Min = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Max = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds bounds = new Bounds();
			bounds.SetMinMax(_Min.value, _Max.value);
			_Result.SetValue(bounds);
		}
	}
}
