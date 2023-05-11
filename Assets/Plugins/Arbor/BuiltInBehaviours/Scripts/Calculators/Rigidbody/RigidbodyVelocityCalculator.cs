//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigidbodyの速度
	/// </summary>
#else
	/// <summary>
	/// The velocity of the rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.Velocity")]
	[BehaviourTitle("Rigidbody.Velocity")]
	[BuiltInBehaviour]
	public sealed class RigidbodyVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 速度。
		/// </summary>
#else
		/// <summary>
		/// Velocity.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Velocity = new OutputSlotVector3();

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
				_Velocity.SetValue(rigidbody.velocity);
			}
		}
	}
}
