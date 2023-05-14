//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// リジッドボディの回転慣性
	/// </summary>
#else
	/// <summary>
	/// The rigidBody rotational inertia.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.Inertia")]
	[BehaviourTitle("Rigidbody2D.Inertia")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DInertiaCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// リジッドボディの回転慣性
		/// </summary>
#else
		/// <summary>
		/// The rigidBody rotational inertia.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Inertia = new OutputSlotFloat();

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
				_Inertia.SetValue(rigidbody2D.inertia);
			}
		}
	}
}
