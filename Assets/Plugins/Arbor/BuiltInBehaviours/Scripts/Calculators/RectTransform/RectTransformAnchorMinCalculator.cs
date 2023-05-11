//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 左下の角をアンカーした親 RectTransform で正規化された位置
	/// </summary>
#else
	/// <summary>
	/// The normalized position in the parent RectTransform that the lower left corner is anchored to.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.AnchorMin")]
	[BehaviourTitle("RectTransform.AnchorMin")]
	[BuiltInBehaviour]
	public sealed class RectTransformAnchorMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectTransform
		/// </summary>
		[SerializeField] private FlexibleRectTransform _RectTransform = new FlexibleRectTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// アンカーの最小の位置
		/// </summary>
#else
		/// <summary>
		/// Minumum position of anchor
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _AnchorMin = new OutputSlotVector2();

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
				_AnchorMin.SetValue(rectTransform.anchorMin);
			}
		}
	}
}
