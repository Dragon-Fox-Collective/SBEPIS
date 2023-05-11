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
	/// 矩形のYMax成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the YMax component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.SetYMax")]
	[BehaviourTitle("Rect.SetYMax")]
	[BuiltInBehaviour]
	public sealed class RectSetYMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMax成分
		/// </summary>
#else
		/// <summary>
		/// YMax component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _YMax = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotRect _Result = new OutputSlotRect();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect value = _Rect.value;
			value.yMax = _YMax.value;
			_Result.SetValue(value);
		}
	}
}
