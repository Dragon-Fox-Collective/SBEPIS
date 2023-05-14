//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collision2DからヒットしたRigidbody2Dを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Rigidbody2D hit from Collision2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/Collision2D.Rigidbody")]
	[BehaviourTitle("Collision2D.Rigidbody")]
	[BuiltInBehaviour]
	public sealed class Collision2DRigidbodyCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision2D
		/// </summary>
		[SerializeField] private InputSlotCollision2D _Collision2D = new InputSlotCollision2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたRigidbody2D。
		/// </summary>
#else
		/// <summary>
		/// Hit the Rigidbody2D
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody2D _Rigidbody = new OutputSlotRigidbody2D();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision2D collision2D = null;
			_Collision2D.GetValue(ref collision2D);
			if (collision2D != null)
			{
				_Rigidbody.SetValue(collision2D.rigidbody);
			}
		}
	}
}
