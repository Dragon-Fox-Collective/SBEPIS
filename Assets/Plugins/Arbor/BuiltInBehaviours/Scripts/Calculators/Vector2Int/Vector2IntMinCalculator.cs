//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのベクトルで各成分の一番小さな値を使用してベクトルを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create a vector that is made from the smallest components of two vectors.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.Min")]
	[BehaviourTitle("Vector2Int.Min")]
	[BuiltInBehaviour]
	public sealed class Vector2IntMinCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 左側の値
		/// </summary>
#else
		/// <summary>
		/// Left side value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Lhs = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 右側の値
		/// </summary>
#else
		/// <summary>
		/// Right side value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Rhs = new FlexibleVector2Int();

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
			_Result.SetValue(Vector2Int.Min(_Lhs.value, _Rhs.value));
		}
	}
}