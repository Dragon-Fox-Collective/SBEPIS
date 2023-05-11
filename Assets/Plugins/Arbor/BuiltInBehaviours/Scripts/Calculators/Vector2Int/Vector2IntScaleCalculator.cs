//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのベクトルの各成分を乗算する。
	/// </summary>
#else
	/// <summary>
	/// Multiplies two vectors component-wise.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.Scale")]
	[BehaviourTitle("Vector2Int.Scale")]
	[BuiltInBehaviour]
	public sealed class Vector2IntScaleCalculator : Calculator
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
		/// 乗算するベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector to multiply
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Scale = new FlexibleVector2Int();

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
			_Result.SetValue(Vector2Int.Scale(_Input.value, _Scale.value));
		}
	}
}