﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2を作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Vector2.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Compose")]
	[BehaviourTitle("Vector2.Compose")]
	[BuiltInBehaviour]
	public sealed class Vector2ComposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// X座標の値
		/// </summary>
#else
		/// <summary>
		/// X coordinate value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _X = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y座標の値
		/// </summary>
#else
		/// <summary>
		/// Y coordinate value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

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

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(new Vector2(_X.value, _Y.value));
		}
	}
}
