//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2IntのX成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the X component of Vector2Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.SetX")]
	[BehaviourTitle("Vector2Int.SetX")]
	[BuiltInBehaviour]
	public sealed class Vector2IntSetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2Int
		/// </summary>
		[SerializeField] private FlexibleVector2Int _Vector2Int = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// X成分
		/// </summary>
#else
		/// <summary>
		/// X component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _X = new FlexibleInt();

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

		public override void OnCalculate()
		{
			Vector2Int value = _Vector2Int.value;
			value.x = _X.value;
			_Result.SetValue(value);
		}
	}
}