//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collider2DにアタッチされているRigidbody2Dを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Rigidbody2D attached to Collider2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collider2D/Collider2D.AttachedRigidbody")]
	[BehaviourTitle("Collider2D.AttachedRigidbody")]
	[BuiltInBehaviour]
	public sealed class Collider2DAttachedRigidbodyCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collider2D
		/// </summary>
		[SerializeField] private InputSlotCollider2D _Collider2D = new InputSlotCollider2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力。<br/>
		/// Collider2DがRigidbody2Dをアタッチしていない場合、null を返します。
		/// </summary>
#else
		/// <summary>
		/// Result output<br/>
		/// Returns null if the Collider2D is attached to no Rigidbody2D.
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody2D _AttachedRigidbody = new OutputSlotRigidbody2D();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collider2D collider2D = null;
			_Collider2D.GetValue(ref collider2D);
			if (collider2D != null)
			{
				_AttachedRigidbody.SetValue(collider2D.attachedRigidbody);
			}
		}
	}
}
