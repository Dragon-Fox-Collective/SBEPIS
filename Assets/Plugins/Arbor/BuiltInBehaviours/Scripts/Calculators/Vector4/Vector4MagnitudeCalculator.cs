//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ベクトルの長さ
	/// </summary>
#else
	/// <summary>
	/// The length of vector.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.Magnitude")]
	[BehaviourTitle("Vector4.Magnitude")]
	[BuiltInBehaviour]
	public sealed class Vector4MagnitudeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector
		/// </summary>
#endif
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
			_Result.SetValue(_Vector4.value.magnitude);
		}
	}
}
