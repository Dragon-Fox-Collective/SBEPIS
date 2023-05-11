//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collision2DからヒットしたCollider2Dを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Collider2D hit from Collision2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/Collision2D.Collider")]
	[BehaviourTitle("Collision2D.Collider")]
	[BuiltInBehaviour]
	public sealed class Collision2DColliderCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision2D
		/// </summary>
		[SerializeField] private InputSlotCollision2D _Collision2D = new InputSlotCollision2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたCollider2D。
		/// </summary>
#else
		/// <summary>
		/// Collider2D hit.
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollider2D _Collider = new OutputSlotCollider2D();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision2D collision2D = null;
			_Collision2D.GetValue(ref collision2D);
			if (collision2D != null)
			{
				_Collider.SetValue(collision2D.collider);
			}
		}
	}
}
