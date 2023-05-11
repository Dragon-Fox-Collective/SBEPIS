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
	[AddBehaviourMenu("RectInt/RectInt.ComposeVec")]
	[BehaviourTitle("RectInt.ComposeVec")]
	[BuiltInBehaviour]
	public sealed class RectIntComposeVecCalculator : Calculator
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
		[SerializeField] private FlexibleVector2Int _Position = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のサイズ
		/// </summary>
#else
		/// <summary>
		/// RectIntangle size
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Size = new FlexibleVector2Int();

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
			_Result.SetValue(new RectInt(_Position.value, _Size.value));
		}
	}
}