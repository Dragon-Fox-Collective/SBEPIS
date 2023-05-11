//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectIntを位置とサイズに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose RectInt into position and size.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.DecomposeVec")]
	[BehaviourTitle("RectInt.DecomposeVec")]
	[BuiltInBehaviour]
	public sealed class RectIntDecomposeVecCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の位置を出力
		/// </summary>
#else
		/// <summary>
		/// Output the position of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Position = new OutputSlotVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズを出力
		/// </summary>
#else
		/// <summary>
		/// Output the rectangle size.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Size = new OutputSlotVector2Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rect = _RectInt.value;
			_Position.SetValue(rect.position);
			_Size.SetValue(rect.size);
		}
	}
}