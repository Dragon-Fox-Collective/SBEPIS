//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ラジアン/秒で測定されたRigidbodyの角速度
	/// </summary>
#else
	/// <summary>
	/// The angular velocity of the rigidbody measured in radians per second.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.AngularVelocity")]
	[BehaviourTitle("Rigidbody.AngularVelocity")]
	[BuiltInBehaviour]
	public sealed class RigidbodyAngularVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 角速度
		/// </summary>
#else
		/// <summary>
		/// The angular velocity
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _AngularVelocity = new OutputSlotVector3();

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
				_AngularVelocity.SetValue(rigidbody.angularVelocity);
			}
		}
	}
}
