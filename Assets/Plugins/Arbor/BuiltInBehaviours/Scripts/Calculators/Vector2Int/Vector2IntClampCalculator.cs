//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2Intをminとmaxで指定された境界にクランプします。
	/// </summary>
#else
	/// <summary>
	/// Clamps the Vector2Int to the bounds given by min and max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.Clamp")]
	[BehaviourTitle("Vector2Int.Clamp")]
	[BuiltInBehaviour]
	public sealed class Vector2IntClampCalculator : Calculator
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
		/// 最小値
		/// </summary>
#else
		/// <summary>
		/// Min
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Min = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大値
		/// </summary>
#else
		/// <summary>
		/// Max
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Max = new FlexibleVector2Int();

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
			v.Clamp(_Min.value, _Max.value);
			_Result.SetValue(v);
		}
	}
}