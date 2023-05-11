﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド座標でのCollider2DのBounds情報
	/// </summary>
#else
	/// <summary>
	/// The world space bounding volume of the Collider2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collider2D/Collider2D.Bounds")]
	[BehaviourTitle("Collider2D.Bounds")]
	[BuiltInBehaviour]
	public sealed class Collider2DBoundsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collider2D
		/// </summary>
		[SerializeField] private InputSlotCollider2D _Collider2D = new InputSlotCollider2D();

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
			Collider2D collider2D = null;
			_Collider2D.GetValue(ref collider2D);
			if (collider2D != null)
			{
				_Bounds.SetValue(collider2D.bounds);
			}
		}
	}
}
