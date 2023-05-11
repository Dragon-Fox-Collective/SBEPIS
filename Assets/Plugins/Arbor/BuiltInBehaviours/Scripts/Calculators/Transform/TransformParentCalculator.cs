//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Transform の親
	/// </summary>
#else
	/// <summary>
	/// The parent of the transform.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.Parent")]
	[BehaviourTitle("Transform.Parent")]
	[BuiltInBehaviour]
	public sealed class TransformParentCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// Transform の親
		/// </summary>
#else
		/// <summary>
		/// The parent of the transform.
		/// </summary>
#endif
		[SerializeField] private OutputSlotTransform _Parent = new OutputSlotTransform();

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
				_Parent.SetValue(transform.parent);
			}
		}
	}
}
