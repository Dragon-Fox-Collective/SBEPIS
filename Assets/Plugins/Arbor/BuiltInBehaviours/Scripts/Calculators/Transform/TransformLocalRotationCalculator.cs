//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 親の Transform オブジェクトから見た相対的な回転値
	/// </summary>
#else
	/// <summary>
	/// The rotation of the transform relative to the parent transform's rotation.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.LocalRotation")]
	[BehaviourTitle("Transform.LocalRotation")]
	[BuiltInBehaviour]
	public sealed class TransformLocalRotationCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 相対的な回転値
		/// </summary>
#else
		/// <summary>
		/// The rotation of the transform relative.
		/// </summary>
#endif
		[SerializeField] private OutputSlotQuaternion _LocalRotation = new OutputSlotQuaternion();

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
				_LocalRotation.SetValue(transform.localRotation);
			}
		}
	}
}
