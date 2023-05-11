//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド座標による質量の中心値
	/// </summary>
#else
	/// <summary>
	/// The center of mass of the rigidbody in world space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.WorldCenterOfMass")]
	[BehaviourTitle("Rigidbody.WorldCenterOfMass")]
	[BuiltInBehaviour]
	public sealed class RigidbodyWorldCenterOfMassCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 質量の中心値
		/// </summary>
#else
		/// <summary>
		/// The center of mass.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _WorldCenterOfMass = new OutputSlotVector3();

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
				_WorldCenterOfMass.SetValue(rigidbody.worldCenterOfMass);
			}
		}
	}
}
