//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2Intの符号を反転する。
	/// </summary>
#else
	/// <summary>
	/// To invert the sign of the Vector2Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.Negative")]
	[BehaviourTitle("Vector2Int.Negative")]
	[BuiltInBehaviour]
	public sealed class Vector2IntNegativeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力ベクトル
		/// </summary>
#else
		/// <summary>
		/// Input vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Input = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Result = new OutputSlotVector2Int();

#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector2Int v = _Input.value;
			_Result.SetValue(v.Negative());
		}
	}
}