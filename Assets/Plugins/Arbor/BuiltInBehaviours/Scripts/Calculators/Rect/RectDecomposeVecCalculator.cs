//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rectを位置とサイズに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Rect into position and size.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.DecomposeVec")]
	[BehaviourTitle("Rect.DecomposeVec")]
	[BuiltInBehaviour]
	public sealed class RectDecomposeVecCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の位置を出力
		/// </summary>
#else
		/// <summary>
		/// Output the position of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Position = new OutputSlotVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズを出力
		/// </summary>
#else
		/// <summary>
		/// Output the rectangle size.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Size = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect rect = _Rect.value;
			_Position.SetValue(rect.position);
			_Size.SetValue(rect.size);
		}
	}
}
