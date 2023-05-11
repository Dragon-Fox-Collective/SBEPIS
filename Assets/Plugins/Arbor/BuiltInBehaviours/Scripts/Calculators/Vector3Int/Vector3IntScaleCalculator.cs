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
	[AddBehaviourMenu("Vector3Int/Vector3Int.Scale")]
	[BehaviourTitle("Vector3Int.Scale")]
	[BuiltInBehaviour]
	public sealed class Vector3IntScaleCalculator : Calculator
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
		/// 乗算するベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector to multiply
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Scale = new FlexibleVector3Int();

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
			_Result.SetValue(Vector3Int.Scale(_Input.value, _Scale.value));
		}
	}
}