//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// CollisionからヒットしたRigidbodyを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Rigidbody hit from Collision.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/Collision.Rigidbody")]
	[BehaviourTitle("Collision.Rigidbody")]
	[BuiltInBehaviour]
	public sealed class CollisionRigidbodyCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision
		/// </summary>
		[SerializeField] private InputSlotCollision _Collision = new InputSlotCollision();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたRigidbody。
		/// </summary>
#else
		/// <summary>
		/// Hit the Rigidbody
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody _Rigidbody = new OutputSlotRigidbody();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision collision = null;
			_Collision.GetValue(ref collision);
			if (collision != null)
			{
				_Rigidbody.SetValue(collision.rigidbody);
			}
		}
	}
}
