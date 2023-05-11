//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rectを作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.Compose")]
	[BehaviourTitle("Rect.Compose")]
	[BuiltInBehaviour]
	public sealed class RectComposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形を作成する基準点の X 値
		/// </summary>
#else
		/// <summary>
		/// The X value the rect is measured from.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _X = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形を作成する基準点の Y 値
		/// </summary>
#else
		/// <summary>
		/// The Y value the rect is measured from.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の幅
		/// </summary>
#else
		/// <summary>
		/// The width of the rectangle.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Width = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の高さ
		/// </summary>
#else
		/// <summary>
		/// 	The height of the rectangle.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Height = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRect _Result = new OutputSlotRect();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(new Rect(_X.value, _Y.value, _Width.value, _Height.value));
		}
	}
}
