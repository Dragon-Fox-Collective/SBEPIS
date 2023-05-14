//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectIntを作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose RectInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.Compose")]
	[BehaviourTitle("RectInt.Compose")]
	[BuiltInBehaviour]
	public sealed class RectIntComposeCalculator : Calculator
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
		[SerializeField] private FlexibleInt _X = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形を作成する基準点の Y 値
		/// </summary>
#else
		/// <summary>
		/// The Y value the rect is measured from.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _Y = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の幅
		/// </summary>
#else
		/// <summary>
		/// The width of the rectangle.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _Width = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の高さ
		/// </summary>
#else
		/// <summary>
		/// 	The height of the rectangle.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _Height = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRectInt _Result = new OutputSlotRectInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(new RectInt(_X.value, _Y.value, _Width.value, _Height.value));
		}
	}
}