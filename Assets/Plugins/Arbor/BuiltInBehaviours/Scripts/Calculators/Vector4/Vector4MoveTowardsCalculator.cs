//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 現在の位置CurrentからTargetに向けて移動するベクトルを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculating a vector that moves from the current position Current to Target.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.MoveTowards")]
	[BehaviourTitle("Vector4.MoveTowards")]
	[BuiltInBehaviour]
	public sealed class Vector4MoveTowardsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の位置
		/// </summary>
#else
		/// <summary>
		/// Current position
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _Current = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標の位置
		/// </summary>
#else
		/// <summary>
		/// Target position
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _Target = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大の移動量
		/// </summary>
#else
		/// <summary>
		/// The maximum amount of movement
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxDistanceDelta = new FlexibleFloat();

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
			_Result.SetValue(Vector4.MoveTowards(_Current.value, _Target.value, _MaxDistanceDelta.value));
		}
	}
}
