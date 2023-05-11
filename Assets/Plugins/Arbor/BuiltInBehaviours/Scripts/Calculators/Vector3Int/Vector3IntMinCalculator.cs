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
	[AddBehaviourMenu("Vector3Int/Vector3Int.Min")]
	[BehaviourTitle("Vector3Int.Min")]
	[BuiltInBehaviour]
	public sealed class Vector3IntMinCalculator : Calculator
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
		[SerializeField] private FlexibleVector3Int _Lhs = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 右側の値
		/// </summary>
#else
		/// <summary>
		/// Right side value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Rhs = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Result = new OutputSlotVector3Int();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector3Int.Min(_Lhs.value, _Rhs.value));
		}
	}
}