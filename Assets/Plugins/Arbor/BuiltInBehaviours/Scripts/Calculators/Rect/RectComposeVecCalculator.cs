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
	[AddBehaviourMenu("Rect/Rect.ComposeVec")]
	[BehaviourTitle("Rect.ComposeVec")]
	[BuiltInBehaviour]
	public sealed class RectComposeVecCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の位置。
		/// </summary>
#else
		/// <summary>
		/// The position of the rect.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Position = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズ
		/// </summary>
#else
		/// <summary>
		/// Rectangle size
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Size = new FlexibleVector2();

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
			_Result.SetValue(new Rect(_Position.value, _Size.value));
		}
	}
}
