//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntをVector3に変換する。
	/// </summary>
#else
	/// <summary>
	/// Vector3Int is converted to Vector3.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.ToVector3")]
	[BehaviourTitle("Vector3Int.ToVector3")]
	[BuiltInBehaviour]
	public sealed class Vector3IntToVector3Calculator : Calculator
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
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue((Vector3)_Vector3Int.value);
		}
	}
}