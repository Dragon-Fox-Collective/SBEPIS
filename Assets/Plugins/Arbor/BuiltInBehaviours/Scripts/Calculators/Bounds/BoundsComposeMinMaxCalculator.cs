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
	[AddBehaviourMenu("Bounds/Bounds.ComposeMinMax")]
	[BehaviourTitle("Bounds.ComposeMinMax")]
	[BuiltInBehaviour]
	public sealed class BoundsComposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _XMin = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _YMin = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _ZMin = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _XMax = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _YMax = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _ZMax = new FlexibleFloat();

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
			bounds.SetMinMax(new Vector3(_XMin.value, _YMin.value, _ZMin.value), new Vector3(_XMax.value, _YMax.value, _ZMax.value));
			_Result.SetValue(bounds);
		}
	}
}
