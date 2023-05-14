﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// リジッドボディのローカル空間において指定された RelativePoint のグローバル空間ポイントを取得する。
	/// </summary>
#else
	/// <summary>
	/// Get a global space point given the point relativePoint in rigidBody local space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.GetRelativePoint")]
	[BehaviourTitle("Rigidbody2D.GetRelativePoint")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DGetRelativePointCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ローカル座標
		/// </summary>
#else
		/// <summary>
		/// Local coordinates
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _RelativePoint = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody2D rigidbody2D = _Rigidbody2D.value;
			if (rigidbody2D != null)
			{
				_Result.SetValue(rigidbody2D.GetRelativePoint(_RelativePoint.value));
			}
		}
	}
}
