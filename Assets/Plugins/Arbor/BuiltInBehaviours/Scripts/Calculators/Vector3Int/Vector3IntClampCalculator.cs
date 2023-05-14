//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3Intをminとmaxで指定された境界にクランプします。
	/// </summary>
#else
	/// <summary>
	/// Clamps the Vector3Int to the bounds given by min and max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.Clamp")]
	[BehaviourTitle("Vector3Int.Clamp")]
	[BuiltInBehaviour]
	public sealed class Vector3IntClampCalculator : Calculator
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
		/// 最小値
		/// </summary>
#else
		/// <summary>
		/// Min
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Min = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大値
		/// </summary>
#else
		/// <summary>
		/// Max
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Max = new FlexibleVector3Int();

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
			v.Clamp(_Min.value, _Max.value);
			_Result.SetValue(v);
		}
	}
}