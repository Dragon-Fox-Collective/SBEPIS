//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2を分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Vector2.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Decompose")]
	[BehaviourTitle("Vector2.Decompose")]
	[BuiltInBehaviour]
	public sealed class Vector2DecomposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力値
		/// </summary>
#else
		/// <summary>
		/// Input value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Input = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// X座標の値
		/// </summary>
#else
		/// <summary>
		/// X coordinate value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _X = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y座標の値
		/// </summary>
#else
		/// <summary>
		/// Y coordinate value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Y = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector2 v = _Input.value;
			_X.SetValue(v.x);
			_Y.SetValue(v.y);
		}
	}
}
