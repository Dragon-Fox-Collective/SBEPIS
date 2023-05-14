//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// オブジェクトのグローバルスケール
	/// </summary>
#else
	/// <summary>
	/// The global scale of the object
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.LossyScale")]
	[BehaviourTitle("Transform.LossyScale")]
	[BuiltInBehaviour]
	public sealed class TransformLossyScaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトのグローバルスケール
		/// </summary>
#else
		/// <summary>
		/// The global scale of the object
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _LossyScale = new OutputSlotVector3();

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
				_LossyScale.SetValue(transform.lossyScale);
			}
		}
	}
}
