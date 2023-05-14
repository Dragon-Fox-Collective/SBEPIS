//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アンカー間の距離と比較した RectTransform のサイズ。
	/// </summary>
#else
	/// <summary>
	/// The size of this RectTransform relative to the distances between the anchors.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.SizeDelta")]
	[BehaviourTitle("RectTransform.SizeDelta")]
	[BuiltInBehaviour]
	public sealed class RectTransformSizeDeltaCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectTransform
		/// </summary>
		[SerializeField] private FlexibleRectTransform _RectTransform = new FlexibleRectTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// サイズ
		/// </summary>
#else
		/// <summary>
		/// Size
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _SizeDelta = new OutputSlotVector2();

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
				_SizeDelta.SetValue(rectTransform.sizeDelta);
			}
		}
	}
}
