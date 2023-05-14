//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boolをフォーマットした文字列を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns a formatted string of Bool.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bool/Bool.ToString")]
	[BehaviourTitle("Bool.ToString")]
	[BuiltInBehaviour]
	public sealed class BoolToStringCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bool
		/// </summary>
		[SerializeField] private FlexibleBool _Bool = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotString _Result = new OutputSlotString();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			bool value = _Bool.value;
			string str = value.ToString();
			_Result.SetValue(str);
		}
	}
}
