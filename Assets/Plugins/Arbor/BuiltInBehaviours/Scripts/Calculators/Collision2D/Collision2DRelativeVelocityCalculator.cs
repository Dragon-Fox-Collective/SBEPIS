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
	[AddBehaviourMenu("Collision2D/Collision2D.RelativeVelocity")]
	[BehaviourTitle("Collision2D.RelativeVelocity")]
	[BuiltInBehaviour]
	public sealed class Collision2DRelativeVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision2D
		/// </summary>
		[SerializeField] private InputSlotCollision2D _Collision2D = new InputSlotCollision2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 相対速度。
		/// </summary>
#else
		/// <summary>
		/// Relative velocity.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _RelativeVelocity = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision2D collision2D = null;
			_Collision2D.GetValue(ref collision2D);
			if (collision2D != null)
			{
				_RelativeVelocity.SetValue(collision2D.relativeVelocity);
			}
		}
	}
}
