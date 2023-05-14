//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2 つのベクトルの各成分を乗算する。
	/// </summary>
#else
	/// <summary>
	/// Multiplies two vectors component-wise.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.Scale")]
	[BehaviourTitle("Vector4.Scale")]
	[BuiltInBehaviour]
	public sealed class Vector4ScaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector4
		/// </summary>
		[SerializeField] private FlexibleVector4 _Vector4 = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 乗算するベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector to multiply
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _Scale = new FlexibleVector4();

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
			_Result.SetValue(Vector4.Scale(_Vector4.value, _Scale.value));
		}
	}
}
