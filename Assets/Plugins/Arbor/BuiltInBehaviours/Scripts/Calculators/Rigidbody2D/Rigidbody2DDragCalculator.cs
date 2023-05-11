//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// オブジェクトの抵抗
	/// </summary>
#else
	/// <summary>
	/// The drag of the object.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.Drag")]
	[BehaviourTitle("Rigidbody2D.Drag")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DDragCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトの抵抗
		/// </summary>
#else
		/// <summary>
		/// The drag of the object.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Drag = new OutputSlotFloat();

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
				_Drag.SetValue(rigidbody2D.drag);
			}
		}
	}
}
