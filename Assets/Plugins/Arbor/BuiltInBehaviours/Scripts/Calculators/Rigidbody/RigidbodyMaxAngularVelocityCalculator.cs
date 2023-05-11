//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigidbody の最大角速度。
	/// </summary>
#else
	/// <summary>
	/// The maximimum angular velocity of the rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.MaxAngularVelocity")]
	[BehaviourTitle("Rigidbody.MaxAngularVelocity")]
	[BuiltInBehaviour]
	public sealed class RigidbodyMaxAngularVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大角速度。
		/// </summary>
#else
		/// <summary>
		/// The maximimum angular velocity.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _MaxAngularVelocity = new OutputSlotFloat();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody rigidbody = _Rigidbody.value;
			if (rigidbody != null)
			{
				_MaxAngularVelocity.SetValue(rigidbody.maxAngularVelocity);
			}
		}
	}
}
