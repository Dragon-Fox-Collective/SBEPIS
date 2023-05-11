//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntをVector2Intに変換する。
	/// </summary>
#else
	/// <summary>
	/// Vector3Int is converted to Vector2Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.ToVector2Int")]
	[BehaviourTitle("Vector3Int.ToVector2Int")]
	[BuiltInBehaviour]
	public sealed class Vector3IntToVector2IntCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3Int
		/// </summary>
		[SerializeField] private FlexibleVector3Int _Vector3Int = new FlexibleVector3Int();

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
			_Result.SetValue((Vector2Int)_Vector3Int.value);
		}
	}
}