//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アンカー基準点に対する RectTransform の相対的なピボットの 3D の位置
	/// </summary>
#else
	/// <summary>
	/// The 3D position of the pivot of this RectTransform relative to the anchor reference point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.AnchoredPosition3D")]
	[BehaviourTitle("RectTransform.AnchoredPosition3D")]
	[BuiltInBehaviour]
	public sealed class RectTransformAnchoredPosition3DCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectTransform
		/// </summary>
		[SerializeField] private FlexibleRectTransform _RectTransform = new FlexibleRectTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ピボットの 3D の位置
		/// </summary>
#else
		/// <summary>
		/// The 3D position of the pivot 
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _AnchoredPosition3D = new OutputSlotVector3();

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
				_AnchoredPosition3D.SetValue(rectTransform.anchoredPosition3D);
			}
		}
	}
}
