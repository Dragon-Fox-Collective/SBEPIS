//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のY成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the Y component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.SetY")]
	[BehaviourTitle("Rect.SetY")]
	[BuiltInBehaviour]
	public sealed class RectSetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分
		/// </summary>
#else
		/// <summary>
		/// Y component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotRect _Result = new OutputSlotRect();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect value = _Rect.value;
			value.y = _Y.value;
			_Result.SetValue(value);
		}
	}
}
