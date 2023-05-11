//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のY成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the Y component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.GetY")]
	[BehaviourTitle("RectInt.GetY")]
	[BuiltInBehaviour]
	public sealed class RectIntGetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Y component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Y = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Y.SetValue(_RectInt.value.y);
		}
	}
}