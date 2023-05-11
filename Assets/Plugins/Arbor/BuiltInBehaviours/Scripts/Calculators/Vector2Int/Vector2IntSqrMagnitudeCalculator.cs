//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ベクトルの2乗の長さを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the length of the square of the vector.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.SqrMagnitude")]
	[BehaviourTitle("Vector2Int.SqrMagnitude")]
	[BuiltInBehaviour]
	public sealed class Vector2IntSqrMagnitudeCalculator : Calculator
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
		[SerializeField] private FlexibleVector2Int _Vector2Int = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Result = new OutputSlotInt();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(_Vector2Int.value.sqrMagnitude);
		}
	}
}