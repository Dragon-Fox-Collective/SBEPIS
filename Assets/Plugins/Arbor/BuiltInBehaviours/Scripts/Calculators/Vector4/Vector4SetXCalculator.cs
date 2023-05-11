//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4のX成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the X component of Vector4.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.SetX")]
	[BehaviourTitle("Vector4.SetX")]
	[BuiltInBehaviour]
	public sealed class Vector4SetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector4
		/// </summary>
		[SerializeField] private FlexibleVector4 _Vector4 = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// X成分
		/// </summary>
#else
		/// <summary>
		/// X component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _X = new FlexibleFloat();

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
			Vector4 value = _Vector4.value;
			value.x = _X.value;
			_Result.SetValue(value);
		}
	}
}
