//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColliderにアタッチされているRigidbodyを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Rigidbody attached to Collider.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collider/Collider.AttachedRigidbody")]
	[BehaviourTitle("Collider.AttachedRigidbody")]
	[BuiltInBehaviour]
	public sealed class ColliderAttachedRigidbodyCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collider
		/// </summary>
		[SerializeField] private InputSlotCollider _Collider = new InputSlotCollider();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力。<br/>
		/// ColliderがRigidbodyをアタッチしていない場合、null を返します。
		/// </summary>
#else
		/// <summary>
		/// Result output<br/>
		/// Returns null if the collider is attached to no rigidbody.
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody _AttachedRigidbody = new OutputSlotRigidbody();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collider collider = null;
			_Collider.GetValue(ref collider);
			if (collider != null)
			{
				_AttachedRigidbody.SetValue(collider.attachedRigidbody);
			}
		}
	}
}
