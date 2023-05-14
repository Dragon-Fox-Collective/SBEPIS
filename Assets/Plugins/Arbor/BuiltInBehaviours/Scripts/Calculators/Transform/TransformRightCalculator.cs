//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド空間の Transform の赤軸
	/// </summary>
#else
	/// <summary>
	/// The red axis of the transform in world space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.Right")]
	[BehaviourTitle("Transform.Right")]
	[BuiltInBehaviour]
	public sealed class TransformRightCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ワールド空間の Transform の赤軸
		/// </summary>
#else
		/// <summary>
		/// The red axis of the transform in world space.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Right = new OutputSlotVector3();

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
				_Right.SetValue(transform.right);
			}
		}
	}
}
