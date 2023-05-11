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
	[AddBehaviourMenu("RectInt/RectInt.GetSize")]
	[BehaviourTitle("RectInt.GetSize")]
	[BuiltInBehaviour]
	public sealed class RectIntGetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズの出力。
		/// </summary>
#else
		/// <summary>
		/// Result the size of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Size = new OutputSlotVector2Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Size.SetValue(_RectInt.value.size);
		}
	}
}