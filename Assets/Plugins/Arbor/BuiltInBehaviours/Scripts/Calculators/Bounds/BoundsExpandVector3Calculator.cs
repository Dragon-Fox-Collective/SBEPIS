//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 設定した amount によってサイズを大きくする。
	/// </summary>
#else
	/// <summary>
	/// Expand the bounds by increasing its size by amount along each side.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.ExpandVector3")]
	[BehaviourTitle("Bounds.ExpandVector3")]
	[BuiltInBehaviour]
	public sealed class BoundsExpandVector3Calculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 拡大する量
		/// </summary>
#else
		/// <summary>
		/// Amount to expand
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Amount = new FlexibleVector3();

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
			Bounds bounds = _Bounds.value;
			bounds.Expand(_Amount.value);
			_Result.SetValue(bounds);
		}
	}
}
