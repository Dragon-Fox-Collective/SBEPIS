//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorのA成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the A component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.SetA")]
	[BehaviourTitle("Color.SetA")]
	[BuiltInBehaviour]
	public sealed class ColorSetACalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// A成分
		/// </summary>
#else
		/// <summary>
		/// A component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _A = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotColor _Result = new OutputSlotColor();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Color value = _Color.value;
			value.a = _A.value;
			_Result.SetValue(value);
		}
	}
}
