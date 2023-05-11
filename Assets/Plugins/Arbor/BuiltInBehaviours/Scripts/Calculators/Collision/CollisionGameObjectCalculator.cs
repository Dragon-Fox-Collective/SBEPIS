//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// CollisionからヒットしたGameObjectを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output GameObject hit from Collision.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/Collision.GameObject")]
	[BehaviourTitle("Collision.GameObject")]
	[BuiltInBehaviour]
	public sealed class CollisionGameObjectCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision
		/// </summary>
		[SerializeField] private InputSlotCollision _Collision = new InputSlotCollision();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたGameObject。
		/// </summary>
#else
		/// <summary>
		/// GameObject that hit.
		/// </summary>
#endif
		[SerializeField] private OutputSlotGameObject _GameObject = new OutputSlotGameObject();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision collision = null;
			_Collision.GetValue(ref collision);
			if (collision != null)
			{
				_GameObject.SetValue(collision.gameObject);
			}
		}
	}
}
