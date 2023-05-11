//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2の各値にCeilToIntを行ってVector2Intへ変換する。
	/// </summary>
#else
	/// <summary>
	/// Converts a Vector2 to a Vector2Int by doing a Ceiling to each value.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.CeilToInt")]
	[BehaviourTitle("Vector2Int.CeilToInt")]
	[BuiltInBehaviour]
	public sealed class Vector2IntCeilToIntCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2の値
		/// </summary>
#else
		/// <summary>
		/// Vector2 value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Vector2 = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2Int _Result = new OutputSlotVector2Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Vector2Int.CeilToInt(_Vector2.value));
		}
	}
}