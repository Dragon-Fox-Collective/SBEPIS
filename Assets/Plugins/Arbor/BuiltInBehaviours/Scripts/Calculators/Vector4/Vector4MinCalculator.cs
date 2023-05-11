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
	[AddBehaviourMenu("Vector4/Vector4.Min")]
	[BehaviourTitle("Vector4.Min")]
	[BuiltInBehaviour]
	public sealed class Vector4MinCalculator : Calculator
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
		[SerializeField] private FlexibleVector4 _Lhs = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 右側の値
		/// </summary>
#else
		/// <summary>
		/// Right side value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _Rhs = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector4 _Result = new OutputSlotVector4();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector4.Min(_Lhs.value, _Rhs.value));
		}
	}
}
