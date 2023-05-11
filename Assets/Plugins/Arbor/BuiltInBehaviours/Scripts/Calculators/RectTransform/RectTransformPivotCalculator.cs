//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 周囲を回転させるこのRectTransformの正規化された位置
	/// </summary>
#else
	/// <summary>
	/// The normalized position in this RectTransform that it rotates around.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.Pivot")]
	[BehaviourTitle("RectTransform.Pivot")]
	[BuiltInBehaviour]
	public sealed class RectTransformPivotCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectTransform
		/// </summary>
		[SerializeField] private FlexibleRectTransform _RectTransform = new FlexibleRectTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// このRectTransformの正規化された位置
		/// </summary>
#else
		/// <summary>
		/// The normalized position in this RectTransform
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Pivot = new OutputSlotVector2();

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
				_Pivot.SetValue(rectTransform.pivot);
			}
		}
	}
}
