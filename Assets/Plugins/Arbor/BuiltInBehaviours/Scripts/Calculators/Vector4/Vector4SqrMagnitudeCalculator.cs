//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ベクトルの 2 乗の長さを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the length of the square of the vector.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.SqrMagnitude")]
	[BehaviourTitle("Vector4.SqrMagnitude")]
	[BuiltInBehaviour]
	public sealed class Vector4SqrMagnitudeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector4
		/// </summary>
		[SerializeField] private FlexibleVector4 _Vector4 = new FlexibleVector4();

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
			_Result.SetValue(_Vector4.value.sqrMagnitude);
		}
	}
}
