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
	/// 矩形の最大コーナーの位置を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the position of the maximum corner of the rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.SetMax")]
	[BehaviourTitle("Rect.SetMax")]
	[BuiltInBehaviour]
	public sealed class RectSetMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大コーナーの位置
		/// </summary>
#else
		/// <summary>
		/// the position of the maximum corner of the rect.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Max = new FlexibleVector2();

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
			value.max = _Max.value;
			_Result.SetValue(value);
		}
	}
}
