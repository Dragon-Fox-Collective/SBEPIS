//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// グローバル空間でのリジッドボディの質量の中心を取得する。
	/// </summary>
#else
	/// <summary>
	/// Gets the center of mass of the rigidBody in global space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.WorldCenterOfMass")]
	[BehaviourTitle("Rigidbody2D.WorldCenterOfMass")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DWorldCenterOfMassCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// グローバル空間でのリジッドボディの質量の中心。
		/// </summary>
#else
		/// <summary>
		/// The center of mass of the rigidBody in global space.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _WorldCenterOfMass = new OutputSlotVector2();

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
				_WorldCenterOfMass.SetValue(rigidbody2D.worldCenterOfMass);
			}
		}
	}
}
