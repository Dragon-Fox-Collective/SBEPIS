//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// CollisionからヒットしたColliderを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Collider hit from Collision.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/Collision.Collider")]
	[BehaviourTitle("Collision.Collider")]
	[BuiltInBehaviour]
	public sealed class CollisionColliderCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision
		/// </summary>
		[SerializeField] private InputSlotCollision _Collision = new InputSlotCollision();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたCollider。
		/// </summary>
#else
		/// <summary>
		/// Collider hit.
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollider _Collider = new OutputSlotCollider();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision collision = null;
			_Collision.GetValue(ref collision);
			if (collision != null)
			{
				_Collider.SetValue(collision.collider);
			}
		}
	}
}
