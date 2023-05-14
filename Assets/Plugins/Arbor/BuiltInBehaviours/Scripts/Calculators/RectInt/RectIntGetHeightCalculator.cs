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
	[AddBehaviourMenu("RectInt/RectInt.GetHeight")]
	[BehaviourTitle("RectInt.GetHeight")]
	[BuiltInBehaviour]
	public sealed class RectIntGetHeightCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 高さの出力
		/// </summary>
#else
		/// <summary>
		/// Output height
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Height = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Height.SetValue(_RectInt.value.height);
		}
	}
}