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
	/// 矩形のサイズを設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the size of the rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.SetSize")]
	[BehaviourTitle("Rect.SetSize")]
	[BuiltInBehaviour]
	public sealed class RectSetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズ
		/// </summary>
#else
		/// <summary>
		/// the size of the rect.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Size = new FlexibleVector2();

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
			value.size = _Size.value;
			_Result.SetValue(value);
		}
	}
}
