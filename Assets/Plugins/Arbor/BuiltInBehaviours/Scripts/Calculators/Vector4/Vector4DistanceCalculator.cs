//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AとBの間の距離を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates the distance between A and B.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.Distance")]
	[BehaviourTitle("Vector4.Distance")]
	[BuiltInBehaviour]
	public sealed class Vector4DistanceCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ベクトルA
		/// </summary>
#else
		/// <summary>
		/// Vector A
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _A = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// ベクトルB
		/// </summary>
#else
		/// <summary>
		/// Vector B
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _B = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector4.Distance(_A.value, _B.value));
		}
	}
}
