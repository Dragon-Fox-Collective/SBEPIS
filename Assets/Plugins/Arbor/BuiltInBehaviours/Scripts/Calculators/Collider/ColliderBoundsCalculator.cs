//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド座標でのColliderの Bounds 情報
	/// </summary>
#else
	/// <summary>
	/// The world space bounding volume of the collider.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collider/Collider.Bounds")]
	[BehaviourTitle("Collider.Bounds")]
	[BuiltInBehaviour]
	public sealed class ColliderBoundsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private InputSlotCollider _Collider = new InputSlotCollider();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBounds _Bounds = new OutputSlotBounds();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Collider collider = null;
			_Collider.GetValue(ref collider);
			if (collider != null)
			{
				_Bounds.SetValue(collider.bounds);
			}
		}
	}
}
