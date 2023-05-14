//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 空間上でColliderがある地点を含むかチェックする。
	/// </summary>
#else
	/// <summary>
	/// Check if a collider overlaps a point in space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collider2D/Collider2D.OverlapPoint")]
	[BehaviourTitle("Collider2D.OverlapPoint")]
	[BuiltInBehaviour]
	public sealed class Collider2DOverlapPointCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collider2D
		/// </summary>
		[SerializeField] private InputSlotCollider2D _Collider2D = new InputSlotCollider2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// ワールド空間上の点
		/// </summary>
#else
		/// <summary>
		/// A point in world space.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Point = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Collider2D collider2D = null;
			_Collider2D.GetValue(ref collider2D);
			if (collider2D != null)
			{
				_Result.SetValue(collider2D.OverlapPoint(_Point.value));
			}
		}
	}
}
