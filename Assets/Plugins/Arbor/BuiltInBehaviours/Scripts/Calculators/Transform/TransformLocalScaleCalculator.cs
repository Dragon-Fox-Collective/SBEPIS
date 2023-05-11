//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 親の Transform オブジェクトから見た相対的なスケール
	/// </summary>
#else
	/// <summary>
	/// The scale of the transform relative to the parent.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.LocalScale")]
	[BehaviourTitle("Transform.LocalScale")]
	[BuiltInBehaviour]
	public sealed class TransformLocalScaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 相対的なスケール
		/// </summary>
#else
		/// <summary>
		/// he scale of the transform relative.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _LocalScale = new OutputSlotVector3();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Transform transform = _Transform.value;
			if (transform != null)
			{
				_LocalScale.SetValue(transform.localScale);
			}
		}
	}
}
