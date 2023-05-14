//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2IntをVector3Intに変換する。
	/// </summary>
#else
	/// <summary>
	/// Vector2Int is converted to Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.ToVector3Int")]
	[BehaviourTitle("Vector2Int.ToVector3Int")]
	[BuiltInBehaviour]
	public sealed class Vector2IntToVector3IntCalculator : Calculator
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
		[SerializeField] private OutputSlotVector3Int _Result = new OutputSlotVector3Int();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue((Vector3Int)_Vector2Int.value);
		}
	}
}