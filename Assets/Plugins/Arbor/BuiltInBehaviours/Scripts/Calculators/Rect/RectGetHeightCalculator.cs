//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の高さを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the height of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetHeight")]
	[BehaviourTitle("Rect.GetHeight")]
	[BuiltInBehaviour]
	public sealed class RectGetHeightCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 高さの出力
		/// </summary>
#else
		/// <summary>
		/// Output height
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Height = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Height.SetValue(_Rect.value.height);
		}
	}
}
