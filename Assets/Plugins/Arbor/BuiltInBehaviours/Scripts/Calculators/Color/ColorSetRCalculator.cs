//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorのR成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the R component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.SetR")]
	[BehaviourTitle("Color.SetR")]
	[BuiltInBehaviour]
	public sealed class ColorSetRCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// R成分
		/// </summary>
#else
		/// <summary>
		/// R component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _R = new FlexibleFloat();

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
			value.r = _R.value;
			_Result.SetValue(value);
		}
	}
}
