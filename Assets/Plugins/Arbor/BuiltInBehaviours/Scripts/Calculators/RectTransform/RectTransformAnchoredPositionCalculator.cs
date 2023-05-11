//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アンカー基準点に対する RectTransform の相対的なピボットの位置
	/// </summary>
#else
	/// <summary>
	/// The position of the pivot of this RectTransform relative to the anchor reference point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.AnchoredPosition")]
	[BehaviourTitle("RectTransform.AnchoredPosition")]
	[BuiltInBehaviour]
	public sealed class RectTransformAnchoredPositionCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectTransform
		/// </summary>
		[SerializeField] private FlexibleRectTransform _RectTransform = new FlexibleRectTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ピボットの位置
		/// </summary>
#else
		/// <summary>
		/// The position of the pivot
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _AnchoredPosition = new OutputSlotVector2();

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
				_AnchoredPosition.SetValue(rectTransform.anchoredPosition);
			}
		}
	}
}
