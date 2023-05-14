//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigdibody の回転
	/// </summary>
#else
	/// <summary>
	/// The rotation of the rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.Rotation")]
	[BehaviourTitle("Rigidbody2D.Rotation")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DRotationCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigdibody の回転
		/// </summary>
#else
		/// <summary>
		/// The rotation of the rigidbody.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Rotation = new OutputSlotFloat();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody2D rigidbody2D = _Rigidbody2D.value;
			if (rigidbody2D != null)
			{
				_Rotation.SetValue(rigidbody2D.rotation);
			}
		}
	}
}
