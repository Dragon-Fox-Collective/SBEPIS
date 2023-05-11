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
	/// Vector3Intの符号を反転する。
	/// </summary>
#else
	/// <summary>
	/// To invert the sign of the Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.Negative")]
	[BehaviourTitle("Vector3Int.Negative")]
	[BuiltInBehaviour]
	public sealed class Vector3IntNegativeCalculator : Calculator
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
		[SerializeField] private FlexibleVector3Int _Input = new FlexibleVector3Int();

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
			Vector3Int v = _Input.value;
			_Result.SetValue(v.Negative());
		}
	}
}