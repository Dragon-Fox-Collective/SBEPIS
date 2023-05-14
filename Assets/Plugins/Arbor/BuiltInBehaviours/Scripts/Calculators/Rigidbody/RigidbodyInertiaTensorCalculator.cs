//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 質量の中心に対する、相対的な対角慣性テンソル
	/// </summary>
#else
	/// <summary>
	/// The diagonal inertia tensor of mass relative to the center of mass.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.InertiaTensor")]
	[BehaviourTitle("Rigidbody.InertiaTensor")]
	[BuiltInBehaviour]
	public sealed class RigidbodyInertiaTensorCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 相対的な対角慣性テンソル
		/// </summary>
#else
		/// <summary>
		/// The diagonal inertia tensor of mass relative
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _InertiaTensor = new OutputSlotVector3();

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
				_InertiaTensor.SetValue(rigidbody.inertiaTensor);
			}
		}
	}
}
