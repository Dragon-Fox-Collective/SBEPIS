//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のサイズを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the size of the rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetSize")]
	[BehaviourTitle("Rect.GetSize")]
	[BuiltInBehaviour]
	public sealed class RectGetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズの出力。
		/// </summary>
#else
		/// <summary>
		/// Result the size of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Size = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Size.SetValue(_Rect.value.size);
		}
	}
}
