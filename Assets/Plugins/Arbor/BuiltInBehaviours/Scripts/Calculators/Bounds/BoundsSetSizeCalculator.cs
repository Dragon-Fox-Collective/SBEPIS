//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsのサイズを設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the size of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.SetSize")]
	[BehaviourTitle("Bounds.SetSize")]
	[BuiltInBehaviour]
	public sealed class BoundsSetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Boundsのサイズ
		/// </summary>
#else
		/// <summary>
		/// the size of the Bounds.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Size = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds value = _Bounds.value;
			value.size = _Size.value;
			_Result.SetValue(value);
		}
	}
}
