//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Quaternion として保存されるワールド空間での Transform の回転
	/// </summary>
#else
	/// <summary>
	/// The rotation of the transform in world space stored as a Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.Rotation")]
	[BehaviourTitle("Transform.Rotation")]
	[BuiltInBehaviour]
	public sealed class TransformRotationCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ワールド空間での Transform の回転
		/// </summary>
#else
		/// <summary>
		/// The rotation of the transform in world space.
		/// </summary>
#endif
		[SerializeField] private OutputSlotQuaternion _Rotation = new OutputSlotQuaternion();

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
				_Rotation.SetValue(transform.rotation);
			}
		}
	}
}
