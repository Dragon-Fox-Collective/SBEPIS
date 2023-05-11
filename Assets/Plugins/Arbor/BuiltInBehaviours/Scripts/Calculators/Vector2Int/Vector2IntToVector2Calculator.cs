//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2IntをVector2に変換する。
	/// </summary>
#else
	/// <summary>
	/// Vector2Int is converted to Vector2.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.ToVector2")]
	[BehaviourTitle("Vector2Int.ToVector2")]
	[BuiltInBehaviour]
	public sealed class Vector2IntToVector2Calculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2Int
		/// </summary>
		[SerializeField] private FlexibleVector2Int _Vector2Int = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue((Vector2)_Vector2Int.value);
		}
	}
}