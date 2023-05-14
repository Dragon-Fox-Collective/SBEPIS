//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突した2つのオブジェクトの相対速度を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the relative velocity of the two collided objects.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/Collision.RelativeVelocity")]
	[BehaviourTitle("Collision.RelativeVelocity")]
	[BuiltInBehaviour]
	public sealed class CollisionRelativeVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision
		/// </summary>
		[SerializeField] private InputSlotCollision _Collision = new InputSlotCollision();

#if ARBOR_DOC_JA
		/// <summary>
		/// 相対速度。
		/// </summary>
#else
		/// <summary>
		/// Relative velocity.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _RelativeVelocity = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision collision = null;
			_Collision.GetValue(ref collision);
			if (collision != null)
			{
				_RelativeVelocity.SetValue(collision.relativeVelocity);
			}
		}
	}
}
