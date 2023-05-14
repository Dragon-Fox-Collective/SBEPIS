//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// オブジェクトの回転に対する抵抗
	/// </summary>
#else
	/// <summary>
	/// The angular drag of the object.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.AngularDrag")]
	[BehaviourTitle("Rigidbody2D.AngularDrag")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DAngularDragCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトの回転に対する抵抗
		/// </summary>
#else
		/// <summary>
		/// The angular drag of the object.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _AngularDrag = new OutputSlotFloat();

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
				_AngularDrag.SetValue(rigidbody2D.angularDrag);
			}
		}
	}
}
