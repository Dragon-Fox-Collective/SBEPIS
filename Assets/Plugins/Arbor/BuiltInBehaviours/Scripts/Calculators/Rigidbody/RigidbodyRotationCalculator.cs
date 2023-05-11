//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigidbody の回転
	/// </summary>
#else
	/// <summary>
	/// The rotation of the rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.Rotation")]
	[BehaviourTitle("Rigidbody.Rotation")]
	[BuiltInBehaviour]
	public sealed class RigidbodyRotationCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 回転
		/// </summary>
#else
		/// <summary>
		/// The rotation.
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
			Rigidbody rigidbody = _Rigidbody.value;
			if (rigidbody != null)
			{
				_Rotation.SetValue(rigidbody.rotation);
			}
		}
	}
}
