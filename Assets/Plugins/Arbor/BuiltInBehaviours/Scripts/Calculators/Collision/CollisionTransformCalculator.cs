//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// CollisionからヒットしたTransformを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Transform hit from Collision.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/Collision.Transform")]
	[BehaviourTitle("Collision.Transform")]
	[BuiltInBehaviour]
	public sealed class CollisionTransformCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision
		/// </summary>
		[SerializeField] private InputSlotCollision _Collision = new InputSlotCollision();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform hit.
		/// </summary>
#endif
		[SerializeField] private OutputSlotTransform _Transform = new OutputSlotTransform();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision collision = null;
			_Collision.GetValue(ref collision);
			if (collision != null)
			{
				_Transform.SetValue(collision.transform);
			}
		}
	}
}
