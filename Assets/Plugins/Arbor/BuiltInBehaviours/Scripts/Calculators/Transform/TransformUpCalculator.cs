//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド空間の Transform の緑軸
	/// </summary>
#else
	/// <summary>
	/// The green axis of the transform in world space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.Up")]
	[BehaviourTitle("Transform.Up")]
	[BuiltInBehaviour]
	public sealed class TransformUpCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド空間の Transform の緑軸
	/// </summary>
#else
		/// <summary>
		/// The green axis of the transform in world space.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Up = new OutputSlotVector3();

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
				_Up.SetValue(transform.up);
			}
		}
	}
}
