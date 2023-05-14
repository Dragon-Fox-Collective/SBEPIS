//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3の各値にCeilToIntを行ってVector3Intへ変換する。
	/// </summary>
#else
	/// <summary>
	/// Converts a Vector3 to a Vector3Int by doing a Ceiling to each value.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.CeilToInt")]
	[BehaviourTitle("Vector3Int.CeilToInt")]
	[BuiltInBehaviour]
	public sealed class Vector3IntCeilToIntCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3の値
		/// </summary>
#else
		/// <summary>
		/// Vector3 value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

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

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Vector3Int.CeilToInt(_Vector3.value));
		}
	}
}