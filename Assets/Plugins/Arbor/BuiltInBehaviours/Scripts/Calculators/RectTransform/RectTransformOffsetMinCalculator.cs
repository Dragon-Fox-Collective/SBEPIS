//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 左下のアンカーを基準にした矩形の左下角のオフセット
	/// </summary>
#else
	/// <summary>
	/// The offset of the lower left corner of the rectangle relative to the lower left anchor.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.OffsetMin")]
	[BehaviourTitle("RectTransform.OffsetMin")]
	[BuiltInBehaviour]
	public sealed class RectTransformOffsetMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectTransform
		/// </summary>
		[SerializeField] private FlexibleRectTransform _RectTransform = new FlexibleRectTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の左下角のオフセット
		/// </summary>
#else
		/// <summary>
		/// The offset of the lower left corner of the rectangle
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _OffsetMin = new OutputSlotVector2();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			RectTransform rectTransform = _RectTransform.value;
			if (rectTransform != null)
			{
				_OffsetMin.SetValue(rectTransform.offsetMin);
			}
		}
	}
}
