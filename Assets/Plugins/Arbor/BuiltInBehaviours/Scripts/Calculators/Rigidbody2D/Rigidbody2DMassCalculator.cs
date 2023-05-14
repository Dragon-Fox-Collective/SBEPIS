//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigidbody の重さ
	/// </summary>
#else
	/// <summary>
	/// Mass of the rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.Mass")]
	[BehaviourTitle("Rigidbody2D.Mass")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DMassCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody の重さ
		/// </summary>
#else
		/// <summary>
		/// Mass of the rigidbody.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Mass = new OutputSlotFloat();

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
				_Mass.SetValue(rigidbody2D.mass);
			}
		}
	}
}
